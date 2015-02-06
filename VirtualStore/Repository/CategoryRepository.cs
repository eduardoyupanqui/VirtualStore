using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Especific;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        //El método GetById() podría haberse omitido ya que el mismo dato podría haberse recuperado 
        //mediante la el método Single() que define el repositorio base, utilizando
        //Category category = new CategoryRepository().Simple(x => x.CategoryID == categoryId);
        public Category GetById(int categoryID)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return context.Set<Category>().FirstOrDefault(x => x.CategoryId == categoryID);
            }

        }

    }
}
