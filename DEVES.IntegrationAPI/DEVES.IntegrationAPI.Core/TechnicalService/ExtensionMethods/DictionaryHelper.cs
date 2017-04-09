using System;
using System.Collections.Generic;
using System.Reflection;

namespace DEVES.IntegrationAPI.Core.ExtensionMethods
{
    public static class DictionaryHelper
    {
        public static T ToType<T>(this Dictionary<string,object> obj)
            where T: new()
        {
            Type type = typeof(T);
            var o = new T();
            object traget = Activator.CreateInstance(type);
            //MethodInfo method = type.GetMethod(methodName);
            // var outputContentInstance = (T_OUTPUT)method.Invoke(instance, methodArgs);

            var tragetTypeList = new Dictionary<string, string>();
            foreach (PropertyInfo pi in traget.GetType().GetProperties())
            {
                tragetTypeList.Add(pi.Name.ToString(), pi.PropertyType.ToString());
            }

            foreach (var item in obj)
            {
                var itemName = item.Key;
                var itemType = item.Value.GetType().ToString();
                var tragetType = tragetTypeList[itemName];
                try
                {
                  
                    if (itemType== "System.String" && tragetType== "System.Guid")
                    {
                        type.GetProperty(item.Key).SetValue(o, new Guid(item.Value.ToString()), null);
                    }
                    else
                    {
                        type.GetProperty(item.Key).SetValue(o, item.Value, null);
                    }
                    
                }
                catch(Exception)
                {
                   
              

                    Console.WriteLine("Cannot Convert "+ itemName + "type: "+itemType  + " to: " + tragetType);
                }
                /*
             Cannot Convert CreatedOntype: System.DateTime to: System.String
            Cannot Convert ModifiedOntype: System.DateTime to: System.String
            Cannot Convert VersionNumbertype: System.Byte[] to: System.String
            Cannot Convert ExchangeRatetype: System.Decimal to: System.Int32
            Cannot Convert pfc_date_of_birthtype: System.DateTime to: System.String
            Cannot Convert pfc_client_start_datetype: System.DateTime to: System.String
            Cannot Convert pfc_client_date_timetype: System.DateTime to: System.String
            Cannot Convert pfc_conv_datetype: System.DateTime to: System.String    

            Cannot Convert crmPolicyDetailIdtype: System.String to: System.Guid
            Cannot Convert policyIssueDatetype: System.DateTime to: System.String
            Cannot Convert policyEffectiveDatetype: System.DateTime to: System.String
            Cannot Convert policyExpiryDatetype: System.DateTime to: System.String
            Cannot Convert policyDeducttype: System.Decimal to: System.Int32
             */

            }

            return o;
        }
    }
}
