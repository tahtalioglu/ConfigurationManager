using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ConfigurationManagement.Data
{
    public class Record
    {
        [BsonId]
        
        public object GuId { get; set; }
 
 
    }
}
