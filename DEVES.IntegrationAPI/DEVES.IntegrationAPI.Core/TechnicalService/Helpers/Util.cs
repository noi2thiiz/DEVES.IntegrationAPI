using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace DEVES.IntegrationAPI.WebApi.Core
{
    public class Util
    {

        public static string IfStringNotEmpty(string source,string defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                {
                    return defaultValue;
                }
                return source;
            }
            catch (NoNullAllowedException e)
            {
                return source;
            }

        }
        public static T Extend<T>(T target, T source)
        {
            Type t = typeof(T);


            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);
            foreach

            (var prop in
                properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }

            return target;
        }

        /// <summary>
        /// Create copy of class object without any reference
        /// </summary>
        /// <param name="other"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Clone<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }


}