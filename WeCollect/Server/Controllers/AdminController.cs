using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App;

namespace WeCollect.Server.Controllers
{
    public class AdminController : Controller
    {
        public Container Container { get; }

        public AdminController(Container container)
        {
            Container = container;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Redeploy()
        {
            await Container.ContractsInitializer.Redeploy();

            return View(nameof(Index));
        }
    }
}
