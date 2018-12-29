using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Enemy : BaseModel<int>, IName
    {
        public string Name { get; set; }

        public int StatsId { get; set; }
        public Stats Stats { get; set; }


    }
}
