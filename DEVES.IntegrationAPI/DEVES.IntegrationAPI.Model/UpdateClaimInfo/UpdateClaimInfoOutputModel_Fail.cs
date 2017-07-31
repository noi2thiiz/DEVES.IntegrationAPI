using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimInfo
{
    public class UpdateClaimInfoOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }

        public UpdateClaimInfoDataOutputModel_Fail data { get; set; }
        public List<string> errorMessage { get; set; }
    }

    public class UpdateClaimInfoDataOutputModel_Fail
    {
        public List<UpdateClaimInfoFieldErrorOutputModel_Fail> fieldError { get; set; }
    }

    public class UpdateClaimInfoFieldErrorOutputModel_Fail
    {
        public string name { get; set; }
        public string message { get; set; }

        public UpdateClaimInfoFieldErrorOutputModel_Fail(string n, string m)
        {
            name = n;
            message = m;
        }
    }
}
