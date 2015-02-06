using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using VirtualStore.Repository.Especific;
using VirtualStore.Repository;
using VirtualStore.Repository.Generic;

namespace VirtualStore.Test
{
    [TestClass]
    public class Employee2Test
    {
        private EmployeeInternal employee1 = null;
        private EmployeeExternal employee2 = null;
        private EmployeeExternal employee3 = null;

        [TestInitialize]
        private void InitializeTestData()
        {
            IEmployee2Repository repoEmployee = new Employee2Repository();

            //
            // elimino registros previos
            //
            List<Employee2> list = repoEmployee.GetAll();

            list.ForEach(x => repoEmployee.Delete(x));


            //
            // creo un empleado interno
            //
            employee1 = new EmployeeInternal()
            {
                FirstName = "name1",
                LastName = "lastname1",
                HireDate = DateTime.Now.AddMonths(-10)
            };
            repoEmployee.Create(employee1);

            //
            // creo un empleado externo
            //
            employee2 = new EmployeeExternal()
            {
                FirstName = "name2",
                LastName = "lastname2",
                ConsultantName = "ConsultantName2",
                ContactExpiration = DateTime.Now.AddYears(2)
            };
            repoEmployee.Create(employee2);

            //
            // creo otro empleado externo
            //
            employee3 = new EmployeeExternal()
            {
                FirstName = "name3",
                LastName = "lastname3",
                ConsultantName = "ConsultantName3",
                ContactExpiration = DateTime.Now.AddYears(1)
            };
            repoEmployee.Create(employee3);
        }
         
        [TestMethod]
        public void GetAll_Employee()
        {
            InitializeTestData();

            //
            //recupero todos los empleados
            //
            IEmployee2Repository repoEmployee = new Employee2Repository();

            List<Employee2> listIntEmployee = repoEmployee.GetAll();

            //
            // Assert
            //
            Assert.AreEqual(listIntEmployee.Count, 3);

            Assert.IsInstanceOfType(listIntEmployee[0], typeof(EmployeeInternal));
            Assert.IsInstanceOfType(listIntEmployee[1], typeof(EmployeeExternal));
            Assert.IsInstanceOfType(listIntEmployee[2], typeof(EmployeeExternal));

        }

        [TestMethod]
        public void GetAllInternal_UsingSpecificRepository_Employee()
        {
            InitializeTestData();

            //
            //recupero solo empleados internos
            //
            IRepository<EmployeeInternal> repoInternalEmployee = new EmployeeInternalRepository();

            List<EmployeeInternal> listIntEmployee = repoInternalEmployee.GetAll();

            //
            // Assert
            //
            Assert.AreEqual(listIntEmployee.Count, 1);

            Assert.IsInstanceOfType(listIntEmployee[0], typeof(EmployeeInternal));

            Assert.AreEqual(listIntEmployee[0].FirstName, employee1.FirstName);
            Assert.IsNotNull(listIntEmployee[0].HireDate);
            Assert.AreEqual(listIntEmployee[0].HireDate.Value.ToShortDateString(), employee1.HireDate.Value.ToShortDateString());

        }

        [TestMethod]
        public void GetAllInternal_UsingGenericRepository_Employee()
        {

            InitializeTestData();

            //
            //recupero solo empleados internos
            //
            IEmployee2Repository repoEmployee = new Employee2Repository();

            List<EmployeeInternal> listIntEmployee = repoEmployee.GetAllInternalType();

            //
            // Assert
            //
            Assert.AreEqual(listIntEmployee.Count, 1);

            Assert.IsInstanceOfType(listIntEmployee[0], typeof(EmployeeInternal));

            Assert.AreEqual(listIntEmployee[0].FirstName, employee1.FirstName);
            Assert.IsNotNull(listIntEmployee[0].HireDate);
            Assert.AreEqual(listIntEmployee[0].HireDate.Value.ToShortDateString(), employee1.HireDate.Value.ToShortDateString());

        }

        [TestMethod]
        public void GetAllExternal_Employee()
        {

            InitializeTestData();

            //
            //recupero solo empleados externos
            //
            IRepository<EmployeeExternal> repoExternalEmployee = new EmployeeExternalRepository();

            List<EmployeeExternal> listExtEmployee = repoExternalEmployee.GetAll();

            //
            // Assert
            //
            Assert.AreEqual(listExtEmployee.Count, 2);

            Assert.IsInstanceOfType(listExtEmployee[0], typeof(EmployeeExternal));
            Assert.IsInstanceOfType(listExtEmployee[1], typeof(EmployeeExternal));

            Assert.AreEqual(listExtEmployee[0].FirstName, employee2.FirstName);
            Assert.IsNotNull(listExtEmployee[0].ContactExpiration);
            Assert.AreEqual(listExtEmployee[0].ContactExpiration.Value.ToShortDateString(), employee2.ContactExpiration.Value.ToShortDateString());
            Assert.AreEqual(listExtEmployee[0].ConsultantName, employee2.ConsultantName);

            Assert.AreEqual(listExtEmployee[1].FirstName, employee3.FirstName);
            Assert.IsNotNull(listExtEmployee[1].ContactExpiration);
            Assert.AreEqual(listExtEmployee[1].ContactExpiration.Value.ToShortDateString(), employee3.ContactExpiration.Value.ToShortDateString());
            Assert.AreEqual(listExtEmployee[1].ConsultantName, employee3.ConsultantName);
        }


        [TestMethod]
        public void GetAll_WithBaseType_Employee()
        {
            IEmployee2Repository repoEmployee = new Employee2Repository();

            //
            // creo un empleado interno
            //
            EmployeeInternal employee1 = new EmployeeInternal()
            {
                FirstName = "name1",
                LastName = "lastname1",
                HireDate = DateTime.Now.AddMonths(-10)
            };
            repoEmployee.Create(employee1);

            //
            // creo un empleado interno
            //
            //saldra error por q esta definida como clase abstracta

            //*//Employee2 employee2 = new Employee2()
            //*//{
            //*//    FirstName = "name2",
            //*//    LastName = "lastname2"
            //*//};
            //*//repoEmployee.Create(employee2);

            //
            //recupero todos los empleados
            //

            List<Employee2> listIntEmployee = repoEmployee.GetAll();

            //
            // Assert
            //
            Assert.AreEqual(listIntEmployee.Count, 2);

            Assert.IsInstanceOfType(listIntEmployee[0], typeof(EmployeeInternal));
            //validamos los tipos base de cada objeto recuperado
            Assert.AreEqual(listIntEmployee[0].GetType().BaseType, typeof(Employee));
            Assert.AreEqual(listIntEmployee[1].GetType().BaseType, typeof(object));
            
            //Al final del test se valida los tipo base de cada instancia, 
            //para el empleado interno será la clase “Employee”, 
            //pero para una instancia base del empleado al no derivar de ninguna otra será el tipo “object”.
        }
        
    }
}
