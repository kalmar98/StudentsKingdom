using StudentsKingdom.Data.Models;
using StudentsKingdom.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Areas.Game.Models.Dorm
{
    public class StatsViewModel : IMapFrom<Stats>
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
