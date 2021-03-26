using System.Collections.Generic;
using System.Linq;

namespace WeCollect.Server.Models
{
    public class SetViewModel
    {
        public IEnumerable<CardDto> Cards { get; set; } = Enumerable.Empty<CardDto>();
    }
}
