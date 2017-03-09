using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model
{
    public enum ENUMDataSource
    {
        undefined,
        srcCrm,
        srcSQL,
        srcEWI        
    }

    [System.AttributeUsage(System.AttributeTargets.Field |
                           System.AttributeTargets.Property,
                           AllowMultiple = true)  
    ]
    public class CrmMappingAttribute : System.Attribute
    {
        public ENUMDataSource Source = ENUMDataSource.undefined;
        public string FieldName=null;
    }

}
