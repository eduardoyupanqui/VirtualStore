using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualStore
{
    public class EmployeeExternal : Employee2
    {
        public string ConsultantName { get; set; }

        public DateTime? ContactExpiration { get; set; }
    }
}
