using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;

namespace StudentsKingdom.Data.Models
{
    public class Character : BaseModel<int>, ICoins
    {
        public int Coins { get; set; }

        public int StatsId { get; set; }
        public Stats Stats { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }



    }
}
