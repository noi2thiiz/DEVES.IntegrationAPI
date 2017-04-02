using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.Model
{
    public class CreateCrmPersonInfoOutputModel : BaseContentJsonProxyOutputModel
    {
        public string crmClientId { set; get; }
    }

    public class CreateCrmCorporateInfoOutputModel : BaseContentJsonProxyOutputModel
    {
        public string crmClientId { set; get; }
    }
}
