using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class Book : Product
    {
        public string Author { get; set; }

        public int ISBN { get; set; }

        public int PublicationYear { get; set; }

        

        public List<Genre> Genre { get; set; }
    }
}
