using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Core.TechnicalService;

namespace DEVES.IntegrationAPI.WebApi.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XrmEntityMappingAttribute : Attribute
    {
        public string EntityName;

        public XrmEntityMappingAttribute(string entityName)
        {
            this.EntityName = entityName;

        }


    }


    public enum EntityFieldKey
    {
        NM, PK, FK,
    }

    public class XrmAttributeMappingAttribute : Attribute
    {
        public string FieldName;
        public Dictionary<Int32, string> OptionSetMapping;
        public EntityFieldKey FieldKey;


        public XrmAttributeMappingAttribute(string fieldName)
        {
            this.FieldName = fieldName;
            this.FieldKey = EntityFieldKey.NM; // PK,FK,NULL
        }

        public XrmAttributeMappingAttribute(string fieldName, EntityFieldKey fieldKey)
        {
            this.FieldName = fieldName;
            this.FieldKey = fieldKey;
        }

        public XrmAttributeMappingAttribute(string fieldName, EntityFieldKey fieldKey, Dictionary<Int32, string> optionSetMapping)
        {
            this.FieldName = fieldName;
            this.FieldKey = fieldKey;
            OptionSetMapping = optionSetMapping;

          //  var x = new Dictionary<Int32, string> {{100000000, "T"}, {100000001, "E"}};

        }
        
    }




}