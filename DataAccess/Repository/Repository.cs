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
        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            try
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it or rethrow a specific exception)
                // For example, log the error:
                Console.WriteLine($"An error occurred: {ex.Message}");

                // You might want to throw a custom exception or return default
                throw new Exception("An error occurred while retrieving the data.", ex);
            }
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
