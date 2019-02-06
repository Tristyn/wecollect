using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeCollect.App;

namespace WeCollect.Server.Middleware
{
    public class JsDataInjectorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _services;
        private readonly object _lock = new object();
        private string _cardContractJson;
        private bool _init = true;

        public JsDataInjectorMiddleware(RequestDelegate next, IServiceProvider services)
        {
            _next = next;
            _services = services;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_init)
            {
                await Startup.StartupComplete;
                var container = (Container)_services.GetService(typeof(Container));
                var cardContract = container.ContractArtifacts.DocumentsByName[nameof(container.ContractArtifacts.Cards)];
                _cardContractJson = JsonConvert.SerializeObject(new { address = cardContract.Address, abi = JsonConvert.DeserializeObject(cardContract.Abi) });
                _init = false;
            }
            var cardContractJson = _cardContractJson;

            context.Items.Add("cardContract", cardContractJson);

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }

    public static class JsDataInjectorMiddlewareExtensions
    {
        public static IApplicationBuilder UseJsDataInjector(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsDataInjectorMiddleware>(builder.ApplicationServices);
        }
    }
}
