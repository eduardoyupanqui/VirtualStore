using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class Territory
    {
        public int TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
