using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualStore.Helpers
{
    public static class DbHelper
    {
        public static void CreateDb()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                if (context.Database.Exists())
                    context.Database.Delete();

                context.Database.Create();
            }
        }
    }
}
