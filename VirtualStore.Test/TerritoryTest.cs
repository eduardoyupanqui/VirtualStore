using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VirtualStore.Repository;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace VirtualStore.Test
{
    [TestClass]
    public class TerritoryTest
    {
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void inicializar()
        {
            //DbHelper.CreateDb();

        }

        //Test – Obtener territorio con empleados asociados
        [TestMethod]
        public void Get_ListEmployee_Territory()
        {
            TerritoryRepository repoTerritory = new TerritoryRepository();

            Territory territoryNew = new Territory()
            {
                TerritoryDescription = "territoty 1",
                Employees = new List<Employee>()
                {
                    new Employee() 
                    { 
                        FirstName = "Name 1", 
                        LastName ="LastaName 1",
                        Localization = new Address()
                        {
                            Street = "Street 1",
                            City = "City 1",
                            Country = "Country 1"
                        },
                        EmployeeExt = new EmployeeExtended()
                        {
                            Notes = "xx xx xx xx"
                        }
                    },
                    new Employee() 
                    { 
                        FirstName = "Name 2", 
                        LastName ="LastaName 2"
                    }
                }
            };
            repoTerritory.Create(territoryNew);

            var territorySel = repoTerritory.Single(x => x.TerritoryId == territoryNew.TerritoryId,
                                                        new List<Expression<Func<Territory, object>>>() { x => x.Employees });

            Assert.IsNotNull(territorySel);
            Assert.IsNotNull(territorySel.Employees);
            Assert.AreEqual(territorySel.Employees.Count, 2);

        }

        [TestMethod]
        public void Update_ExistingEmployee_Territory()
        {
            string PhotoPath = Path.Combine(this.TestContext.DeploymentDirectory, "foto.jpg");

            TerritoryRepository repoTerritory = new TerritoryRepository();
            EmployeeRepository repoEmployee = new EmployeeRepository();

            //se crean los empleados
            Employee employeeNew1 = new Employee()
            {
                FirstName = "name 1",
                LastName = "lastname 1",
                Localization = new Address()
                {
                    Street = "Street 1",
                    City = "City 1",
                    Country = "Country 1"
                },
                EmployeeExt = new EmployeeExtended()
                {
                    Notes = "xx xx xx xx",
                    Photo = ConvertImageToByteArray(new Bitmap(PhotoPath), ImageFormat.Jpeg),
                    PhotoPath = PhotoPath
                }

            };
            repoEmployee.Create(employeeNew1);

            Employee employeeNew2 = new Employee()
            {
                FirstName = "name 2",
                LastName = "lastname 2",
                Localization = new Address()
                {
                    Street = "Street 2",
                    City = "City 2",
                    Country = "Country 2"
                },
                EmployeeExt = new EmployeeExtended()
                {
                    Notes = "xx xx xx",
                    Photo = ConvertImageToByteArray(new Bitmap(PhotoPath), ImageFormat.Jpeg),
                    PhotoPath = PhotoPath
                }

            };
            repoEmployee.Create(employeeNew2);

            //se crea el territorio
            Territory territoryNew = new Territory()
            {
                TerritoryDescription = "territoty 1"
            };
            repoTerritory.Create(territoryNew);

            //asignamos los empleados al territorio existente
            repoTerritory.AddEmployees(territoryNew, new List<Employee>(new Employee[] { employeeNew1, employeeNew2 }));

            //validamos que la asignacion se haya realizado correctamente 
            //recuperando la entidad y sus relaciones
            var territorySel = repoTerritory.Single(x => x.TerritoryId == territoryNew.TerritoryId,
                                                        new List<Expression<Func<Territory, object>>>() { x => x.Employees });

            Assert.IsNotNull(territorySel);
            Assert.IsNotNull(territorySel.Employees);
            Assert.AreEqual(territorySel.Employees.Count, 2);


        }


        [TestMethod]
        public void Delete_AssignedEmployee_Territory()
        {
            TerritoryRepository repoTerritory = new TerritoryRepository();
            EmployeeRepository repoEmployee = new EmployeeRepository();

            //se crean los empleados
            Employee employeeNew1 = new Employee()
            {
                FirstName = "Name 1",
                LastName = "LastaName 1"
            };
            repoEmployee.Create(employeeNew1);

            Employee employeeNew2 = new Employee()
            {
                FirstName = "Name 2",
                LastName = "LastaName 2"
            };
            repoEmployee.Create(employeeNew2);

            //se crea el territorio
            Territory territoryNew = new Territory()
            {
                TerritoryDescription = "territoty 1"
            };
            repoTerritory.Create(territoryNew);

            //asignamos los empleados al territorio existente
            repoTerritory.AddEmployees(territoryNew, new List<Employee>(new Employee[] { employeeNew1, employeeNew2 }));

            //validamos que la asignacion se haya realizado correctamente 
            //recuperando la entidad y sus relaciones
            var territorySel = repoTerritory.Single(x => x.TerritoryId == territoryNew.TerritoryId,
                                                        new List<Expression<Func<Territory, object>>>() { x => x.Employees });

            Assert.IsNotNull(territorySel);
            Assert.IsNotNull(territorySel.Employees);
            Assert.AreEqual(territorySel.Employees.Count, 2);

            //removemos uno de los empleados asignados
            repoTerritory.RemoveEmployees(territoryNew, new List<Employee>(new Employee[] { employeeNew1 }));

            //recuperamos el territorio para validar que se haya eliminado el empleado 
            var territorySel2 = repoTerritory.Single(x => x.TerritoryId == territoryNew.TerritoryId,
                                                        new List<Expression<Func<Territory, object>>>() { x => x.Employees });

            Assert.IsNotNull(territorySel2);
            Assert.IsNotNull(territorySel2.Employees);
            Assert.AreEqual(territorySel2.Employees.Count, 1);

            Employee employeeSel = territorySel2.Employees.First();
            Assert.AreEqual(employeeSel.FirstName, employeeNew2.FirstName);
            Assert.AreEqual(employeeSel.LastName, employeeNew2.LastName);
        }



        public static byte[] ConvertImageToByteArray(Image _image, ImageFormat _formatImage)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _image.Save(ms, _formatImage);
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
