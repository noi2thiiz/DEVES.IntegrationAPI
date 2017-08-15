using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.EWI;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.ClaimRegistration
{

   
    public class ClaimRegistrationContentOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<ClaimRegistrationDataOutputModel> data { set; get; }
    }
    public class ClaimRegistrationDataOutputModel : BaseDataModel
    {
        [CrmMapping(FieldName = "claimId", Source = ENUMDataSource.srcEWI)]
        public string claimID { get; set; }

        [CrmMapping( FieldName = "claimNo" , Source = ENUMDataSource.srcEWI )]
        public string claimNo { get; set; }

        public string errorMessage { get; set; }
    }


    public class LocusClaimRegistrationOutputModel : BaseEWIResponseModel
    {
        [JsonProperty(Order = 1)]
        public LocusClaimRegistrationContentOutputModel content { set; get; }
    }

    public class LocusClaimRegistrationContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 2)]
        public LocusClaimRegistrationDataOutputModel data { set; get; }
    }

    public class LocusClaimRegistrationDataOutputModel: BaseDataModel
    {
        private LocusClaimRegistrationDataOutputModel _data;

    
        public LocusClaimRegistrationDataOutputModel(LocusClaimRegistrationDataOutputModel data)
        {
            _data = data;
            this.claimId = data.claimId;
            this.claimNo = data.claimNo;
            this.ticketNumber = data.ticketNumber;
        }

        public string claimId { get; set; }
        public string claimNo { get; set; }
        public string ticketNumber { get; set; }
    }

    /*
    public class LocusClaimRegistrationOutputModel
    {
         public int code { get; set; }
         public string message { get; set; }
         public string description { get; set; }
         public string transactionId { get; set; }
         public DateTime transactionDateTime { get; set; }
         public LocusClaimRegistrationDataOutputModel data { get; set; }
    }
    */

}