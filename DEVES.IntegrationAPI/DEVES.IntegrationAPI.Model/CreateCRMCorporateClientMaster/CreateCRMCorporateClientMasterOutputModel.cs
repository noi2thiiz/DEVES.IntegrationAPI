using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CreateCRMCorporateClientMaster
{
    class CreateCRMCorporateClientMasterOutputModel
    {
    }

    public class CreateCRMCorporateClientMasterOutputModel_Pass
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public CreateCRMCorporateClientMasterDataOutputModel_Pass data { get; set; }
    }

    public class CreateCRMCorporateClientMasterDataOutputModel_Pass
    {
        public string cleasingId { get; set; }
        public string clientId { get; set; }
        public string sapId { get; set; }
        public string corporateName1 { get; set; }
        public string corporateName2 { get; set; }
        public string corporateBranch { get; set; }
    }

    public class CreateCRMCorporateClientMasterOutputModel_Fail
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public CreateCRMCorporateClientMasterDataOutputModel_Fail data { get; set; }
    }

    public class CreateCRMCorporateClientMasterDataOutputModel_Fail
    {
        public string fieldError { get; set; }
        public string name { get; set; }
        public string message { get; set; }
    }
}
