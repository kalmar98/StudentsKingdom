using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Quest : BaseModel<int>, IName, ICoins
    {
        public string Name { get; set; }

        public int Coins { get; set; }

        public int EnemyId { get; set; }
        public Enemy Enemy { get; set; }
    }
}
