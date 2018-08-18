using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.Server.Models;

namespace WeCollect.App.Bll
{
    public class CardFactory
    {
        public async Task CreateCard(CardDto card, CardOptions options)
        {
            
        }

        public class CardOptions
        {
            public bool SkipContract { get; set; }

            public bool SkipDocument { get; set; }
        }
    }
}
