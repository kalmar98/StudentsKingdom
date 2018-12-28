using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Stats : BaseModel<int>, IStats
    {
        public int Health { get; private set; }

        public int Damage { get; private set; }

        public int Defence { get; private set; }

        public int Vitality { get; private set; }

        public int Strength { get; private set; }

        public int Agility { get; private set; }

        public int Intellect { get; private set; }

        public int Endurance { get; private set; }

        public int Speed { get; private set; }

        public int Weight { get; private set; }
    }
}
