using System.Collections.Generic;

namespace ConfigurationManagement.Data
{
    public interface IStorageRepository<T>
    {
        void Add(T record);
        IEnumerable<T> Get(string applicationName);
        T Get(string id, string applicationName);
        void Update(T entity);
    }
}
