using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Collections;

namespace VirtualStore.Test
{
    [TestClass]
    public class ShoppingCartTest
    {
        public Book book1 = null;
        public Book book2 = null; 
        public Book book3 = null;
        public Customer customer1 = null;
        public Customer customer2 = null;
        public Movie movie1 = null;
        public Movie movie2 = null;

        [TestMethod]
        public void OneToOne()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<VirtualStoreContext>());

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Customers.RemoveRange(context.Customers);

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

                customer1 = new Customer()
                {
                    FirstName = "Andres",
                    LastName = "Perez",
                    BirdDate = DateTime.Now.AddYears(-20),
                    Residence = new Address()
                    {
                        Country = "Peru",
                        City = "Huancayo",
                        Street = "Av Real",
                        Number = 1300
                    },
                    Email = "aperez@outlook.com",

                    ShoppingCarts = new List<ShoppingCart>()
                    {
                        new ShoppingCart()
                        {
                            PurchaseDate = DateTime.Now,
                            TotalAmount = 320,
                            Discount = 20,
                            Items = new List<ShoppingItem>()
                            {
                                new ShoppingItem()
                                {
                                    Product = book1,
                                    Quantity = 1,
                                    Detail = new ShoppingItemDetail()
                                    {
                                        SalesMan = "Hernandez, Juan",
                                        Observations = "Se aplica descuento"
                                    }
                                }
                            }
                        }
                    }
                };


                context.Customers.Add(customer1);
                context.SaveChanges();

