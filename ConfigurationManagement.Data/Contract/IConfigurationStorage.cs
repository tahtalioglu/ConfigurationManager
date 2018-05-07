

namespace ConfigurationManagement.Data
{
    public interface IConfigurationStorage:IStorageRepository<ConfigurationRecord>
    {
        ConfigurationRecord GetWithKey(string key, string applicationName);
        void Remove(string guid);
    }
}
