using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using System.Collections.Generic;

namespace StudentsKingdom.Data.Models
{
    public class Character : BaseModel<int>, ICoins
    {
        public int Coins { get; set; }

        public int StatsId { get; set; }
        public virtual Stats Stats { get; set; }

        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

    }
}
