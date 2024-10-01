using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal Prn211AssignmentContext context;
        internal DbSet<T> dbSet;

        public Repository(Prn211AssignmentContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public T Add(T _object)
        {
            try
            {
                context.Add(_object);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _object;
        }

        public T Delete(T _object)
        {
            try
            {
                context.Remove(_object);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
            return _object;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            try
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return query.ToList();

        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            try
            {
                query = query.Where(filter);
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return query.FirstOrDefault();
        }

        public T Update(T _object)
        {
            try
            {
                context.Update(_object);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _object;
        }
    }
}
