using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManagement.Data
{
    public class FakeConfigurationStorage : IConfigurationStorage
    {
        public void Add(ConfigurationRecord record)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ConfigurationRecord> Get(string applicationName)
        {
            return null;
        }

        public ConfigurationRecord Get(string id, string applicationName)
        {
            return null;
        }

        public ConfigurationRecord GetWithKey(string key, string applicationName)
        {
            return null;
        }

        public void Remove(string guid)
        {
            throw new NotImplementedException();
        }

        public void Update(ConfigurationRecord record )
        {
            throw new NotImplementedException();
        }
    }
}
