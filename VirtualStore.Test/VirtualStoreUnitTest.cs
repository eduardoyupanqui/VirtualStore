using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VirtualStore.Test
{
    [TestClass]
    public class VirtualStoreUnitTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Init()
        {
            var appDataDir = this.TestContext.DeploymentDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", appDataDir);
        }


        [TestMethod]
        public void ObtenerCustomers()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var customers = context.Customers.ToList();

                Assert.AreEqual(customers.Count, 0);
            }

        }
    }
}
