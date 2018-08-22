﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Models;
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
        public Task<IActionResult> Index()
        {
            return Card("universe");
        }

        [Route("collection/{name}")]
        public async Task<IActionResult> Set([FromRoute]string name)
        {
            IEnumerable<CardDto> cards = await _container.Documents.GetCardSet(name);



            return View();
        }

        [Route("collectible/{name}", Name = CardRoute)]
        public async Task<IActionResult> Card([FromRoute]string name)
        {
            CardDto card = await _container.Documents.GetCardWithUriName(name);
            return View(card);
        }
        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("collectible/{id}")]
        public IActionResult Card([FromRoute]int id)
        {
            return View();
        }
    }
}