                var customer = context.Customers.FirstOrDefault(x=>x.CustomerId == customer1.CustomerId);
                Assert.IsNotNull(customer);
            }
        }

        [TestMethod]
        public void OneToMany()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<VirtualStoreContext>());

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                //context.Customers.RemoveRange(context.Customers);

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

                movie1 = new Movie()
                {
                    Title = "El Hombre Araña 3",
                    Description = @"El Hombre Araña 3 cuenta la historia de ... bla bla bla",
                    Price = 200,
                    Duration = new TimeSpan(2, 0, 0),
                    LanguageSound = "Español/Ingles",
                    Stock = 3
                };
                context.Products.Add(movie1);

                context.SaveChanges();

                customer1 = new Customer()
                {
                    FirstName = "Andres",
                    LastName = "Perez",
                    BirdDate = DateTime.Now.AddYears(-20),
                    Residence = new Address()
                    {
                        Country = "Peru",
                        City = "Huancayo",
                        Street = "Av Real",
                        Number = 1300
                    },
                    Email = "aperez@outlook.com",

                    ShoppingCarts = new List<ShoppingCart>()
                    {
                        new ShoppingCart()
                        {
                            PurchaseDate = DateTime.Now.AddMonths(-1),
                            TotalAmount = 480,
                            Discount = 0,
                            Items = new List<ShoppingItem>()
                            {
                                new ShoppingItem()
                                {
                                    ProductID = book1.ProductID,
                                    Quantity = 2
                                },
                                new ShoppingItem()
                                {
                                    ProductID = movie1.ProductID,
                                    Quantity = 1
                                }
                            }
                        },
                        new ShoppingCart()
                        {
                            PurchaseDate = DateTime.Now,
                            TotalAmount = 140,
                            Discount = 20,
                            Items = new List<ShoppingItem>()
                            {
                                new ShoppingItem()
                                {
                                    ProductID = book1.ProductID,
                                    Quantity = 1
                                }
                            }
                        }
                    }
                };

                context.Customers.Add(customer1);
                context.SaveChanges();

                var customer = context.Customers.FirstOrDefault(x => x.CustomerId == customer1.CustomerId);
                Assert.IsNotNull(customer);
            }
        }

        [TestMethod]
        public void ManyToMany()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<VirtualStoreContext>());
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var genre1 = new Genre() { Description = "Ciencia Ficción" };
                context.Genres.Add(genre1);
                var genre2 = new Genre() { Description = "Fantasia" };
                context.Genres.Add(genre2);

                context.SaveChanges();

                var book1 = new Book()
                {
                    Title = "El Silmarillon",
                    Description = @"El Silmarillon cuenta la historia de ... bla bla bla",
                    Price = 139,
                    ISBN = 1452356565,
                    PublicationYear = 2002,
                    Stock = 5,
                    Author = "J. R. R. Tolkien",
                    Genre = new List<Genre>()
                    {
                        genre1,genre2
                    }
                };

                context.Products.Add(book1);

                var book2 = new Book()
                {
                    Title = "El Nombre del viento",
                    Description = @"El Nombre del viento cuenta la historia de ... bla bla bla",
                    Price = 319,
                    ISBN = 1452121512,
                    PublicationYear = 2009,
                    Stock = 10,
                    Author = "Patrick",
                    Genre = new List<Genre>()
                    {
                        genre1
                    }
                };

                context.Products.Add(book2);

                context.SaveChanges();

                var books = context.Products.OfType<Book>().ToList();

                Assert.IsNotNull(books);
            }
        }

        //CARGAR ENTIDADES RELACIONADAS

        public void LlenarDatos()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<VirtualStoreContext>());

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                #region Products
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
                    Duration = new TimeSpan(2, 0, 0),
                    LanguageSound = "Español/Ingles",
                    Stock = 3
                };
                context.Products.Add(movie1);

                movie2 = new Movie()
                {
                    Title = "Capitan America",
                    Description = @"Capitan America cuenta la historia de ... bla bla bla",
                    Price = 100,
                    Duration = new TimeSpan(2, 30, 0),
                    LanguageSound = "Español/Ingles",
                    Stock = 5
                };
                context.Products.Add(movie2);
                context.SaveChanges();
                #endregion

                #region customer1
                customer1 = new Customer()
                {
                    FirstName = "Andres",
                    LastName = "Perez",
                    BirdDate = DateTime.Now.AddYears(-20),
                    Residence = new Address()
                    {
                        Country = "Peru",
                        City = "Huancayo",
                        Street = "Av Real",
                        Number = 1300
                    },
                    Email = "aperez@outlook.com",

                    ShoppingCarts = new List<ShoppingCart>()
                    {
                        new ShoppingCart()
                        {
                            PurchaseDate = DateTime.Now,
                            TotalAmount = 340,
                            Discount = 20,
                            Items = new List<ShoppingItem>()
                            {
                                new ShoppingItem()
                                {
                                    ProductID = book1.ProductID,
                                    Quantity = 1,
                                    Detail = new ShoppingItemDetail()
                                    {
                                        SalesMan = "Hernandez, Juan",
                                        Observations = "Se aplica descuento"
                                    }
                                },
                                new ShoppingItem()
                                {
                                    ProductID = book3.ProductID,
                                    Quantity = 1
                                }
                            }
                        }
                    }
                };

                context.Customers.Add(customer1);
                #endregion

                #region customer2
                //customer2 = new Customer()
                //{
                //    FirstName = "Andres",
                //    LastName = "Perez",
                //    BirdDate = DateTime.Now.AddYears(-20),
                //    Residence = new Address()
                //    {
                //        Country = "Peru",
                //        City = "Huancayo",
                //        Street = "Av Real",
                //        Number = 1300
                //    },
                //    Email = "aperez@outlook.com",

                //    ShoppingCarts = new List<ShoppingCart>()
                //    {
                //        new ShoppingCart()
                //        {
                //            PurchaseDate = DateTime.Now.AddMonths(-1),
                //            TotalAmount = 480,
                //            Discount = 0,
                //            Items = new List<ShoppingItem>()
                //            {
                //                new ShoppingItem()
                //                {
                //                    ProductID = book1.ProductID,
                //                    Quantity = 2
                //                },
                //                new ShoppingItem()
                //                {
                //                    ProductID = movie1.ProductID,
                //                    Quantity = 1
                //                }
                //            }
                //        },
                //        new ShoppingCart()
                //        {
                //            PurchaseDate = DateTime.Now,
                //            TotalAmount = 140,
                //            Discount = 20,
                //            Items = new List<ShoppingItem>()
                //            {
                //                new ShoppingItem()
                //                {
                //                    ProductID = book1.ProductID,
                //                    Quantity = 1
                //                }
                //            }
                //        }
                //    }
                //};
                //context.Customers.Add(customer2);
                #endregion

                context.SaveChanges();
            }

        }
        //carga ociosa de datos
        [TestMethod]
        public void LazyLoadTest()
        {
            LlenarDatos();
            //Hace q se pueda acceder los datos internos del objeto segun  la demanda que se quiera, osea una busqueda completa
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var customer = context.Customers.FirstOrDefault(x => x.CustomerId == customer1.CustomerId);
                //var lista = customer.ShoppingCarts;
                //int listac = lista!=null?lista.Count:0;
                Assert.IsNotNull(customer);
            }

        }

        [TestMethod]
        public void LazyLoadTestOutContext()
        {
            //Si esta fuera de contexto using, hace q genere error al tratar de recueprar indformacion interna
            LlenarDatos();
            Customer customer = null;
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                customer = context.Customers.FirstOrDefault(x => x.CustomerId == customer1.CustomerId);
            }
            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void IncludeTest()
        {
            LlenarDatos();
            Customer customer = null;
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                customer = context.Customers
                    .Include(x=>x.ShoppingCarts)
                    .Include(x => x.ShoppingCarts.Select(y => y.Items))
                    .Include(x => x.ShoppingCarts.Select(y => y.Items.Select(z=>z.Product)))
                    .FirstOrDefault(x => x.CustomerId == customer1.CustomerId);
            }
            Assert.IsNotNull(customer);
        }

        //CONSULTANDO ENTIDADES RELACIONADAS
        [TestMethod]
        public void GetProductsByCustomer()
        {
            LlenarDatos();
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var products = context.Customers
                    .Where(c=>c.CustomerId == customer1.CustomerId)
                    .SelectMany(x=>x.ShoppingCarts.SelectMany(y=>y.Items.Select(z=>z.Product)));

                var products1 = from customer in context.Customers
                                join cart in context.ShoppingCarts on customer.CustomerId equals cart.CustomerId
                                join item in context.ShoppingItems on cart.ShoppingCartId equals item.ShoppingCartId
                                join product in context.Products on item.ProductID equals product.ProductID
                                where customer.CustomerId == customer1.CustomerId
                                select product;
                
                var productList = products.ToList();
                var productList1 = products1.ToList();

                Assert.AreEqual(productList.Count, 2);
                Assert.IsInstanceOfType(productList[0], typeof(Book));
                Assert.IsInstanceOfType(productList[1], typeof(Book));

            }
    }
    }
}
