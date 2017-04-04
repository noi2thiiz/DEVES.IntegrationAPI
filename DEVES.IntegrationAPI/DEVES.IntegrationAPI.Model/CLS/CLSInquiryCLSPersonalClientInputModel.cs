using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CLS
{

    public class CLSInquiryPersonalClientInputModel : BaseDataModel
    {
        public string clientId { set; get; }
        public string roleCode { set; get; }
        public string personalFullName { set; get; }
        public string idCitizen { set; get; }
        public string telephone { set; get; }
        public string emailAddress { set; get; }

        public string backDay { set; get; }
    }
}
