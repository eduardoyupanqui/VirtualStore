using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository.Especific
{
    public interface IEmployee2Repository : IRepository<Employee2>
    {
        List<EmployeeInternal> GetAllInternalType();
        List<Employee2> GetAllExternalType();
        int GetLastId();
    }
}