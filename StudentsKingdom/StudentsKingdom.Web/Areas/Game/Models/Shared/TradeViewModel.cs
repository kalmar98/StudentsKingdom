using StudentsKingdom.Data.Models;
using StudentsKingdom.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Areas.Game.Models.Shared
{
    public class TradeViewModel : IMapFrom<Location>
    {
        public string Name { get; set; }

        public Inventory Inventory { get; set; }
    }
}
