using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace ConfigurationManagement.Data
{
    public class MongoConfigurationStorage : IConfigurationStorage
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<ConfigurationRecord> _collection;
        public MongoConfigurationStorage(string connectionString)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("config");// db
            _collection = _database.GetCollection<ConfigurationRecord>("records");
         
        }

        public void Add(ConfigurationRecord record)
        {
            var document = new ConfigurationRecord()
            {
                ApplicationName = record.ApplicationName,
                IsActive = true,
                Name = record.Name,
                Type = record.Type,
                Value = record.Value 

            };
            var hede = _collection.InsertOneAsync(document);
            var status = hede.Status;
        }

        public IEnumerable<ConfigurationRecord> Get(string applicationName)
        {
            
            var builder = Builders<ConfigurationRecord>.Filter;
            FilterDefinition<ConfigurationRecord> filt = builder.Eq(p => p.IsActive, true) &
                                                         builder.Eq(p => p.ApplicationName, applicationName);
            var result = _collection.Find<ConfigurationRecord>(filt);
            return result.ToList();
        }

        public ConfigurationRecord Get(string id, string applicationName)
        {

            var builder = Builders<ConfigurationRecord>.Filter;
            FilterDefinition<ConfigurationRecord> filt = builder.Eq(p => p.GuId, new ObjectId(id)) &
                                                         builder.Eq(p => p.ApplicationName, applicationName);
            var result = _collection.Find<ConfigurationRecord>(filt);
            return result.FirstOrDefault();
        }



        public ConfigurationRecord GetWithKey(string key, string applicationName)
        {
            var builder = Builders<ConfigurationRecord>.Filter;
            FilterDefinition<ConfigurationRecord> filt = builder.Eq(p => p.Name, key) &
                                                         builder.Eq(p => p.IsActive, true) &
                                                         builder.Eq(p => p.ApplicationName, applicationName);
            IFindFluent<ConfigurationRecord, ConfigurationRecord> result = _collection.Find<ConfigurationRecord>(filt);
            if (result != null)
            {
                return result.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public void Remove(string guid)
        {
            ConfigurationRecord entity = null;
            var builder = Builders<ConfigurationRecord>.Filter;
            FilterDefinition<ConfigurationRecord> filt = builder.Eq(p => p.GuId, guid);
            IFindFluent<ConfigurationRecord, ConfigurationRecord> result = _collection.Find<ConfigurationRecord>(filt);
            if (result != null)
            {
                entity = result.FirstOrDefault();
            }
            entity.IsActive = false;
            _collection.ReplaceOne(
      item => item.GuId == entity.GuId,
      entity,
      new UpdateOptions { IsUpsert = false });



        }

        public void Update(ConfigurationRecord entity)
        {
            var builder = Builders<ConfigurationRecord>.Filter;
            FilterDefinition<ConfigurationRecord> filt = builder.Eq(p => p.GuId, entity.GuId) &
                                                         builder.Eq(p => p.ApplicationName, entity.ApplicationName);
            _collection.ReplaceOne(
      item => item.GuId == entity.GuId,
      entity,
      new UpdateOptions { IsUpsert = false });
        }


    }
}
