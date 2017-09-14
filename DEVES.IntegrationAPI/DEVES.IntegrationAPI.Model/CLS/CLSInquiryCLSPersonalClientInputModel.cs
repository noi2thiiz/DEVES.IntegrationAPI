using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CLS
{

    public class CLSInquiryPersonalClientInputModel : BaseEWIRequestContentModel
    {
        public String clientId { set; get; } = "";
        public string roleCode { set; get; } = "";
        public string cleansingId { set; get; } = "";
        public String personalFullName { set; get; } = "";
        public String idCitizen { set; get; } = "";
        public String telephone { set; get; } = "";
        public String emailAddress { set; get; } = "";

        public string backDay { set; get; } = "";
    }
}
