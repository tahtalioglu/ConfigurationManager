namespace ConfigurationManagement.Business.Contract
{
    public interface ICacheManager
    {
        void Add<T>(string key, T value, int durationMinutes);
        void Remove(string key);
        T Get<T>(string key);
 
    }
}
