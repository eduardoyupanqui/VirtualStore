using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtualStore.Repository.Generic
{
    //La implementación base
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {

        public List<T> GetAll()
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return (List<T>)context.Set<T>().ToList();
            }
        }

        public List<T> GetAll(List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                DbQuery<T> query = context.Set<T>();

                includelist.ForEach(x => query = query.Include(x));

                return (List<T>)query.ToList();
            }

        }


        public T Single(Expression<Func<T, bool>> predicate)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return context.Set<T>().FirstOrDefault(predicate);
            }
        }

        public T Single(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                DbQuery<T> query = context.Set<T>();

                includelist.ForEach(x => query = query.Include(x));

                return query.FirstOrDefault(predicate);
            }
        }


        public List<T> Filter(Expression<Func<T, bool>> predicate)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                return (List<T>)context.Set<T>().Where(predicate).ToList();
            }
        }

        public List<T> Filter(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                DbQuery<T> query = context.Set<T>();

                includelist.ForEach(x => query = query.Include(x));

                return (List<T>)query.Where(predicate).ToList();
            }
        }


        public void Create(T entity)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public virtual void Create(T entity, List<Expression<Func<T, object>>> unchangeProp)
        {
            // se obtiene la lista de propiedades que deben marcarse con el estado Unchanged 
            List<string> unchangelist = unchangeProp.Select(x => ((MemberExpression)x.Body).Member.Name).ToList();

            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Set<T>().Add(entity);

                if (unchangeProp != null)
                {
                    // se toma la instancia del objeto que esta asignada a la propiedad 
                    // y se asigna el estodo Unchanged
                    foreach (string property in unchangelist)
                    {
                        PropertyInfo propertyInfo = typeof(T).GetProperty(property);
                        var value = propertyInfo.GetValue(entity, null);

                        context.Entry(value).State = EntityState.Unchanged;
                    }
                }

                context.SaveChanges();
            }
        }


        public void Update(T entity)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            using (VirtualStoreContext context = new VirtualStoreContext())
            {
                var entities = context.Set<T>().Where(predicate).ToList();
                entities.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                context.SaveChanges();
            }
        }

    }
}
