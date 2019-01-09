using Newtonsoft.Json;
using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Stats : BaseModel<int>, IStats
    {
        public int Health { get; set; }

        public int Damage { get; set; }

        public int Defence { get; set; }

        public int Vitality { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Intellect { get; set; }

    }
}
