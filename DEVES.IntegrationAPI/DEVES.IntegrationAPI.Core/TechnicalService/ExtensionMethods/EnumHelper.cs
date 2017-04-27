using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.Attributes;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.Core.TechnicalService.ExtensionMethods
{
    public static class EnumHelper
    {
        public static string GetValue(this Enum obj)
           
        {
            Type type = obj.GetType();
           
            var memInfo = type.GetMember(obj.ToString());

            var attribute = memInfo[0].GetCustomAttribute<EnumMemberAttribute>();
            if (null != attribute)
            {
                return attribute.Value;
            }
            return ((int)(Enum.Parse(type, obj.ToString()))).ToString();
         

        }

        public static int ToInt(this Enum obj)

        {
            Type type = obj.GetType();

            var memInfo = type.GetMember(obj.ToString());

            var attribute = memInfo[0].GetCustomAttribute<EnumMemberAttribute>();
            if (null == attribute) return (int) Enum.Parse(type, obj.ToString());
            if (Int32.TryParse(attribute.Value, out int nOut))
            {
                return nOut;
            }
            return (int) Enum.Parse(type, obj.ToString() );


        }

        public static OptionSetValue ToOptionSetValue(this Enum obj)

        {
            Type type = obj.GetType();

            var memInfo = type.GetMember(obj.ToString());

            var attribute = memInfo[0].GetCustomAttribute<XrmOptionSetValueAttribute>();

           // (float)Enum.Parse(type, obj.ToString()
           return attribute != null ? new OptionSetValue(attribute.Value) : new OptionSetValue();
        }
    }
    
}
