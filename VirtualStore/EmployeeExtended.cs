using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualStore
{
    public class EmployeeExtended
    {
        public int EmployeeId { get; set; }

        public string Notes { get; set; }

        public byte[] Photo { get; set; }

        public string PhotoPath { get; set; }
    }
}
