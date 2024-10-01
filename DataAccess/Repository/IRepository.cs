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
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        public T Add(T _object);
        public T Delete(T _object);
        public T Update(T _object);
    }
}
