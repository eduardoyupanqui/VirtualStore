using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Collections.Generic;
using VirtualStore.Helpers;
using VirtualStore.Repository;

namespace VirtualStore.Test
{
    [TestClass]
    public class SupplierTest
    {

        [TestInitialize]
        public void inicializar()
        {
            //DbHelper.CreateDb();
        }

        [TestMethod]
        public void Delete_Supplier()
        {
            ProductRepository repoProduct = new ProductRepository();
            SupplierRepository repoSupplier = new SupplierRepository();

            //se crea el proveedor
            Supplier supplierNew = new Supplier()
            {
                 CompanyName = "supplier 1"
            };
            repoSupplier.Create(supplierNew);

            //se crea el producto relacionandolo con el proveedor
            Product productNew1 = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                Category = new Category()
                {
                    CategoryName = "category1",
                    Description = "desc category 1"
                },
                SupplierID = supplierNew.SupplierId
            };


            repoProduct.Create(productNew1);

            //se crea otro producto relacionandolo con el proveedor
            Product productNew2 = new Movie()
            {
                Title = "El Hombre Araña 3",
                Description = @"El Hombre Araña 3 cuenta la historia de ... bla bla bla",
                Price = 200,
                Duration = new TimeSpan(2, 0, 0),
                LanguageSound = "Español/Ingles",
                Stock = 3,
                Category = new Category()
                {
                    CategoryName = "category2",
                    Description = "desc category 2"
                },
                SupplierID = supplierNew.SupplierId
            };
            repoProduct.Create(productNew2);


            //se recupera el proveedor para obtener los productos asociados
            var supplierSel = repoSupplier.Single(x => x.SupplierId == supplierNew.SupplierId,
                                                new List<Expression<Func<Supplier, object>>>() { x => x.Products });

            //se elimina la categoria y se desasocian los productos 
            repoSupplier.Delete(supplierSel);


            //se recupera la categoria para validar si se elimino
            var categorySelected = repoSupplier.Single(x => x.SupplierId == supplierNew.SupplierId);

            Assert.IsNull(categorySelected);

            //se recupera el primer producto
            Product productSel1 = repoProduct.Single(x => x.ProductID == productNew1.ProductID);
            Assert.IsNotNull(productSel1);

            //se recupera el primer producto
            Product productSel2 = repoProduct.Single(x => x.ProductID == productNew2.ProductID);
            Assert.IsNotNull(productSel2);
        }

        [TestMethod]
        public void GetAllSupplier()
        {
            SupplierRepository repository = new SupplierRepository();

            Supplier supplier = new Supplier()
            {
                CompanyName = "Compañia 1",
                Contact = new Contact()
                {
                    ContactName = "contacto 1",
                    ContactTitle = "Sr",
                    Fax = "111-111111",
                    Phone = "237616"
                }
            };
            repository.Create(supplier);

            var supplierList = repository.GetAll();

            Assert.AreEqual(supplierList.Count, 1);
            Assert.IsTrue(supplierList[0].Contact != null);



        }


        //Asociación opcional – Eliminar
        [TestMethod]
        public void Delete_Supplier2()
        {
            ProductRepository repoProduct = new ProductRepository();
            SupplierRepository repoSupplier = new SupplierRepository();

            //se crea el proveedor
            Supplier supplierNew = new Supplier()
            {
                CompanyName = "supplier 1"
            };
            repoSupplier.Create(supplierNew);

            //se crea el producto relacionandolo con el proveedor
            Product productNew1 = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                Category = new Category()
                {
                    CategoryName = "category1",
                    Description = "desc category 1"
                },
                SupplierID = supplierNew.SupplierId
            };
            repoProduct.Create(productNew1);

            //se crea otro producto relacionandolo con el proveedor
            Product productNew2 = new Movie()
            {
                Title = "El Hombre Araña 3",
                Description = @"El Hombre Araña 3 cuenta la historia de ... bla bla bla",
                Price = 200,
                Duration = new TimeSpan(2, 0, 0),
                LanguageSound = "Español/Ingles",
                Stock = 3,
                Category = new Category()
                {
                    CategoryName = "category2",
                    Description = "desc category 2"
                },
                SupplierID = supplierNew.SupplierId
            };
            repoProduct.Create(productNew2);


            //se recupera el proveedor para obtener los productos asociados
            var supplierSel = repoSupplier.Single(x => x.SupplierId == supplierNew.SupplierId,
            new List<Expression<Func<Supplier, object>>>() { x => x.Products });

            //se elimina la categoria y se desasocian los productos
            repoSupplier.Delete(supplierSel);


            //se recupera la categoria para validar si se elimino
            var categorySelected = repoSupplier.Single(x => x.SupplierId == supplierNew.SupplierId);

            Assert.IsNull(categorySelected);

            //se recupera el primer producto
            Product productSel1 = repoProduct.Single(x => x.ProductID == productNew1.ProductID);
            Assert.IsNotNull(productSel1);

            //se recupera el primer producto
            Product productSel2 = repoProduct.Single(x => x.ProductID == productNew2.ProductID);
            Assert.IsNotNull(productSel2);
        }

    }
}
