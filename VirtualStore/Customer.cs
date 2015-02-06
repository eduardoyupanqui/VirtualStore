using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
        }

        public DateTime BirdDate { get; set; }

        public string Email { get; set; }

        public Address Residence { get; set; }

        public Address Delivery { get; set; }

        public virtual List<ShoppingCart> ShoppingCarts { get; set; }

        public Customer()
        {
            this.Residence = new Address();
            this.Delivery = new Address();
        }
    }
}
