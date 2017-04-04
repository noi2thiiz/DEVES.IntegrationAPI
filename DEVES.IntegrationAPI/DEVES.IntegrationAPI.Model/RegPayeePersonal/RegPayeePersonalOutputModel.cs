using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegPayeePersonal
{
    public class RegPayeePersonalContentOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<RegPayeePersonalDataOutputModel> data { set; get; }
    }


    public abstract class RegPayeePersonalDataOutputModel : BaseDataModel
    {
    }


    public class RegPayeePersonalOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegPayeePersonalDataOutputModel_Pass> data { get; set; }
    }

    public class RegPayeePersonalDataOutputModel_Pass: RegPayeePersonalDataOutputModel
    {
        public string polisyClientId { get; set; }
        public string sapVendorCode { get; set; }
        public string sapVendorGroupCode { get; set; }
        public string personalName { get; set; }
        public string personalSurname { get; set; }

        public RegPayeePersonalDataOutputModel_Pass()
        {
            polisyClientId = "";
            sapVendorCode = "";
            sapVendorGroupCode = "";
            personalName = "";
            personalSurname = "";
        }
    }

    public class RegPayeePersonalOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public List<RegPayeePersonalDataOutputModel_Fail> data { get; set; }
    }

    public class RegPayeePersonalDataOutputModel_Fail: RegPayeePersonalDataOutputModel
    {
        public string fieldErrors { get; set; }
        public string message { get; set; }
        public string name { get; set; }
    }

}