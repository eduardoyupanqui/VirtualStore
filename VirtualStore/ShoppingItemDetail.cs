using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class ShoppingItemDetail
    {
        public int ShoppingItemDetailId { get; set; }

        public string SalesMan { get; set; }

        public string Observations{ get; set; }


        public ShoppingItem Item { get; set; }
    }
}
