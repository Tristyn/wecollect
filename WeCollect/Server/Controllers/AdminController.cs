using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Models;

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

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromForm] CardDto card)
        {
            await Container.Documents.Cards.Create(card);

            return Redirect(Url.Action("Card", "Home", new { name = card.Name }));
        }
    }
}
