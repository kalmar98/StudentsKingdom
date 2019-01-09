using Newtonsoft.Json;
using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Inventory : BaseModel<int>, ICapacity
    {
        public int Capacity { get; set; }

        [JsonIgnore]
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

    }
}
