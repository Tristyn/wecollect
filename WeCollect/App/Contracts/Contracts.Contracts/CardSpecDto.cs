using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.App.Models
{
    public class CardSpecDto
    {
        public int cardContractId { get; set; }

        public string name { get; set; }
        
        public string uriName { get; set; }
    }
}
