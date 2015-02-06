using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualStore.Repository;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

using System.Collections.Generic;
using System.Linq.Expressions;

namespace VirtualStore.Test
{
    [TestClass]
    public class EmployeeTest
    {
        public TestContext TestContext { get; set; }

        Employee employeeNew = null;

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void GetAll_Employee()
        {
            string PhotoPath = Path.Combine(this.TestContext.DeploymentDirectory, "foto.jpg");

            EmployeeRepository repoEmployee = new EmployeeRepository();

            //se eliminan las entidades que pudieran quedar de la ejecucion de test anteriores
            repoEmployee.GetAll().ForEach(x => repoEmployee.DeleteIncludeExtended(x));


            //se crean dos empleados
            CreateEmployeeWithPhoto();

            Employee employee2 = new Employee()
            {
                FirstName = "name 2",
                LastName = "lastname 2",
                EmployeeExt = new EmployeeExtended()
            };
            repoEmployee.Create(employee2);

            var empleados = repoEmployee.GetAll();

            Assert.AreEqual(empleados.Count, 2);
            
            Assert.IsNull(empleados[0].EmployeeExt);
            Assert.IsNotNull(empleados[0].Localization);
            //Assert.IsTrue(empleados[0].Localization.HasValue);//trabaja si se le pone ? a la clase
            Assert.IsTrue(empleados[0].Localization != null);

            Assert.IsNull(empleados[1].EmployeeExt);
            Assert.IsNotNull(empleados[1].Localization);
        }

        //Test – Obtener entidad SIN información extendida
        [TestMethod]
        public void GetById_Employee()
        {
            EmployeeRepository repoEmployee = new EmployeeRepository();

            //se crea el empleado con informacion extendida
            CreateEmployeeWithPhoto();


            //Se recupera la entidad
            Employee employeeSel = repoEmployee.Single(x => x.EmployeeId == employeeNew.EmployeeId);


            Assert.AreEqual(employeeSel.EmployeeId, employeeNew.EmployeeId);
            Assert.IsNotNull(employeeSel.Localization);
            Assert.AreEqual(employeeSel.Localization.Street, "Street 1");
            Assert.AreEqual(employeeSel.Localization.City, "City 1");
            Assert.AreEqual(employeeSel.Localization.Country, "Country 1");

            Assert.IsNull(employeeSel.EmployeeExt);

        }

        //Test – Obtener entidad CON información extendida
        [TestMethod]
        public void GetById_EmployeeExt()
        {
            EmployeeRepository repoEmployee = new EmployeeRepository();

            //se crea el empleado con informacion extendida
            CreateEmployeeWithPhoto();

            //se recupera la entidad y sus propiedad extendida
            Employee employeeSel = repoEmployee.Single(x => x.EmployeeId == employeeNew.EmployeeId,
                                                            new List<Expression<Func<Employee, object>>>() 
                                                            { 
                                                                x=>x.EmployeeExt 
                                                            }
                                                       );

            Assert.AreEqual(employeeSel.EmployeeId, employeeNew.EmployeeId);
            Assert.IsTrue(employeeSel.Localization != null);
            Assert.AreEqual(employeeSel.Localization.Street, "Street 1");
            Assert.AreEqual(employeeSel.Localization.City, "City 1");
            Assert.AreEqual(employeeSel.Localization.Country, "Country 1");

            Assert.IsNotNull(employeeSel.EmployeeExt);
            Assert.IsNotNull(employeeSel.EmployeeExt.Photo);
            Assert.AreEqual(employeeSel.EmployeeExt.PhotoPath, employeeNew.EmployeeExt.PhotoPath);

        }

        //Test – Recuperar SOLO la información extendida
        [TestMethod]
        public void GetById_Employee_RecoverOnlyExtended()
        {
            EmployeeRepository repoEmployee = new EmployeeRepository();

            CreateEmployeeWithPhoto();


            EmployeeExtended employeeSel = repoEmployee.GetExtendedById(employeeNew.EmployeeId);


            Assert.IsNotNull(employeeSel);
            Assert.AreEqual(employeeSel.EmployeeId, employeeNew.EmployeeId);
            Assert.IsNotNull(employeeSel.Photo);
            Assert.AreEqual(employeeSel.PhotoPath, employeeNew.EmployeeExt.PhotoPath);

        }


        //Test – Eliminar empleado con y sin información extendida
        [TestMethod]
        public void Delete_Employee()
        {
            EmployeeRepository repoEmployee = new EmployeeRepository();

            //
            //se crea un empleado con info extendida
            //
            CreateEmployeeWithPhoto();

            //elimina el empleado y su informacion extendida
            repoEmployee.DeleteIncludeExtended(new Employee() { EmployeeId = employeeNew.EmployeeId });

            //se recupera el empleado que fue eliminado en el paso anterior
            Employee employeeSel = repoEmployee.Single(x => x.EmployeeId == employeeNew.EmployeeId);

            Assert.IsNull(employeeSel);


            //
            //se crean el empleados sin informacion extendida
            //
            Employee employee = new Employee()
            {
                FirstName = "name 1",
                LastName = "lastname 1",
                EmployeeExt = new EmployeeExtended()
            };
            repoEmployee.Create(employee);

            repoEmployee.DeleteIncludeExtended(employee);

            //se recupera el empleado que fue eliminado en el paso anterior
            employeeSel = repoEmployee.Single(x => x.EmployeeId == employee.EmployeeId);

            Assert.IsNull(employeeSel);

        }

        private void CreateEmployeeWithPhoto()
                {
                    string PhotoPath = Path.Combine(this.TestContext.DeploymentDirectory, "foto.jpg");
                    EmployeeRepository repoEmployee = new EmployeeRepository();
                    employeeNew = new Employee()
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
                    repoEmployee.Create(employeeNew);
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
