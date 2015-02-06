using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }

        public Address Localization { get; set; }
        public Contact Contact { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public Supplier()
        {
            this.Localization = new Address();
            this.Contact = new Contact();
        }
    }
}
