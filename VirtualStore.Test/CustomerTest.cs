using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Collections;

namespace VirtualStore.Test
{
    [TestClass]
    public class CustomerTest
    {
        public Customer customer1 = null;
        public Customer customer2 = null;
        public Customer customer3 = null;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<VirtualStoreContext>());

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Customers.RemoveRange(context.Customers);

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
                    Email = "aperez@outlook.com"
                };
                context.Customers.Add(customer1);

                customer2 = new Customer()
                {
                    FirstName = "Eduardo",
                    LastName = "Quispe",
                    BirdDate = DateTime.Now.AddYears(-24),
                    Residence = new Address()
                    {
                        Country = "Argentina",
                        City = "Tambo",
                        Street = "Av mariscal castilla",
                        Number = 4522
                    },
                    Email = "equispe@outlook.com"
                };
                context.Customers.Add(customer2);

                customer3 = new Customer()
                {
                    FirstName = "Manuel",
                    LastName = "Yupanqui",
                    BirdDate = DateTime.Now.AddYears(-30),
                    Residence = new Address()
                    {
                        Country = "Argentina",
                        City = "Chilca",
                        Street = "Av Leoncio Prado",
                        Number = 2500
                    },
                    Email = "myupanqui@outlook.com",
                };
                context.Customers.Add(customer3);

                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetAllCustomers()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var customers = context.Customers.ToList();

                Assert.IsNotNull(customers);
                Assert.AreEqual(customers.Count,3);


                Assert.AreEqual(customers[0].FirstName,customer1.FirstName);
                Assert.AreEqual(customers[0].LastName, customer1.LastName);

                Assert.AreEqual(customers[1].FirstName, customer2.FirstName);
                Assert.AreEqual(customers[1].LastName, customer2.LastName);

                Assert.AreEqual(customers[2].FirstName, customer3.FirstName);
                Assert.AreEqual(customers[2].LastName, customer3.LastName);

            }
        }

        [TestMethod]
        public void GetAllCustomers_UsingDynamic()
        {

            //Recuperamos la lista de cliente agrupada por pais
            //
            IEnumerable<dynamic> dynamicCustomers = GetCustomers_GroupedByCountry_Dynamic();

            //Assert
            Assert.AreEqual(dynamicCustomers.Count(), 2);

            dynamic elem1 = dynamicCustomers.ElementAt(0);
            Assert.AreEqual(elem1.Country, "Argentina");
            Assert.AreEqual(elem1.Customers.Count, 2);

            dynamic elem2 = dynamicCustomers.ElementAt(1);
            Assert.AreEqual(elem2.Country, "Peru");
            Assert.AreEqual(elem2.Customers.Count, 1);

            //Validamos contra la lista original
            List<Customer> customers;
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                customers = context.Customers.ToList();
            }

            foreach (dynamic country in dynamicCustomers)
            {
                foreach (dynamic customer in country.Customers)
                {
                    var exist = customers.Any(x => x.CustomerId == customer.Id
                        && x.LastName + ", "+x.FirstName == customer.Name);
                    Assert.IsTrue(exist);
                }
            }
        }

        [TestMethod]
        public void GetById() 
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var customers = context.Customers.FirstOrDefault(c => c.CustomerId == customer1.CustomerId);

                Assert.IsNotNull(customers);

                Assert.AreEqual(customers.FirstName, customer1.FirstName);
                Assert.AreEqual(customers.LastName, customer1.LastName);
            }
        }

        [TestMethod]
        public void GetCustomers_ByCountry()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var customers = context.Customers.Where(c => c.Residence.Country == "Peru").ToList();

                var customers1 = (from c in context.Customers
                                 where c.Residence.Country == "Peru"
                                 select c).ToList();

                Assert.IsNotNull(customers);

                Assert.AreEqual(customers[0].FirstName, customer1.FirstName);
                Assert.AreEqual(customers[0].LastName, customer1.LastName);
            }
        }

        [TestMethod]
        public void GetCustomers_GroupedByCountry()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var customerGrouped = (from customer in context.Customers
                                       group customer by customer.Residence.Country into g
                                       select new 
                                       {
                                           Country = g.Key,
                                           Customer = g
                                       }).ToList();

                Assert.AreEqual(customerGrouped.Count(),2);

                Assert.AreEqual(customerGrouped[0].Country,"Argentina");
                Assert.AreEqual(customerGrouped[0].Customer.Count(), 2);
                Assert.AreEqual(customerGrouped[1].Country, "Peru");
                Assert.AreEqual(customerGrouped[1].Customer.Count(), 1);
            }
        }

        private IEnumerable<dynamic> GetCustomers_GroupedByCountry_Dynamic()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var result = context.Customers
                                    .GroupBy(x => x.Residence.Country)
                                    .Select(x => new
                                            {
                                                Country = x.Key,
                                                Customers = x.Select(y => new
                                                {
                                                    Id = y.CustomerId,
                                                    Name = y.LastName + ", " + y.FirstName
                                                })
                                            });
                return result.ToList();
            }
        }

        [TestMethod]
        public void GetCustomers_GroupedByCountryAndBirdDate()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                DateTime birdDate = DateTime.Now.AddYears(-25);

                //var customers = (from customer in context.Customers
                //                where customer.Residence.Country == "Peru"
                //                    && customer.BirdDate > birdDate
                //                    && customer.LastName.StartsWith("P")
                //                select customer).ToList();

                var customerArg = from customer in context.Customers
                                  where customer.Residence.Country == "Peru"
                                  select customer;

                var customersStartWithP = from customer in customerArg
                                          where customer.LastName.StartsWith("P")
                                          select customer;

                var customers = customersStartWithP.Where(x => x.BirdDate > birdDate).ToList();

                Assert.IsNotNull(customers);
                Assert.AreEqual(customers.Count(), 1);

                Assert.AreEqual(customers[0].LastName, customer1.LastName);
               
            }
        }
    }
}
