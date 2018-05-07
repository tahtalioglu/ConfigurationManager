using ConfigurationManagement.Data;
using System;

namespace ConfigurationManagement.Business
{
    public class ConfigurationStorageFactory
    {
        public virtual IConfigurationStorage CreateStorage(string connectionString, string type)
        {

            if (type == "Mongo")
            {
                return new MongoConfigurationStorage(connectionString);
            }
            else if (type == "Fake")
            {
                return new FakeConfigurationStorage();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
