using ConfigurationManagement.Business.Contract;
using ConfigurationManagement.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using ConfigurationManagement.Business.Helper;
using MongoDB.Bson;

namespace ConfigurationManagement.Business.Manager
{
    public class ConfigurationReader : IConfigurationReader
    {
        private string _applicationName;
        private string _connectionString;
        private int _refreshTimerIntervalInMs;
        readonly ConfigurationStorageFactory _configurationStorageFactory;
        private CacheManagerFactory _cacheManagerFactory;
        private IConfigurationStorage _storage;
        private const string cacheProvider = "Redis";

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            _applicationName = applicationName;
            _connectionString = connectionString;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _configurationStorageFactory = new ConfigurationStorageFactory();
            _cacheManagerFactory = new CacheManagerFactory();
            _storage = _configurationStorageFactory.CreateStorage(_connectionString, "Mongo");
        }

        /// <summary>
        /// Unit Test Amaçlı
        /// </summary>
        /// <param name="configurationStorageFactory"></param>
        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs, ConfigurationStorageFactory configurationStorageFactory, CacheManagerFactory cacheManagerFactory, IConfigurationStorage storage)
        {
            _applicationName = applicationName;
            _connectionString = connectionString;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _configurationStorageFactory = configurationStorageFactory;
            _cacheManagerFactory = cacheManagerFactory;
            _storage = storage;
        }

        public T GetValue<T>(string key)
        {
            ICacheManager cacheManager = _cacheManagerFactory.GetCacheProvider(_refreshTimerIntervalInMs, cacheProvider);
            IEnumerable<ConfigurationRecord> cacheRecordDto = cacheManager.Get<IEnumerable<ConfigurationRecord>>(_applicationName);
            var converter = new ConfigurationConverter<T>();
            if (cacheRecordDto != null && cacheRecordDto.ToList() != null && cacheRecordDto.ToList().Count > 0)
            {
                var cacheRecord = cacheRecordDto.Where(p => p.Name == key).FirstOrDefault();            
                bool typeMatch = TypeHelper.HasTypeMatched<T>(cacheRecord.Type);
                if (typeMatch)
                {
                    return converter.GetValue(cacheRecord.Value);
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                
                ConfigurationRecord record = _storage.GetWithKey(key, _applicationName);
                if (record != null)
                {
                    bool typeMatch = TypeHelper.HasTypeMatched<T>(record.Type);

                    if (typeMatch)
                    {
                        AddToCache(cacheManager);
                        return converter.GetValue(record.Value);
                    }
                    else
                    {
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }
            }
        }

        public void Write(RecordDto recorDto)
        {
            if (!string.IsNullOrEmpty(recorDto.ApplicationName) &&
                !string.IsNullOrEmpty(recorDto.Name) &&
                 !string.IsNullOrEmpty(recorDto.Value) &&
                !string.IsNullOrEmpty(recorDto.Type))
            {
                ConfigurationRecord record = new ConfigurationRecord()
                {
                    ApplicationName = _applicationName,
                    IsActive = true,
                    Name = recorDto.Name,
                    Type = recorDto.Type,
                    Value = recorDto.Value
                };
                 
                _storage.Add(record);

                ICacheManager cacheManager = _cacheManagerFactory.GetCacheProvider(_refreshTimerIntervalInMs, cacheProvider);
                cacheManager.Remove(_applicationName);
            }
        }

        public List<RecordDto> GetAll()
        {
            List<RecordDto> result = new List<RecordDto>();
            IEnumerable<ConfigurationRecord> cacheRecordDto = GetCachedRecords();
            if (cacheRecordDto != null)
            {
                result = cacheRecordDto.Select(p => new RecordDto()
                {
                    ApplicationName = p.ApplicationName,
                    Guid = p.GuId,
                    Name = p.Name,
                    Value = p.Value,
                    Type = p.Type
                }
                ).ToList();
            }
            else
            {                 
                var records = _storage.Get(_applicationName).ToList();
                result = records.Select(p => new RecordDto()
                {
                    Name = p.Name,
                    Type = p.Type,
                    Value = p.Value,
                    Guid = p.GuId,
                    ApplicationName = p.ApplicationName
                }
                ).ToList();
            }
            return result;

        }

       

        public void Update(RecordDto recorDto)
        {
            ConfigurationRecord record = new ConfigurationRecord()
            {
                ApplicationName = _applicationName,
                IsActive = true,
                Name = recorDto.Name,
                Type = recorDto.Type,
                Value = recorDto.Value,
                GuId = recorDto.Guid
            };            
            _storage.Update(record);
            ICacheManager cacheManager = _cacheManagerFactory.GetCacheProvider(_refreshTimerIntervalInMs, cacheProvider);
            cacheManager.Remove(_applicationName);
        }

        public RecordDto GetValueWithId(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                IEnumerable<ConfigurationRecord> cacheRecordDto = GetCachedRecords();
                if (cacheRecordDto != null && cacheRecordDto.ToList() != null & cacheRecordDto.ToList().Count > 0)
                {
                    return cacheRecordDto.Where(q => q.GuId.ToString() == guid).Select(p => new RecordDto()
                    {
                        ApplicationName = p.ApplicationName,
                        Guid = p.GuId,
                        Name = p.Name,
                        Value = p.Value,
                        Type = p.Type
                    }).FirstOrDefault();
                }
                else
                {                    
                    ConfigurationRecord record = _storage.Get(guid, _applicationName);
                    if (record != null)
                    {
                        return new RecordDto()
                        {
                            ApplicationName = _applicationName,

                            Name = record.Name,
                            Type = record.Type,
                            Value = record.Value,
                            Guid = record.GuId
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public void Remove(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {               
                _storage.Remove(guid);
                ICacheManager cacheManager = _cacheManagerFactory.GetCacheProvider(_refreshTimerIntervalInMs, cacheProvider);
                cacheManager.Remove(_applicationName);
            }
        }

        private void AddToCache(ICacheManager cacheManager)
        {
            var list = _storage.Get(_applicationName);
            cacheManager.Add(_applicationName, list, _refreshTimerIntervalInMs);
        }
        private IEnumerable<ConfigurationRecord> GetCachedRecords()
        {
            ICacheManager cacheManager = _cacheManagerFactory.GetCacheProvider(_refreshTimerIntervalInMs, cacheProvider);
            IEnumerable<ConfigurationRecord> cacheRecordDto = cacheManager.Get<IEnumerable<ConfigurationRecord>>(_applicationName);
            return cacheRecordDto;
        }
    }
}
