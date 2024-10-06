using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GenericBuilder
{
    public class GenericBuilder<T> where T : new()
    {
        private readonly T _instance;

        public GenericBuilder(T instance)
        {
            _instance = instance;
        }

        public GenericBuilder<T> SetProperty(Action<T> setProperty)
        {
            setProperty(_instance);
            return this;
        }

        public T Build()
        {
            return _instance;
        }
    }
}
