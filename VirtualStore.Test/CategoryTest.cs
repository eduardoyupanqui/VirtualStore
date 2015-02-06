using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualStore.Repository;

namespace VirtualStore.Test
{
    [TestClass]
    public class CategoryTest
    {
        [TestMethod]
        public void Get_All_Category()
        {
            CategoryRepository repoCategory = new CategoryRepository();

            Category categoryNew = new Category()
            {
                CategoryName = "Categoria 1",
                Description = "Descripcion 1"
            };
            repoCategory.Create(categoryNew);

            var categoryList = repoCategory.GetAll();

            Assert.AreEqual(categoryList.Count, 1);
        }

        [TestMethod]
        public void Get_SingleById_Category()
        {
            CategoryRepository repoCategory = new CategoryRepository();

            Category categoryNew = new Category()
            {
                CategoryName = "Categoria 1",
                Description = "Descripcion 1"
            };
            repoCategory.Create(categoryNew);

            var categorySel = repoCategory.Single(x => x.CategoryId == categoryNew.CategoryId);

            Assert.IsNotNull(categorySel);
            Assert.AreEqual(categorySel.CategoryId, categoryNew.CategoryId);
            Assert.AreEqual(categorySel.CategoryName, categoryNew.CategoryName);
            Assert.AreEqual(categorySel.Description, categoryNew.Description);

        }


        [TestMethod]
        public void Delete_ById_Category()
        {
            CategoryRepository repoCategory = new CategoryRepository();

            //
            //creamos una nueva categoria
            //
            Category categoryNew = new Category()
            {
                CategoryName = "CatName2",
                Description = "Desc2"
            };
            repoCategory.Create(categoryNew);

            //
            //la eliminamos
            //
            repoCategory.Delete(new Category() { CategoryId = categoryNew.CategoryId });

            //
            // se recupera para validar que no exista
            //
            var categorySel = repoCategory.GetById(categoryNew.CategoryId);

            Assert.IsNull(categorySel);

        }


        //Asociación obligatoria – Eliminar en cascada
        [TestMethod]
        public void Delete_Cascade_Category()
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
            Product productNew1 = new Book()
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
            repoProduct.Create(productNew1);

            Product productNew2 = new Movie()
            {
                Title = "El Hombre Araña 3",
                Description = @"El Hombre Araña 3 cuenta la historia de ... bla bla bla",
                Price = 200,
                Duration = new TimeSpan(2, 0, 0),
                LanguageSound = "Español/Ingles",
                Stock = 3,
                CategoryID = categoryNew.CategoryId
            };
            repoProduct.Create(productNew2);


            //elimina la categoria y sus productos asociados
            repoCategory.Delete(categoryNew);


            //se recupera la categoria para validar si se elimino
            var categorySelected = repoCategory.Single(x => x.CategoryId == categoryNew.CategoryId);

            Assert.IsNull(categorySelected);

            //se recupera el primer producto
            Product productSel1 = repoProduct.Single(x => x.ProductID == productNew1.ProductID);
            Assert.IsNull(productSel1);

            //se recupera el primer producto
            Product productSel2 = repoProduct.Single(x => x.ProductID == productNew2.ProductID);
            Assert.IsNull(productSel2);
        }

        //Asociación obligatoria – Anular eliminar en cascada DARA ERROR
        //en el map 
        [TestMethod]
        public void Delete_Cascade_CategoryClone()
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
            Product productNew1 = new Book()
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
            repoProduct.Create(productNew1);

            Product productNew2 = new Movie()
            {
                Title = "El Hombre Araña 3",
                Description = @"El Hombre Araña 3 cuenta la historia de ... bla bla bla",
                Price = 200,
                Duration = new TimeSpan(2, 0, 0),
                LanguageSound = "Español/Ingles",
                Stock = 3,
                CategoryID = categoryNew.CategoryId
            };
            repoProduct.Create(productNew2);


            //elimina la categoria y sus productos asociados
            repoCategory.Delete(categoryNew);


            //se recupera la categoria para validar si se elimino
            var categorySelected = repoCategory.Single(x => x.CategoryId == categoryNew.CategoryId);

            Assert.IsNull(categorySelected);

            //se recupera el primer producto
            Product productSel1 = repoProduct.Single(x => x.ProductID == productNew1.ProductID);
            Assert.IsNull(productSel1);

            //se recupera el primer producto
            Product productSel2 = repoProduct.Single(x => x.ProductID == productNew2.ProductID);
            Assert.IsNull(productSel2);
        }
        


    }
}
