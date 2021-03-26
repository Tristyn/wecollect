using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.Server.Hubs;
using WeCollect.Server.Models;

namespace WeCollect.Server.Controllers
{
    [Route("~/")]
    public class HomeController : Controller
    {
        private readonly Container _container;

        public HomeController(Container container, IHubContext<CardHub, ICardHubClient> cardHub)
        {
            _container = container;
            GlobalHubContext.CardHub = cardHub;
        }

        public const string CardRoute = "Card";
        public const string UniverseRoute = "Universe";

        [Route("~/", Name = UniverseRoute)]
        public async Task<IActionResult> Index()
        {
            return await Card("universe");
        }

        [Route("collection/{name}")]
        public async Task<IActionResult> Set([FromRoute]string name)
        {
            var set = new SetViewModel
            {
                Cards = await _container.Documents.GetCardSet(name)
            };
            return View("Set", set);
        }

        [Route("collectible/{name}", Name = CardRoute)]
        public async Task<IActionResult> Card([FromRoute]string name)
        {
            var card = await _container.Documents.GetCardWithUriName(name);
            return View("Card", card);
        }
        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
