using StudentsKingdom.Data.Models;
using StudentsKingdom.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Areas.Game.Models.University
{
    public class QuestInfoViewModel : IMapFrom<Quest>
    {
        public string Name { get; set; }

        public int Coins { get; set; }

        public string EnemyName { get; set; }
    }
}
