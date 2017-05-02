using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.Helper
{
    public static class ObjectHelper
    {
        public static string ToStringOrEmpty(this object value)
        {
            var output = "";
            if (value != null)
            {
                output = value.ToString();
            }
            return output;
        }
    }
}