using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimInfo
{
    public class UpdateClaimInfoOutputModel_Fail
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }

        public UpdateClaimInfoDataOutputModel_Fail data { get; set; }
    }

    public class UpdateClaimInfoDataOutputModel_Fail
    {
        public UpdateClaimInfoFieldErrorOutputModel_Fail fieldError { get; set; }
    }

    public class UpdateClaimInfoFieldErrorOutputModel_Fail
    {
        public string name { get; set; }
        public string message { get; set; }
    }
}
