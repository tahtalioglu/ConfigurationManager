using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace ConfigurationManagement.Data
{
    public class ConfigurationRecord:Record
    { 
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("value")]
        public string Value { get; set; }
        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("applicationName")]
        public string ApplicationName { get; set; }
    }
}
