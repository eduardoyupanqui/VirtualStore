using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public EmployeeExtended EmployeeExt { get; set; }

        public Address Localization { get; set; }

        public Employee()
        {
            this.Localization = new Address();
            this.EmployeeExt = new EmployeeExtended();
        }

        public virtual ICollection<Territory> Territories { get; set; }
    }
}
