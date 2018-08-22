using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WeCollect.Server.Hubs
{
    public class GlobalHubContext
    {
        private static IHubContext<CardHub, ICardHubClient> _hub;
        private static ManualResetEventSlim _hubEvent = new ManualResetEventSlim();

        public static IHubContext<CardHub, ICardHubClient> CardHub
        {
            get
            {
                _hubEvent.Wait();
                return _hub;
            }
            set
            {
                _hub = value;
                _hubEvent.Set();
            }
        }
    }
}
