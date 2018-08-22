using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.App.Models
{
    public class EntityIdDto : Document
    {
        public int currentId { get; set; }

        public override string name { get; set; }

        public override string type => nameof(EntityIdDto);

        public static string GetId(string name)
        {
            return nameof(EntityIdDto) + name;
        }
    }
}
