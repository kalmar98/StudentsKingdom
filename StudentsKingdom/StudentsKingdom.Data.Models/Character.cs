using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;

namespace StudentsKingdom.Data.Models
{
    public class Character : BaseModel<int>, IStats
    {


        public int Health { get; private set; }

        public int Damage { get; private set; }

        public int Defence { get; private set; }

        public int Vitality { get; private set; }

        public int Strength { get; private set; }

        public int Agility { get; private set; }

        public int Intellect { get; private set; }

        public int Endurance { get; private set; }
    }
}
