using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManagement.Business
{
    [Serializable]
   public class RecordDto
    {
        public string Type { get; set; }
        public string Name{ get; set; }
        public string Value { get; set; }
        [BsonId]
        public object Guid { get; set; }
        public string ApplicationName { get; set; }
    }
}
