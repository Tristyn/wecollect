using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App.Models;

namespace WeCollect.Server.Models
{
    public class SetViewModel
    {
        public IEnumerable<CardDto> Cards { get; set; } = Enumerable.Empty<CardDto>();
    }
}
