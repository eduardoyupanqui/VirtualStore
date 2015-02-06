using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Especific;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository
{
    public class Employee2Repository : BaseRepository<Employee2>, IEmployee2Repository
    {
        /// <summary>
        /// Retorna todos los empleados internos a la empresa
        /// </summary>
        /// <returns></returns>
        public List<EmployeeInternal> GetAllInternalType()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return context.Employee2s.OfType<EmployeeInternal>().ToList();
            }
        }
        /// <summary>
        /// Retorna todos los empleados externos a la empresa
        /// con linq
        /// </summary>
        /// <returns></returns>
        public List<Employee2> GetAllExternalType()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var result = from employee in context.Employee2s
                             where employee is EmployeeExternal
                             select employee;

                return result.ToList();
            }
        }

        //obtener el ultimo id para Herencia - Tabla por tipo concreto - Table per Concrete Type 
        public int GetLastId()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                int? lastId = context.Employees.Max(x => (int?)x.EmployeeId);

                return lastId.HasValue ? lastId.Value : 0;
            }
        }

    }

    public class EmployeeInternalRepository : BaseRepository<EmployeeInternal>
    {

    }

    public class EmployeeExternalRepository : BaseRepository<EmployeeExternal>
    {

    }
}
