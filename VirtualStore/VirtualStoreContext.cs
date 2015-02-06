using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtualStore
{
    public class VirtualStoreContext : DbContext
    {
        public VirtualStoreContext() : base("VirtualStore")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee2> Employee2s { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<EmployeeExtended> EmployeeExtendeds { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DEFINIR UNA CLAVE A UNA ENTIDAD FUERA DE LA CONVENCION
            //modelBuilder.Entity<Customer>().HasKey(p=>p.Code); //PARA ESCOGER OTRO ID



            //REMOVER UNA CONVENCION "using System.Data.Entity.ModelConfiguration.Conventions"
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();



            ////REDEFINICIENDO CONVENCIONES
            ////todas las propiedades que terminen en code seran clave primaria
            //modelBuilder.Properties<int>().Where(p=> p.Name.EndsWith("Code"))
            //    .Configure(x =>x.IsKey());



            //todas las propiedaded que sean string seran de maxlengh 50
            modelBuilder.Properties<string>().Where(p=> p.Name == "Description")
                .Configure(p => p.HasMaxLength(50));

            modelBuilder.Properties<string>().
                Configure(p => p.HasMaxLength(250));
            //Agregar una convencion extendida de la clase Convention
            modelBuilder.Conventions.Add(new DateTime2Convention());



            ////DEFINIR UN TIPO COMPLEJO a) desde el onmodeling  b) mirar la clase AddressMap
            //modelBuilder.ComplexType<Address>()
            //    .EmployeeId(p => p.Street)
            //    .HasMaxLength(100);
            ////mapeo de tipo complejo b) mirar la clase AddressMap
            //modelBuilder.Configurations.Add(new AddressMap());

            ////DEFINIR UN TIPO DE HERENCIA
            ////Table-Per-Hierarchy TPH Tabla por jerarquia o ponerlo en ProductoMap
            //modelBuilder.Entity<Product>()
            //    .Map<Book>(m => m.Requires("Type").HasValue(0))
            //    .Map<Movie>(m => m.Requires("Type").HasValue(1))
            //    .Map<Music>(m => m.Requires("Type").HasValue(2));
            ////Table-Per-Type TPT Tabla por Tipo o ponerlo en cada Map
            //modelBuilder.Entity<Book>().ToTable("Books");
            //modelBuilder.Entity<Movie>().ToTable("Movies");
            //modelBuilder.Entity<Music>().ToTable("Musics");
            ////



            //MAPEOS DE ENTIDADES

            //mapeo recuperando los entidadMAPS extendidas por entityTypeConfiguration<T> del proyecto
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            //mapeo clase por clase de la sgte manera
            //modelBuilder.Configurations.Add(new CustomerMap());

            ////DEFINIR UN TIPO COMPLEJO a) desde el onmodeling  b) mirar la clase AddressMap
            //modelBuilder.ComplexType<Address>()
            //    .EmployeeId(p => p.Street)
            //    .HasMaxLength(100);
            


            base.OnModelCreating(modelBuilder);
        }


        //SI DESEO HACER UNA AUDITORIA DE CAMBIOS PARA GUARDARLOS
        public override int SaveChanges()
        {
            var cambios = ChangeTracker.Entries();
            if (cambios.Any())
            {
                var Estado = cambios.First().State.ToString();
                var Entidad = cambios.First().Entity.ToString();
            }

            return base.SaveChanges();
        }
    }

    public class CustomerMap : EntityTypeConfiguration<Customer> 
    {
        public CustomerMap()
        {
            //Definir la clave principal
            HasKey(p=>p.CustomerId);
            //Ignorar una propiedad
            Ignore(p=> p.FullName);
            //opcional "null or not null"
            Property(p => p.Email).IsOptional();
            //Mapear con un nombre en especifico
            Property(p => p.Email)
                .HasColumnName("PersonalEmail");
            //Redifinir el tipo con la q sera creada
            Property(p => p.BirdDate)
                .HasColumnType("datetime2");
            //Redifinir el tipo con la q sera creada, y cuan largo sera
            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50);


            Property(p => p.Residence.Street).HasColumnName("ResidenceStreet");
        }
    }

    public class EmployeeExtendedMap : EntityTypeConfiguration<EmployeeExtended>
    {
        public EmployeeExtendedMap()
        {
            ToTable("Employees");
            HasKey(x => x.EmployeeId);

            Property(x => x.Notes)
            .HasColumnType("ntext")
            .IsOptional();

            Property(x => x.Photo)
            .HasColumnType("image")
            .IsOptional();

            Property(x => x.PhotoPath)
            .HasColumnType("nvarchar")
            .HasMaxLength(255)
            .IsOptional();

        }
    }

    public class EmployeeMap : EntityTypeConfiguration<Employee> 
    {
        public EmployeeMap()
        {
            ToTable("Employees");
            HasKey(x => x.EmployeeId);

            Property(x => x.LastName)
            .HasMaxLength(20)
            .IsRequired();

            Property(x => x.FirstName)
            .HasMaxLength(10)
            .IsRequired();

            HasRequired(x => x.EmployeeExt)
            .WithRequiredPrincipal();
        }
    }

    #region Herencia - Tabla por jerarquía - Table per Hierarchy (TPH)
    public class Employee2Map : EntityTypeConfiguration<Employee2>
    {
        public Employee2Map()
        {
            HasKey(x => x.EmployeeId);
            Property(x => x.LastName).HasMaxLength(20).IsRequired();
            Property(x => x.FirstName).HasMaxLength(10).IsRequired();

            Property(x => x.Address).HasMaxLength(60);
            Property(x => x.City).HasMaxLength(15);
            Property(x => x.Region).HasMaxLength(15);
            Property(x => x.PostalCode).HasMaxLength(10);
            Property(x => x.Country).HasMaxLength(15);

            //En ellas se define el nombre del campo que actuara como discriminador del tipo, 
            //así como los valores que tomara para cada clase hija definida, opcionalmente se 
            //puede especificar el tipo y precisión de la columna.

            //Sino se especifica el tipo para la columna del discriminador Entity Framework 
            //usara valores por defecto, por lo que la columna podrías crearse como nvarchar(128), 
            //esto puede resultar de poca importancia, pero si solo se va a contener un único carácter 
            //se estaría desperdiciando espacio para ese campo.

            //El campo definido como discriminador no se define como propiedad en las clases de la entidad 
            //de dominio, ya que la propia instancia de la clase define el tipo en si mismo.
            Map<EmployeeInternal>(x => x.Requires("Type")
            .HasValue("I")
            .HasColumnType("char")
            .HasMaxLength(1));

            Map<EmployeeExternal>(x => x.Requires("Type")
            .HasValue("E"));


            //si queremos unos Campos discriminador numérico
            //Map<EmployeeInternal>(x => x.Requires("Type")
            //    .HasValue(0));
            //Map<EmployeeExternal>(x => x.Requires("Type")
            //    .HasValue(1));
        }
    }
    #endregion
    #region Herencia - Tabla por tipo - Table per Type (TPT)
    //public class Employee2Map : EntityTypeConfiguration<Employee>
    //{
    //    public EmployeeMap()
    //    {
    //        HasKey(x => x.EmployeeId);
    //        Property(x => x.LastName).HasMaxLength(20).IsRequired();
    //        Property(x => x.FirstName).HasMaxLength(10).IsRequired();

    //    }
    //}

    //public class Employee2InternalMap : EntityTypeConfiguration<EmployeeInternal>
    //{
    //    public EmployeeInternalMap()
    //    {
    //        ToTable("EmployeeInternal");


    //    }
    //}

    //public class Employee2ExternalMap : EntityTypeConfiguration<EmployeeExternal>
    //{
    //    public EmployeeExternalMap()
    //    {
    //        ToTable("EmployeeExternal");

    //        Property(x => x.ConsultantName).IsRequired()
    //        .HasColumnType("varchar")
    //        .HasMaxLength(100);
    //    }
    //}

    //
    #endregion
    #region Herencia - Tabla por tipo concreto - Table per Concrete Type (TPC) 
    //porque base es abtract y va haber problema con el id , ver repositorio IEmployee2Repository
    //public class EmployeeInternalMap : EntityTypeConfiguration<EmployeeInternal>
    //{
    //    public EmployeeInternalMap()
    //    {

    //        Map(x =>
    //        {
    //            //Se destaca del mapping la invocación al método MapInheritedProperties(), 
    //            //lo cual aplica un re-mapping las propiedades de la clase base.
    //            x.MapInheritedProperties();
    //            x.ToTable("InternalEmployee");
    //        });


    //    }
    //}

    //public class EmployeeExternalMap : EntityTypeConfiguration<EmployeeExternal>
    //{
    //    public EmployeeExternalMap()
    //    {

    //        Map(x =>
    //        {
    //            x.MapInheritedProperties();
    //            x.ToTable("ExternalEmployee");
    //        });

    //        Property(x => x.ConsultantName).IsRequired()
    //        .HasColumnType("varchar")
    //        .HasMaxLength(100);
    //    }
    //}
    #endregion

    public class TerritoryMap : EntityTypeConfiguration<Territory>
    {
        public TerritoryMap()
        {
            HasKey(x => x.TerritoryId);
            Property(x => x.TerritoryDescription).HasColumnType("nchar").HasMaxLength(50).IsRequired();

            HasMany(x => x.Employees)//para definir la relación mucho a muchos//or HasOptional
            .WithMany(x => x.Territories)//Unidirectional WithMany() //bidirectional WithMany(x => x.Territories)
            .Map(mc =>//FK column Name
            {
                //Nota: la definición de la relación no era obligatorio hacerlo en la entidad Territory, 
                //se podría haber realizado en la clase Map del Employee sin ningún problema, 
                //solo hay que tener en cuenta cambiaran las definiciones de MapLeftKey() y MapRighKey()
                mc.ToTable("EmployeeTerritories");
                mc.MapLeftKey("TerritoryId");
                mc.MapRightKey("EmployeeId");
            });
        }
    }

    public class SupplierMap : EntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            HasKey(x => x.SupplierId);
            Property(x => x.CompanyName).HasMaxLength(40).IsRequired();

            //ComplexType para una sola entidad , y si no , hacemos su mapping
            Property(x => x.Contact.ContactName).HasColumnName("ContactName").HasMaxLength(30);
            Property(x => x.Contact.ContactTitle).HasColumnName("ContactTitle").HasMaxLength(30);
            Property(x => x.Contact.Phone).HasColumnName("Phone").HasMaxLength(24);
            Property(x => x.Contact.Fax).HasColumnName("Fax").HasMaxLength(24);
            Property(x => x.Contact.HomePage).HasColumnName("HomePage").HasColumnType("ntext");

        }
    }

    public class ShoppingCartMap : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartMap()
        {
            HasKey(p => p.ShoppingCartId);
            //eliminar identity cuando se crea la columna
            //EmployeeId(p => p.ShoppingCartId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            //si es dato numerico, define la presicion del campo
            Property(p => p.TotalAmount).HasPrecision(8, 2);

            //OBSERVAR OJO
            HasRequired(c=>c.Customer)
                .WithMany(c=>c.ShoppingCarts)
                .HasForeignKey(c => c.CustomerId);

        }
    }

    public class ShoppingItemMap : EntityTypeConfiguration<ShoppingItem>
    {
        public ShoppingItemMap()
        {
            //Definir claves compuestas en este caso una mas de la q ya esta
            //HasKey(p => new { p.ShoppingItemID, p.Secuential });

            //RELACION 1 A N Con relacion a Producto (osea nos interesa un sentido de navegacion)
            HasRequired(p => p.Product)//ida hacia productos
                .WithMany()//solo ncesitamos un sentido de navegacion
                .HasForeignKey(p=>p.ProductID);//que propiedad se utilizara para la asociacion
            //RELACION 1 A N Con relacion a ShoppingCart
            HasRequired(p => p.ShoppingCart)//ida hacia ShoppingCart
                .WithMany(p => p.Items)//vuelta (ya q nos interesa la navegacion inversa)
                .HasForeignKey(p => p.ShoppingCartId);//que se utilizara como campo de navegacion
        }
    }

    public class ShoppingItemDetailMap : EntityTypeConfiguration<ShoppingItemDetail>
    {
        public ShoppingItemDetailMap()
        {
            //RELACION 1 A 1 Con relacion a ShoppingItem
            //a)EN ESTE CASO LA TABLA SE RELACION DE 1 A 0o1
            //Definida como requerida la relacion en caso de q se quiera q sea optional seria asi
            HasRequired(p => p.Item)
                .WithOptional(p=>p.Detail);
            //b)EN ESTE CASO LA TABLA SE RELACION DE 1 A 1
            //Definida como requerida la relacion si o si
            //HasRequired(p => p.Item)
            //    .WithRequiredPrincipal(p => p.Detail);

            //TENER PRESENTE QUE , ESTAS DOS DEFINICIONES SON LA MISMA PARA 1 A 1
            //EN ShoppingItemDetailMap
            //HasRequired(p => p.Item)
            //    .WithOptional(p => p.Detail);
            //EN ShoppingItemMap
            //HasOptional(p => p.Detail)
            //    .WithRequired(p => p.Item);




        }
    }

    public class AddressMap : ComplexTypeConfiguration<Address>
    {
        public AddressMap()
        {
            Property(p => p.Street).HasMaxLength(100);
        }
    }

    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            ToTable("Categories");

            HasKey(c => c.CategoryId);
            //estamos redundando , ya q EF lo pone indentity por default
            Property(c => c.CategoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.CategoryName).IsRequired().HasMaxLength(15);
            Property(c => c.Description).HasColumnType("ntext");
        }
    }
    #region Table per Hierarchy
    //public class ProductoMap : EntityTypeConfiguration<Product>
    //{
    //    public ProductoMap()
    //    {
    //        Map<Book>(m => m.Requires("Type").HasValue(0));
    //        Map<Movie>(m => m.Requires("Type").HasValue(1));
    //        Map<Music>(m => m.Requires("Type").HasValue(2));
    //
    //        //CATEGORIA
    //        HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryID)
    //              .WillCascadeOnDelete(false);//LO IDEAL ya q producto es fuera del contexto de categoria
    //        //esta no porq sera utilizada en Table per Type
    //        //Nota: se quito la propiedad que define el ICollection<> hacia los Productos la configuración cambiaria a:
    //        //HasRequired(x => x.Category).WithMany().HasForeignKey(x => x.CategoryID);
    //
    //        HasOptional(x => x.Supplier).WithMany(x => x.Products).HasForeignKey(x => x.SupplierID);
    //        //HasOptional(x => x.Supplier).WithMany(x => x.Products).HasForeignKey(x => x.SupplierID);
    //    }
    //}
    #endregion
    #region Table per Type
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            ToTable("Books");
            //RELACION N a N
            //Creando Map para los generos de cada producto Books
            HasMany(b => b.Genre)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("BooksGenre");
                    m.MapLeftKey("ProductID");
                    m.MapRightKey("GenreID");
                });



            //CATEGORIA
            //esta no porq sera utilizada en Table per Hierarchy
            //HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryID);
            HasRequired(x => x.Category).WithMany().HasForeignKey(x => x.CategoryID)
                .WillCascadeOnDelete(false);//LO IDEAL ya q producto es fuera del contexto de categoria

            HasOptional(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierID);
        }
    }
    public class MovieMap : EntityTypeConfiguration<Movie>
    {
        public MovieMap()
        {
            ToTable("Movies");
            //RELACION N a N
            //Creando Map para los generos de cada producto Movies
            HasMany(b => b.Genre)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("MoviesGenre");
                    m.MapLeftKey("ProductID");
                    m.MapRightKey("GenreID");
                });


            //CATEGORIA
            //esta no porq sera utilizada en Table per Hierarchy
            //HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryID);
            HasRequired(x => x.Category).WithMany().HasForeignKey(x => x.CategoryID)
                .WillCascadeOnDelete(false);//LO IDEAL ya q producto es fuera del contexto de categoria
            HasOptional(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierID);
        }
    }
    public class MusicMap : EntityTypeConfiguration<Music>
    {
        public MusicMap()
        {
            ToTable("Musics");

            //CATEGORIA
            //esta no porq sera utilizada en Table per Hierarchy
            //HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryID);
            HasRequired(x => x.Category).WithMany().HasForeignKey(x => x.CategoryID)
                .WillCascadeOnDelete(false);//LO IDEAL ya q producto es fuera del contexto de categoria

            HasOptional(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierID);
        }
    }
    #endregion

    public class DateTime2Convention : Convention
    {
        public DateTime2Convention()
        {
            this.Properties<DateTime>().Configure(p=> p.HasColumnType("datetime2"));
        }
    }

}
