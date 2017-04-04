using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DEVES.IntegrationAPI.WebApi
{
    public class EnumValue
    {
       // public string Name { get; set; }
        public string Value { get; set; }
       // public string Value2 { get; set; }
        public string Description { get; set; }

    }



    public static class EnumExtensions
    {
        public static string GetAttributeFrom<T>( string value, string attribute)
        {
            var xml = GetXmlCommentsPathForModels();

            var attrType = typeof(T);
            var document = XDocument.Load(xml); ;
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            var name = "F:" + attrType + "." + value;
            //F:DEVES.IntegrationAPI.WebApi.Services.CorporateClientMaster.EconActivityOptions.A
            return  document.XPathSelectElement("/doc/members/member[@name='"+name+"']/"+attribute).Value.ToString().Trim();

        }
        public static string GetXmlCommentsPathForModels()
        {
            return String.Format(@"{0}\bin\DEVES.IntegrationAPI.WebApi.XML", AppDomain.CurrentDomain.BaseDirectory);
        }

        public static List<EnumValue> GetValues<T>()
        {
            List<EnumValue> values = new List<EnumValue>();
            foreach (var itemType in Enum.GetValues(typeof(T)))
            {
                //For each value of this enumeration, add a new EnumValue instance

                var enumType = typeof (T);
                var name = Enum.GetName(enumType, itemType);
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                var key =  enumMemberAttribute.Value;

                var attrType = typeof(T);
                var v = (int) itemType;
                values.Add(new EnumValue()
                {
                    Value = key.ToString() ,//Enum.GetName(typeof(T), itemType),
                    //Value2 = key.ToString(),
                   // Value = Enum.GetUnderlyingType(),
                    Description =  GetAttributeFrom<T>(itemType.ToString(),"summary")
                });
            }
            return values;
        }
    }
}