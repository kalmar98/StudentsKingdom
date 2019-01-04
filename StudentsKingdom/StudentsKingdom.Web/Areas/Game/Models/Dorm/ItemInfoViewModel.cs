using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Areas.Game.Models.Dorm
{
    public class ItemInfoViewModel
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public StatsViewModel Stats { get; set; }
    }
}
