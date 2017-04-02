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
        public string polisyClntnum { get; set; }
        public string vendorCode { get; set; }
        public string fullName { get; set; }
        public string taxNo { get; set; }
        public string taxBranchCode { get; set; }
        public string requester { get; set; }

    
    }

    public class InquiryAPARPayeeOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public InquiryAPARPayeeContentModel content { set; get; }


    }



    public class InquiryAPARPayeeContentModel : BaseContentJsonServiceOutputModel
    {
        [JsonProperty(Order = 21)]
        public List<InquiryAPARPayeeContentAparPayeeListCollectionDataModel> aparPayeeListCollection { set; get; }
    }



    public class InquiryAPARPayeeContentAparPayeeListCollectionDataModel
    {
        public InquiryAPARPayeeListModel aparPayeeList { set; get; }
    }
    public class InquiryAPARPayeeListModel : BaseDataModel
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
