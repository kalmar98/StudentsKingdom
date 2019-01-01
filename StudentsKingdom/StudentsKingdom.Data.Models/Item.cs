using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using StudentsKingdom.Data.Common.Enums.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Item : BaseModel<int>, IName, ICoins, IImage
    {
        public string Name { get; set; }

        public ItemType Type { get; set; }

        public int Coins { get; set; }

        public string Image { get; set; }

        public int StatsId { get; set; }
        public virtual Stats Stats { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
