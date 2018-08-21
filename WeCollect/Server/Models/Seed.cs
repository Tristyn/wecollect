using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using WeCollect.App;
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
                LastMiningCollectedDate = DateTimeOffset.UtcNow,
                Name = "Universe",
            },
            new CardFactory.CardOptions
            {
            });
        }
    }
}
