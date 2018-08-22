using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.Server.Models
{
    public class CardEventLog<T>
    {
        public CardEventLog()
        {

        }

        public CardEventLog(CardDto card, EventLog<T> @event)
        {
            Card = card;
            Event = @event.Event;
            Log = @event.Log;
        }

        public CardEventLog(CardDto card, T @event, FilterLog log)
        {
            Card = card;
            Event = @event;
            Log = log;
        }

        public CardDto Card { get; set; }

        public T Event { get; set; }

        public FilterLog Log { get; set; }
    }
}
