using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.CRM
{
    public class CRM_MasterCountry
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ctryPolisy { get; set; }
        public string ctrySAP { get; set; }
    }
}
