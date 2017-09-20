using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CreateCRMPersonalClientMaster
{
    class CreateCRMPersonalClientMasterOutputModel
    {
    }

    public class CreateCRMPersonalClientMasterOutputModel_Pass
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime? transactionDateTime { get; set; }
        public CreateCRMPersonalClientMasterDataOutputModel_Pass data { get; set; }
    }

    public class CreateCRMPersonalClientMasterDataOutputModel_Pass
    {
        public string cleasingId { get; set; }
        public string clientId { get; set; }
        public string sapId { get; set; }
        public string salutationText { get; set; }
        public string personalName { get; set; }
        public string personalSurname { get; set; }

    }

    public class CreateCRMPersonalClientMasterOutputModel_Fail
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime? transactionDateTime { get; set; }
        public CreateCRMPersonalClientMasterDataOutputModel_Fail data { get; set; }
    }

    public class CreateCRMPersonalClientMasterDataOutputModel_Fail
    {
        public string fieldError { get; set; }
        public string name { get; set; }
        public string message { get; set; }
    }

}
