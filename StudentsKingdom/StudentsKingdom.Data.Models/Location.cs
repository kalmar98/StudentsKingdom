using StudentsKingdom.Data.Common;
using StudentsKingdom.Data.Common.Contracts;
using StudentsKingdom.Data.Common.Enums.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Models
{
    public class Location : BaseModel<int>, IName
    {
        public string Name { get; set; }

        public int? InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public LocationType Type { get; set; }
    }
}
