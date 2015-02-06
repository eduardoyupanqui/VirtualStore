using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Repository.Especific
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetWithCategory(int productID);
    }
}
