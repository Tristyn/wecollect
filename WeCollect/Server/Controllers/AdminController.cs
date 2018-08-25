using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Nethereum.Util;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Extensions;
using WeCollect.App.Models;
using WeCollect.Server.Hubs;
using WeCollect.Server.Models;

namespace WeCollect.Server.Controllers
{
    public class AdminController : Controller
    {
        public Container Container { get; }

        public AdminController(Container container, IHubContext<CardHub, ICardHubClient> cardHub)
        {
            Container = container;
            GlobalHubContext.CardHub = cardHub;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["AllCards"] = (await Container.Documents.GetAllCards())
                .Select(card => new SelectListItem { Text = card.name, Value = card.cardsContractId.ToString() });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Redeploy()
        {
            await Container.ContractsInitializer.Redeploy();

            return View(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MintCard([FromForm] CardMintingDto mintingCard)
        {
            // The form accepts price in eth, convert to wei
            mintingCard.PriceWei = UnitConversion.Convert.ToWei(mintingCard.PriceWeiDecimal, UnitConversion.EthUnit.Ether);
            await Container.CardFactory.MintCard(mintingCard, new App.Bll.CardFactory.CardOptions { });

            var card = await Container.Documents.GetCardWithName(mintingCard.Name);
            return Redirect(Url.Action("Card", "Home", new { name = card.uriName }));
        }
    }
}
