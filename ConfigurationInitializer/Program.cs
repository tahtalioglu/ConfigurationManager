using ConfigurationManagement.Business;
using ConfigurationManagement.Business.Contract;
using ConfigurationManagement.Business.Manager;
using MongoDB.Bson;
using System;

namespace ConfigurationInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationReader reader = new ConfigurationReader("console", "mongodb://localhost:27017", 20);
            // Read(reader);
            //  Write(reader);
            //ReadList(reader);
            Update(reader);
            //  ReadWithId(reader, "1a1ce04d-7d3f-4831-ad47-7b30d01f6d23");
        }

        private static void ReadWithId(IConfigurationReader reader, string v)
        {
             
          
            Console.ReadLine();
        }

        private static void Update(IConfigurationReader reader)
        {
            //RecordDto dto = new RecordDto
            //{
            //    Name = "refresh",
            //    Type = "Int",
            //    Value = "15",
            //    Guid = new ObjectId("M41Xl74k40S1jB7//4/AUQ==") 
            //};
            reader.Remove("b1c647e7-5faf-4199-901f-5858b5be2738");
        }

        private static void Write(IConfigurationReader reader)
        {
            RecordDto dto = new RecordDto
            {
                Name = "IsActive",
                Type = "Boolean",
                Value = "true"
            };
            reader.Write(dto);
        }

        private static void Read(IConfigurationReader reader)
        {
            var value = reader.GetValue<string>("nosql");
            Console.Write(value);
            Console.ReadLine();
        }

        private static void ReadList(IConfigurationReader reader)
        {
            var value = reader.GetAll();
            for (int i = 0; i < value.Count; i++)
            {
                Console.Write(string.Format("{0},{1},{2}", value[i].Name, value[i].Type, value[i].Value));
                Console.WriteLine();
            }
            Console.ReadLine();
        }



    }
}
