using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class ShoppingItem
    {
        public int ShoppingItemID { get; set; }

        public int Quantity { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
        
        public int Secuential { get; set; }



        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        
        public virtual ShoppingItemDetail Detail { get; set; }
    }
}
