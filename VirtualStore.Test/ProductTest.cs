using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Collections;
using VirtualStore.Repository;
using System.Linq.Expressions;

namespace VirtualStore.Test
{
    [TestClass]
    public class ProductTest
    {
        public Book book1 = null;
        public Book book2 = null;
        public Book book3 = null;
        public Movie movie1 = null;
        public Movie movie2 = null;

        //[TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<VirtualStoreContext>());

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Products.RemoveRange(context.Products);

                book1 = new Book()
                {
                    Title = "El Silmarillon",
                    Description = @"El Silmarillon cuenta la historia de ... bla bla bla",
                    Price = 139,
                    ISBN = 1452356565,
                    PublicationYear = 2002,
                    Stock = 5,
                    Author = "J. R. R. Tolkien"
                };
                context.Products.Add(book1);

                book2 = new Book()
                {
                    Title = "El Nombre del viento",
                    Description = @"El Nombre del viento cuenta la historia de ... bla bla bla",
                    Price = 319,
                    ISBN = 1452121512,
                    PublicationYear = 2009,
                    Stock = 10,
                    Author = "Patrick"
                };
                context.Products.Add(book2);

                book3 = new Book()
                {
                    Title = "Un Mago de Terramar",
                    Description = @"Un Mago de Terramar cuenta la historia de ... bla bla bla",
                    Price = 217,
                    ISBN = 1452342344,
                    PublicationYear = 1968,
                    Stock = 3,
                    Author = "Ursula"
                };
                context.Products.Add(book3);

                movie1 = new Movie()
                {
                    Title = "El Hombre Araña 3",
                    Description = @"El Hombre Araña 3 cuenta la historia de ... bla bla bla",
                    Price = 200,
                    Duration = new TimeSpan(2,0,0),
                    LanguageSound = "Español/Ingles",
                    Stock = 3
                };
                context.Products.Add(movie1);

                movie2 = new Movie()
                {
                    Title = "Capitan America",
                    Description = @"Capitan America cuenta la historia de ... bla bla bla",
                    Price = 100,
                    Duration = new TimeSpan(2,30,0),
                    LanguageSound = "Español/Ingles",
                    Stock = 5
                };
                context.Products.Add(movie2);

                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetAllProducts()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var products = context.Products.ToList();

                Assert.IsNotNull(products);
                Assert.AreEqual(products.Count, 5);


