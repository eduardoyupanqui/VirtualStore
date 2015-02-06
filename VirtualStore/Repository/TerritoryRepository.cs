using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository
{
    public class TerritoryRepository : BaseRepository<Territory>
    {

        public void AddEmployees(Territory territory, List<Employee> employes)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                //marcamos el territorio para que no reciba cambios
                context.Entry(territory).State = EntityState.Unchanged;

                if (territory.Employees == null)
                    territory.Employees = new List<Employee>();

                //recorremos cada empleado que se quiera asociar
                employes.ForEach(x =>
                {
                    //el empleado tampoco debe recibir cambios
                    context.Entry(x).State = EntityState.Unchanged;
                    //asociamos a la colecion de empleados del territorio el nuevo item
                    //este si recibira cambios
                    territory.Employees.Add(x);
                });

                context.SaveChanges();
            }
        }

        public void RemoveEmployees(Territory territory, List<Employee> employees)
        {
            //validamos que haya algo que remover
            if (employees == null || employees.Count == 0)
                return;

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                //recuperamos el terrotorio y sus empleados
                //esto es necesario porque el objeto donde se debe remover tiene que estar dentro del contexto de EF
                Territory territorySel = context.Set<Territory>().Include("Employees").FirstOrDefault(x => x.TerritoryId == territory.TerritoryId);

                if (territory.Employees == null || territory.Employees.Count == 0)
                    return;

                employees.ForEach(x =>
                {
                    //localizamos al empleado dentro de la coleccion que se recupero anteriormente
                    Employee employeeRemove = territorySel.Employees.First(e => e.EmployeeId == x.EmployeeId);
                    //se remueve de la coleccion haciendo uso de la instancia
                    territorySel.Employees.Remove(employeeRemove);
                });

                context.SaveChanges();
            }
        }

    }
}