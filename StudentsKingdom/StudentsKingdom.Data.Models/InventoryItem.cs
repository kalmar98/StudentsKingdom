using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class InventoryItem
    {
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public bool IsEquipped { get; set; }
    }
}
