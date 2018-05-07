using Newtonsoft.Json;

namespace ConfigurationManagement.Business.Helper
{
    public static class Serializer
    {
        public static string Serialize(this object serializableObject)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.SerializeObject(serializableObject, jsonSerializerSettings);
        }

        public static T Deserialize<T>(this string serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.DeserializeObject<T>(serializedObject, jsonSerializerSettings);
        }
    }
}
