using StudentsKingdom.Data.Models;
using StudentsKingdom.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Areas.Game.Models.Dorm
{
    public class CharacterViewModel: IMapFrom<Character>
    {
        public int Coins { get; set; }

        public Stats Stats { get; set; }
       
        public Inventory Inventory { get; set; }
    }
}
