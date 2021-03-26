using System;
using System.Threading.Tasks;
using WeCollect.App.Bll;
using WeCollect.App.Extensions;
using WeCollect.App.Models;

namespace WeCollect.Server.Models
{
    public static class Seed
    {
        public static async Task DoSeed(CardFactory cardFactory)
        {
            await cardFactory.MintCard(new CardMintingDto()
            {
                MiningLevel = 1,
                PriceWei = Wei.FromEth(0.2m),
                LastMiningCollectedDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Name = "Universe",
                ImageUploadUri = new Uri(@"http://24.69.146.99/img/cards/universe.png")
            },
            new CardFactory.CardOptions
            {
            });
        }
    }
}
