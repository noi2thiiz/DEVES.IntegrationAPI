using System;

namespace DEVES.IntegrationAPI.Model.Attributes
{
    public class XrmOptionSetValueAttribute : System.Attribute
    {
        public int Value { get; set; }

        public XrmOptionSetValueAttribute(int value)
        {
            Value = value;
        }
    }
}