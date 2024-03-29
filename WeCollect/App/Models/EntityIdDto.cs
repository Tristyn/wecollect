﻿namespace WeCollect.App.Models
{
    public class IdCounterDto : Document
    {
        public int currentId { get; set; }

        public override string name { get; set; }

        public override string type => nameof(IdCounterDto);

        public static string GetId(string name)
        {
            return nameof(IdCounterDto) + name;
        }
    }
}
