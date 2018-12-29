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

        public IList<Item> Items { get; set; } = new List<Item>();

    }
}
