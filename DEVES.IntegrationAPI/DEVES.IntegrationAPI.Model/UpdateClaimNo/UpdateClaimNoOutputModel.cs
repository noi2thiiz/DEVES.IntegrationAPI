using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.UpdateClaimNo
{
    class UpdateClaimNoOutputModel
    {
    }

    public class UpdateClaimNoOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public UpdateClaimNoDataOutputModel_Pass data { get; set; }
    }

    public class UpdateClaimNoDataOutputModel_Pass
    {
        public string message { get; set; }
    }

    public class UpdateClaimNoOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public UpdateClaimNoDataOutputModel_Fail data { get; set; }
    }

    public class UpdateClaimNoDataOutputModel_Fail
    {
        public List<UpdateClaimNoFieldErrors> fieldErrors { get; set; }
    }

    public class UpdateClaimNoFieldErrors
    {
        public string name { get; set; }
        public string message { get; set; }

        public UpdateClaimNoFieldErrors(string n, string m)
        {
            name = n;
            message = m;
        }
    }

}