                Assert.AreEqual(products.OfType<Book>().Count(), 3);
                Assert.AreEqual(products.OfType<Movie>().Count(), 2);

            }
        }

        [TestMethod]
        public void GetProducts_ByType()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var products1 = context.Products.OfType<Book>().ToList();
                var products2 = (from prod in context.Products
                                    where prod is Movie
                                 select prod).ToList();

                Assert.AreEqual(products1.Count, 3);
                Assert.AreEqual(products2.Count, 2);
            }
        }

        [TestMethod]
        public void GetProducts_FilterTwoTypes()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var products = (from prod in context.Products
                                let book = prod as Book
                                let movie = prod as Movie
                                where (((book != null) && (book.Author.Contains("Tolkien")))
                                    || ((movie != null) && (movie.Price > 120)))
                                select prod).ToList();

                Assert.AreEqual(products.Count,2);

                Assert.IsInstanceOfType(products[0],typeof(Book));
                Assert.AreEqual(((Book)products[0]).Author, book1.Author);
                Assert.AreEqual(((Book)products[0]).Title, book1.Title);


                Assert.IsInstanceOfType(products[1], typeof(Movie));
                Assert.AreEqual(((Movie)products[1]).Title, movie1.Title);
                Assert.AreEqual(((Movie)products[1]).Price, movie1.Price);
            }
        }



        [TestMethod]
        public void GetSingle_IncludeCategory_Product()
        {
            ProductRepository repoProduct = new ProductRepository();
            CategoryRepository repoCategory = new CategoryRepository();
            SupplierRepository repoSupplier = new SupplierRepository();

            //se crea la categoria
            Category categoryNew = new Category()
            {
                CategoryName = "category1",
                Description = "desc category 1"
            };
            repoCategory.Create(categoryNew);

            //se crea un proveedor
            Supplier supplierNew = new Supplier()
            {
                CompanyName = "Company 1",
            };
            repoSupplier.Create(supplierNew);

            //se crea el producto relacionado con la categoria
            Product productNew = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                CategoryID = categoryNew.CategoryId,
                SupplierID = supplierNew.SupplierId
            };
            repoProduct.Create(productNew);

            //se recupea el producto con la categoria asociada
            var productSelected = repoProduct.Single(x => x.ProductID == productNew.ProductID,
                                                    new List<Expression<Func<Product, object>>>() { x => x.Category });

            Assert.IsNotNull(productSelected.Category);
            Assert.AreEqual(productSelected.Category.CategoryId, categoryNew.CategoryId);
            Assert.AreEqual(productSelected.CategoryID, categoryNew.CategoryId);

            Assert.AreEqual(productSelected.SupplierID, supplierNew.SupplierId);
            Assert.IsNull(productSelected.Supplier);
        }

        [TestMethod]
        public void Get_WithCategory_Product()
        {

            ProductRepository repoProduct = new ProductRepository();
            CategoryRepository repoCategory = new CategoryRepository();

            //se crea la categoria
            Category category = new Category()
            {
                CategoryName = "category1",
                Description = "desc category 1"
            };
            repoCategory.Create(category);

            //se crea el producto relacionado con la categoria
            Product prod = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                CategoryID = category.CategoryId
            };
            repoProduct.Create(prod);

            var productSelected = repoProduct.GetWithCategory(prod.ProductID);

            Assert.IsNotNull(productSelected.Category);
            Assert.AreEqual(productSelected.Category.CategoryId, category.CategoryId);
            Assert.IsNull(productSelected.Supplier);
        }

        [TestMethod]
        public void GetSingle_WithCategory_Product()
        {
            ProductRepository repoProduct = new ProductRepository();
            CategoryRepository repoCategory = new CategoryRepository();

            //se crea la categoria
            Category categoryNew = new Category()
            {
                CategoryName = "category1",
                Description = "desc category 1"
            };
            repoCategory.Create(categoryNew);

            //se crea el producto relacionado con la categoria
            Product prod = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                CategoryID = categoryNew.CategoryId
            };
            repoProduct.Create(prod);

            //se recupea el producto con la categoria y proveedor asociado
            var productSelected = repoProduct.Single(x => x.ProductID == prod.ProductID,
                                                    new List<Expression<Func<Product, object>>>() { x => x.Category });

            Assert.AreEqual(productSelected.Category.CategoryId, categoryNew.CategoryId);
            Assert.IsNotNull(productSelected.Category);
            Assert.IsNull(productSelected.Supplier);
        }

        [TestMethod]
        public void GetSingle_WithCategoryAndSupplier_Product()
        {
            ProductRepository repoProduct = new ProductRepository();
            CategoryRepository repoCategory = new CategoryRepository();
            SupplierRepository repoSupplier = new SupplierRepository();

            //se crea la categoria
            Category categoryNew = new Category()
            {
                CategoryName = "category1",
                Description = "desc category 1"
            };
            repoCategory.Create(categoryNew);

            //se crea un proveedor
            Supplier supplierNew = new Supplier()
            {
                CompanyName = "Company 1",
            };
            repoSupplier.Create(supplierNew);

            //se crea el producto relacionado con la categoria
            Product prod = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                CategoryID = categoryNew.CategoryId,
                SupplierID = supplierNew.SupplierId
            };
            repoProduct.Create(prod);

            //se recupea el producto con la categoria y proveedor asociado
            var productSelected = repoProduct.Single(x => x.ProductID == prod.ProductID,
                                                    new List<Expression<Func<Product, object>>>() { x => x.Category, x => x.Supplier });

            Assert.IsNotNull(productSelected.Category);
            Assert.AreEqual(productSelected.Category.CategoryId, categoryNew.CategoryId);

            Assert.IsNotNull(productSelected.Supplier);
            Assert.AreEqual(productSelected.Supplier.SupplierId, supplierNew.SupplierId);
        }

        [TestMethod]
        public void Create_WithCategoryAndSupplier_Product()
        {
            ProductRepository repoProduct = new ProductRepository();

            //se crea el producto relacionado con la categoria
            Product productNew = new Book()
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
                Supplier = new Supplier()
                {
                    CompanyName = "Company 1",
                }
            };
            repoProduct.Create(productNew);

            //se recupea el producto con la categoria y proveedor asociado
            var productSelected = repoProduct.Single(x => x.ProductID == productNew.ProductID,
                                                    new List<Expression<Func<Product, object>>>() { x => x.Category, x => x.Supplier });

            Assert.IsNotNull(productSelected.Category);
            Assert.AreEqual(productSelected.Category.CategoryId, productNew.CategoryID);

            Assert.IsNotNull(productSelected.Supplier);
            Assert.AreEqual(productSelected.Supplier.SupplierId, productNew.SupplierID);
        }

        [TestMethod]
        public void Create_WithInstanceCategoryAndInstanceSupplier_Product()
        {
            ProductRepository repoProduct = new ProductRepository();
            CategoryRepository repoCategory = new CategoryRepository();
            SupplierRepository repoSupplier = new SupplierRepository();


            //se crea la categoria
            Category categoryNew = new Category()
            {
                CategoryName = "category1",
                Description = "desc category 1"
            };
            repoCategory.Create(categoryNew);

            //se crea un proveedor
            Supplier supplierNew = new Supplier()
            {
                CompanyName = "Company 1",
            };
            repoSupplier.Create(supplierNew);

            //se crea el producto relacionado con la categoria
            Product productNew = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                Category = categoryNew,//ESTE CREARA OTRAS
                Supplier = supplierNew//ESTE CREARA OTRAS
            };
            repoProduct.Create(productNew);

            //se recupea el producto con la categoria y proveedor asociado
            var productSelected = repoProduct.Single(x => x.ProductID == productNew.ProductID,
                                                    new List<Expression<Func<Product, object>>>() { x => x.Category, x => x.Supplier });

            Assert.IsNotNull(productSelected.Category);
            Assert.AreEqual(productSelected.Category.CategoryId, productNew.CategoryID);

            Assert.IsNotNull(productSelected.Supplier);
            Assert.AreEqual(productSelected.Supplier.SupplierId, productNew.SupplierID);
        }

        [TestMethod]
        public void Create_Unchange_CategoryAndSupplier_Product()
        {
            ProductRepository repoProduct = new ProductRepository();
            CategoryRepository repoCategory = new CategoryRepository();
            SupplierRepository repoSupplier = new SupplierRepository();

            //se crea la categoria
            Category categoryNew = new Category()
            {
                CategoryName = "category1",
                Description = "desc category 1"
            };
            repoCategory.Create(categoryNew);

            //se crea un proveedor
            Supplier supplierNew = new Supplier()
            {
                CompanyName = "Company 1",
            };
            repoSupplier.Create(supplierNew);


            //se crea el producto relacionado con la categoria
            Product productNew = new Book()
            {
                Title = "El Eduardo",
                Description = @"El Eduardo cuenta la historia de ... bla bla bla",
                Price = 119,
                ISBN = 1452622565,
                PublicationYear = 2002,
                Stock = 5,
                Author = "J. R. R.",
                Category = categoryNew,
                Supplier = supplierNew
            };

            repoProduct.Create(productNew,
                                new List<Expression<Func<Product, object>>>() { x => x.Category, x => x.Supplier });

            //se recupea el producto con la categoria y proveedor asociado
            var productSelected = repoProduct.Single(x => x.ProductID == productNew.ProductID,
                                                    new List<Expression<Func<Product, object>>>() { x => x.Category, x => x.Supplier });

            Assert.IsNotNull(productSelected.Category);
            Assert.AreEqual(productSelected.Category.CategoryId, productNew.CategoryID);

            Assert.IsNotNull(productSelected.Supplier);
            Assert.AreEqual(productSelected.Supplier.SupplierId, productNew.SupplierID);
        }
    }
}
