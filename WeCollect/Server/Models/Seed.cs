using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using WeCollect.App;
using WeCollect.App.Bll;

namespace WeCollect.Server.Models
{
    public static class Seed
    {
        public static async Task DoSeed(CardFactory cardFactory)
        {
            await cardFactory.CreateCard(new CardDto
            {
                MiningLevel = 1,
                PriceEth = 20,
                LastMiningCollectedDate = DateTimeOffset.UtcNow
            }, new CardFactory.CardOptions
            {
                
            });
        }
    }
}
