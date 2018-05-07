using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationManagement.Models
{
    public class ConfigurationModel
    {
        [BsonId]
        public object Guid { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }


        public string ApplicationName { get; set; }
    }
}
