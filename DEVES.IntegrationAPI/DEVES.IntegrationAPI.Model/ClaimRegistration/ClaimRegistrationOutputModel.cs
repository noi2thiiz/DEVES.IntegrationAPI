using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.ClaimRegistration
{
    public class ClaimRegistrationOutputModel: BaseDataModel
    {
        [CrmMapping(FieldName = "claimId", Source = ENUMDataSource.srcEWI)]
        public string claimID { get; set; }

        [CrmMapping( FieldName = "claimNo" , Source = ENUMDataSource.srcEWI )]
        public string claimNo { get; set; }
    }

    public class LocusClaimRegistrationDataOutputModel: BaseDataModel
    {
        private EWI.EWIResponseContentData _data;

        public LocusClaimRegistrationDataOutputModel(EWI.EWIResponseContentData data)
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