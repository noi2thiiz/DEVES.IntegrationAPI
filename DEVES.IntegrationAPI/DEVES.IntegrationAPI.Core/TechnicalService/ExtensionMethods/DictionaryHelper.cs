using System;
using System.Collections.Generic;

namespace DEVES.IntegrationAPI.Core.ExtensionMethods
{
    public static class DictionaryHelper
    {
        public static T ToType<T>(this Dictionary<string,object> obj)
            where T: new()
        {
            Type type = typeof(T);
            var o = new T();
            object instance = Activator.CreateInstance(type);
            //MethodInfo method = type.GetMethod(methodName);
            // var outputContentInstance = (T_OUTPUT)method.Invoke(instance, methodArgs);

            foreach (var item in obj)
            {
               
                type.GetProperty(item.Key).SetValue(o, item.Value, null);
            }

            return o;
        }
    }
}
