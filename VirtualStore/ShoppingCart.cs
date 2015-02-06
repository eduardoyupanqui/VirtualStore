using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }

        public decimal Discount { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        //DECLARANDO CLAVE FORANEA
        //<Nombre de propiedad de navegacion><nombre de propiedad de clave principal de tipo principal>
        //public int ClientCode { get; set; }

        //<Nombre de clase de tipo principal><nombre de propiedad de clave principal>
        public int CustomerId { get; set; }
        //public int? CustomerId { get; set; }

        //<Nombre de propiedad de clave principal de tipo principal>
        //public int Code { get; set; }

        //<Propiedad de navegacion>
        public virtual Customer Customer { get; set; }

        public virtual List<ShoppingItem> Items { get; set; }
    }
}
