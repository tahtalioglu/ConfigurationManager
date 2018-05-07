using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManagement.Business.Manager
{
   public class ConfigurationConverter<T>
    {
        public readonly Type _type;
         
       public T GetValue(string key)
        {
            return (T)Convert.ChangeType(key, typeof(T));
        }
    }
}
