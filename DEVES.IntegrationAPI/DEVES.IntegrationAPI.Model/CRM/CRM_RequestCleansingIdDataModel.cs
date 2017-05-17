using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CRM
{
    public class CRMRequestCleansingIdDataInputModel:BaseDataModel
    {
        public string titlePersonalid { get; set; } = "";
        public string stringpersonalCode { get; set; } = "";
        public string stringfirstname { get; set; } = "";
        public string lastname { get; set; } = "";
        public string gendercode{ get; set; } = "";
        public string citizenId { get; set; } = "";
        public string telephone1{ get; set; } = "";
        public string telephone2 { get; set; } = "";
        public string telephone3{ get; set; } = "";
        public string fax { get; set; } = "";
        public string mobilePhone1{ get; set; } = "";
        public string emailaddress1 { get; set; } = "";
    }
}
