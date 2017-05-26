using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CLS
{
    public class CLSInquiryCorporateClientInputModel: BaseDataModel
    {
        public string clientId { set; get; } = "";
        public string roleCode { set; get; } = "";

        public string corporateFullName { set; get; } = "";
        public string taxNo { set; get; } = "";
        public string telephone { set; get; } = "";
        public string emailAddress { set; get; } = "";

        public string backDay { set; get; } = "7";
    }
}
