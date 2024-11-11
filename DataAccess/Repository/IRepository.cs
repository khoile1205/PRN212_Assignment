using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>> include = null);
        public Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include = null);
        public Task<T> Add(T _object);
        public Task<T> Delete(T _object);
        public Task<T> Update(T _object);
    }
}
