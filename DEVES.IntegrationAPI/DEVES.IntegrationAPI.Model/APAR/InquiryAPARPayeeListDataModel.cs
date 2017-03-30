using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.APAR
{
    public class InquiryAPARPayeeListInputModel : BaseDataModel
    {
        public string vendorCode { get; set; }
        public string taxBranchCode { get; set; }
        public string requester { get; set; }
        public string polisyClntnum { get; set; }
        public string fullName { get; set; }

    
    }
   

    public class InquiryAPARPayeeModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public InquiryAPARPayeeContentOutputModel content { set; get; }
    }

   

    public class InquiryAPARPayeeContentOutputModel : BaseContentJsonServiceOutputModel
    {
        [JsonProperty(Order = 21)]
        public InquiryAPARPayeeContentAparPayeeListCollectionModel aparPayeeListCollection { set; get; }
    }

    public class InquiryAPARPayeeContentAparPayeeListCollectionModel
    {
        public List<InquiryAPARPayeeListOutputModel> aparPayeeList { set; get; }
    }
    public class InquiryAPARPayeeListOutputModel : BaseDataModel
    {

        public string telephone1 { get; set; }
        public string taxBranchCode { get; set; }
        public string taxNo { get; set; }
        public string vendorCode { get; set; }
        public string vendorGroupCode { get; set; }
        public string telephone2 { get; set; }
        public string oldTaxNo1 { get; set; }
        public string oldTaxNo2 { get; set; }
        public string address { get; set; }
        public string polisyClntnum { get; set; }
        public string faxNo { get; set; }
        public string fullName { get; set; }

    }
}
