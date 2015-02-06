using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualStore
{
    public abstract class Product
    {
        public string Description { get; set; }

        public double Price { get; set; }

        public int ProductID { get; set; }

        public int Stock { get; set; }

        public string Title { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public int? SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
}
