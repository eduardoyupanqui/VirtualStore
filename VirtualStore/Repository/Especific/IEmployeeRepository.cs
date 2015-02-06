using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository.Especific
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        EmployeeExtended GetExtendedById(int id);

        void DeleteIncludeExtended(Employee entity);
    }
}
