using System.Collections.Generic;

namespace ConfigurationManagement.Business.Contract
{
    public  interface IConfigurationReader
    {
        T GetValue<T>(string key);
        void Write(RecordDto recordDto);
        List<RecordDto> GetAll();
        void Update(RecordDto recorDto);
        RecordDto GetValueWithId(string guid);
        void Remove(string guid);
    }
}
