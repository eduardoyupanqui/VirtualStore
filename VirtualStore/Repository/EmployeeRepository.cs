using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Generic;
using VirtualStore.Repository.Especific;

namespace VirtualStore.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        /// <summary>
        /// Recupera solo la entidad Extendida
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeExtended GetExtendedById(int id)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return context.EmployeeExtendeds.FirstOrDefault(x => x.EmployeeId == id);
            }
        }

        /// <summary>
        /// Elimina la entidad incluyendo la informacion extendida
        /// en caso de tenerla
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteIncludeExtended(Employee entity)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                if (entity.EmployeeExt == null)
                    entity.EmployeeExt = new EmployeeExtended() { EmployeeId = entity.EmployeeId };

                context.Employees.Attach(entity);
                context.Employees.Remove(entity);

                context.SaveChanges();

            }
        }

    }
}
