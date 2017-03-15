using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Templates
{


    // extend abstract Metadata class
    public class CommandParameter
    {
        public string Name { get; private set; }
        public Object Value { get; private set; }
        public CommandParameter(string name, Object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}