using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nito.AsyncEx;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Bll;
using WeCollect.App.Blob;
using WeCollect.App.Documents;
using WeCollect.App.Web3;
using WeCollect.Server.Models;

namespace WeCollect
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _startupWaitHandle = new AsyncManualResetEvent();
            StartupComplete = Task.Run(async () => { await _startupWaitHandle.WaitAsync(); });

            Configuration = configuration;
        }

        private static AsyncManualResetEvent _startupWaitHandle;

        public static Task StartupComplete { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        public static Container Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
            services.Configure<RazorViewEngineOptions>(o =>
            {
                // {2} is area, {1} is controller,{0} is the action    
                o.ViewLocationFormats.Add("/Server/Views/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Server/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Server/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });









            // build the container
            Container = new Container();
            services.AddSingleton(Container);
            var config = Container.Config = Configuration.Get<ServerConfiguration>();
            
            var blobService = Container.BlobService = await BlobService.Create(
                new Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer(
                    config.StorageAccountBlobsUri,
                    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                        config.StorageAccountName,
                        config.StorageAccountKey)));

            var web3 = Container.Web3 = new Nethereum.Web3.Web3(Configuration["web3Url"]);

            var contracts = Container.ContractArtifacts = ContractArtifacts.Initialize().Result;


            CardDocumentDb documents = Container.Documents = new CardDocumentDb(
                new Microsoft.Azure.Documents.Client.DocumentClient(
                    Configuration.GetValue<Uri>("documentDbEndpoint"),
                    Configuration["documentDbKey"],
                    null,
                    Microsoft.Azure.Documents.ConsistencyLevel.Session),
                Container);
            await documents.EnsureDbExists();

            // Initialize contracts
            var contractsInitializer = Container.ContractsInitializer = new ContractsInitializer(Container);
            await Container.ContractsInitializer.Initialize();

            var contractDocuments = await documents.GetAllContracts();
            contracts.DocumentsByName = contractDocuments
                .Where(contract => contract.name != null)
                .ToDictionary(contract => contract.name);

            var cardsService = Container.CardsContractMethods = new Contracts.Contracts.Cards.CardsService(web3, contracts.DocumentsByName[nameof(contracts.Cards)].Address);

            Web3Db web3Db = Container.Web3Db = new Web3Db(
                web3,
                contracts,
                cardsService,
                config.Web3ServerAddress,
                config.Web3ServerPrivateKey);

            //Seed
            var cardFactory = Container.CardFactory = new CardFactory(web3Db, web3, documents, Container.Config, Container.BlobService);
            services.AddSingleton(cardFactory);
            await Seed.DoSeed(cardFactory);


            var cardEventsController = Container.CardEventsController = new ContractEventsController(Container);

            var newBlockManager = new NewBlockManager(Container, cardEventsController);


            var checkpointFactory = new BlockCheckpointFactory(web3, documents);

            // start checkpoint from contract blockid
            var cardContractCreatedBlock = Container.ContractArtifacts.DocumentsByName[Container.ContractArtifacts.Cards.Name].TransactionReceipt.BlockNumber.Value;
            var checkpoint = await checkpointFactory.GetOrCreateCheckpoint("blockManagerCheckpoint", cardContractCreatedBlock);


            // Begin BlockLoop
            var blockLoop = new NewBlockLoop(web3, checkpoint);
            blockLoop.Start();
            var _ = Task.Run(() => blockLoop.Loop(newBlockManager.OnBlock, ex => { Debugger.Break(); return Task.CompletedTask; }));


            _startupWaitHandle.Set();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<Server.Hubs.CardHub>("/api/hubs");
            });
        }
    }
}
