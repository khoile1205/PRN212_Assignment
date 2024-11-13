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
        internal ApplicationDbContext context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        // Async Add method (keep the same name)
        public async Task<T> Add(T _object)
        {
            try
            {
                await context.AddAsync(_object);  // Use AddAsync for async add
                await context.SaveChangesAsync(); // Use SaveChangesAsync for async save
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _object;
        }

        // Async Delete method (keep the same name)
        public async Task<T> Delete(T _object)
        {
            try
            {
                context.Remove(_object);  // Use Remove for delete
                await context.SaveChangesAsync(); // Use SaveChangesAsync for async save
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _object;
        }

        // Async GetAll method (keep the same name)
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = dbSet;

            try
            {
                // Apply filter if provided
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Apply includes if provided
                if (include != null)
                {
                    query = include(query);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return await query.ToListAsync();
        }

        // Async GetFirstOrDefault method (keep the same name)
        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            try
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.FirstOrDefaultAsync(predicate); // Use FirstOrDefaultAsync for async query execution
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw new Exception("An error occurred while retrieving the data.", ex);
            }
        }

        // Async Update method (keep the same name)
        public async Task<T> Update(T _object)
        {
            try
            {
                var entry = context.Entry(_object);

                // Check if the entity is already being tracked
                if (entry.State == EntityState.Detached)
                {
                    var key = context.Entry(_object).Property("Id").CurrentValue;  // Replace "Id" with the primary key name if different
                    var existingEntity = await dbSet.FindAsync(key);

                    if (existingEntity != null)
                    {
                        // Update the existing tracked entity with new values
                        context.Entry(existingEntity).CurrentValues.SetValues(_object);
                    }
                    else
                    {
                        // Attach and update the entity if it wasn't already tracked
                        dbSet.Attach(_object);
                        entry.State = EntityState.Modified;
                    }
                }
                else
                {
                    entry.State = EntityState.Modified;  // Directly mark as modified if already tracked
                }

                await context.SaveChangesAsync();
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
