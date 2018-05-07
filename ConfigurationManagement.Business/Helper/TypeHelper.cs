using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManagement.Business.Helper
{
    public static class TypeHelper
    {
        public static bool HasTypeMatched<T>(string cacheType)
        {
            Type type = Type.GetType("System." + cacheType);
            bool typeMatch = typeof(T) == type;
            return typeMatch;

        }
    }
}
