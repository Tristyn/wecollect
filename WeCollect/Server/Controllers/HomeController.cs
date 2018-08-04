using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Models;
using WeCollect.Server.Models;

namespace WeCollect.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly Container _container;

        public HomeController(Container container)
        {
            _container = container;
        }

        public const string CardRoute = "Card";
        public const string UniverseRoute = "Universe";

        [Route("Index", Name = UniverseRoute)]
        public Task<IActionResult> Index()
        {
            return Set("universe");
        }

        [Route("set/{name}")]
        public async Task<IActionResult> Set([FromRoute]string name)
        {
            IEnumerable<CardDto> cards = await _container.Documents.GetCardSet(name);



            return View();
        }

        [Route("card/{name}", Name = CardRoute)]
        public async Task<IActionResult> Card([FromRoute]string name)
        {
            CardDto card = await _container.Documents.Cards.Get(CardDto.GetId(name));
            return View(card);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("card/{id}")]
        public IActionResult Card([FromRoute]int id)
        {
            return View();
        }
    }
}
