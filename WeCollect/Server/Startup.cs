﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Bll;
using WeCollect.App.Documents;
using WeCollect.App.Extensions;
using WeCollect.App.Web3;
using WeCollect.Server.Hubs;

namespace WeCollect
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

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

            var web3 = Container.Web3 = new Nethereum.Web3.Web3(Configuration["web3Url"]);

            var contracts = Container.ContractArtifacts = ContractArtifacts.Initialize().Result;


            CardDocumentDb documents = Container.Documents = new CardDocumentDb(
                new Microsoft.Azure.Documents.Client.DocumentClient(
                    Configuration.GetValue<Uri>("documentDbEndpoint"),
                    Configuration["documentDbKey"],
                    null,
                    Microsoft.Azure.Documents.ConsistencyLevel.Session),
                Container);

            // Initialize contracts
            var contractsInitializer = Container.ContractsInitializer = new ContractsInitializer(Container);
            await Container.ContractsInitializer.Initialize();

            contracts.DocumentsByName = (await documents.GetAllContracts()).ToDictionary(contract => contract.Name);



            var cardsService = Container.CardsContractMethods = new Contracts.Contracts.Cards.CardsService(web3, contracts.DocumentsByName[nameof(contracts.Cards)].Address);

            Web3Db web3Db = Container.Web3Db = new Web3Db(
                web3,
                contracts,
                cardsService,
                Configuration["web3ServerAddress"],
                Configuration["web3ServerPrivateKey"]);


            var cardEventsController = Container.CardEventsController = new CardEventsController(Container);

            var newBlockManager = new NewBlockManager(Container, cardEventsController);



            var checkpointFactory = new BlockCheckpointFactory(web3, documents);

            // start checkpoint from contract blockid
            var cardContractCreatedBlock = Container.ContractArtifacts.DocumentsByName[Container.ContractArtifacts.Cards.Name].TransactionReceipt.BlockNumber;
            var checkpoint = await checkpointFactory.GetOrCreateCheckpoint("blockManagerCheckpoint", new BlockParameter(cardContractCreatedBlock));


            var blockLoop = new NewBlockLoop(web3, checkpoint);


            services.AddSingleton(Container);
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
