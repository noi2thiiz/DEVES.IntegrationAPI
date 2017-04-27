using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DEVES.IntegrationAPI.WebApi.Core.Attributes;
using Microsoft.Xrm.Sdk.Query;

namespace DEVES.IntegrationAPI.Core.TechnicalService.DataManager
{
    public class XrmAttributeReflector
     
    {
        public List<string> IgnoreAttributes = new List<string>{
            "logicalname",
            "id",
            "attributes",
            "entitystate",
            "formattedvalues",
            "relatedentities",
            "rowversion",
            "keyattributes",
            "item",
            "extensiondata",
            "state",
            "status"};

        public ColumnSet GetColumnSetByAttributes<TModelClass>()
        {
            Console.WriteLine("====GetColumnSetByAttributes==="+ typeof(TModelClass));
            var attributes = new ColumnSet(new string[] { });

            Dictionary<string, string> _dict = new Dictionary<string, string>();

            var props = typeof(TModelClass).GetProperties();
            foreach (var pi in props)
            {
                var attr = pi.GetCustomAttribute<XrmAttributeMappingAttribute>();

                if (null == attr) continue;
                var xrmFieldName = attr.FieldName;
                attributes.AddColumn(xrmFieldName);
                Console.WriteLine(pi.Name+"====xrmFieldName===" + xrmFieldName);
            }

            return attributes;

        }

        public ColumnSet GetColumnSet<ModelClass>()
        {
            var attributes = new ColumnSet(new string[] {});
            foreach (PropertyInfo pi in typeof(ModelClass).GetProperties())
            {
                var piName = pi.Name.ToLower();
                if (!IgnoreAttributes.Contains(piName))
                {
                    attributes.AddColumn(piName);
                }

            }

            return attributes;

        }

        public IEnumerable<PropertyInfo> GetProperties<TModelClass>()
        {
            var obj = (TModelClass)Activator.CreateInstance(typeof(TModelClass));

            var properties = typeof(TModelClass).GetProperties();
            var propertiesList = new List<PropertyInfo>();
            foreach (PropertyInfo pi in properties)
            {
                string piName = pi.Name.ToLower();
                if (null != pi && pi.CanWrite && ! IgnoreAttributes.Contains(piName))
                {
                    try
                    {
                        propertiesList.Add(pi);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "===>" + piName);
                    }
                }
            }

            return properties;
        }
    }


}