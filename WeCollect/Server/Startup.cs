using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Documents;
using WeCollect.App.Web3;

namespace WeCollect
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        public static Container Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RazorViewEngineOptions>(o =>
            {
                // {2} is area, {1} is controller,{0} is the action    
                o.ViewLocationFormats.Add("/Server/Views/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Server/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Server/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            Container = new Container();

            ContractArtifacts contracts = new ContractArtifacts();

            Web3Db web3Db = Container.Web3 = new Web3Db(
                new Nethereum.Web3.Web3(Configuration["web3Url"]),
                contracts,
                Configuration["web3ServerAddress"],
                Configuration["web3ServerPrivateKey"]);
            
            DocumentDb documents = Container.Documents = new DocumentDb(
                new Microsoft.Azure.Documents.Client.DocumentClient(
                    Configuration.GetValue<Uri>("documentDbEndpoint"),
                    Configuration["documentDbKey"],
                    null,
                    Microsoft.Azure.Documents.ConsistencyLevel.Session));

            //var publisher = new ContractPublisher(web3Db);

            var contractSpecs = Container.ContractArtifacts = new ContractArtifacts();

            var contractsInitializer = Container.ContractsInitializer = new ContractsInitializer(Container);

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
        }

        public static async Task Initialize()
        {
            await Container.ContractsInitializer.Initialize();
        }
    }
}
