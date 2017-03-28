using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CLS
{
    public class CLSInquiryCorporateClientInputModel: BaseDataModel
    {
        public String clientId { set; get; }
        public string roleCode { set; get; }
        
        public String corporateFullName { set; get; }
        public String taxNo { set; get; }
        public String telephone { set; get; }
        public String emailAddress { set; get; }
    }
}
