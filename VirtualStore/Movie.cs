using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class Movie : Product
    {
        public TimeSpan Duration { get; set; }

        public string LanguageSound { get; set; }

        public List<Genre> Genre { get; set; }
    }
}
