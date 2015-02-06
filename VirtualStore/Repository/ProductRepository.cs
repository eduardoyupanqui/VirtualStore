using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Especific;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {

        public Product GetWithCategory(int productID)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return context.Set<Product>().Include("Category").FirstOrDefault(x => x.ProductID == productID);
            }

        }

    }

}
