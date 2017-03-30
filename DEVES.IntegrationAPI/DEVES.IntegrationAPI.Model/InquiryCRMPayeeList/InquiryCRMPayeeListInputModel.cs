using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryCRMPayeeList
{
    public class InquiryCRMPayeeListInputModel : BaseDataModel
    {
        public string clientType { get; set; }
        public string roleCode { get; set; }
        public string polisyClientId { get; set; }
        public string sapVendorCode { get; set; }
        public string fullname { get; set; }
        public string taxNo { get; set; }
        public string taxBranchCode { get; set; }
        public string requester { get; set; }
        public string emcsCode { get; set; }
    }
}