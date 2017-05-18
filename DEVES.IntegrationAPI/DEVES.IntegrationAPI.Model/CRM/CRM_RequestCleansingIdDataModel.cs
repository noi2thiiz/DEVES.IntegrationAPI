using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CRM
{
    public class CRMRequestCleansingIdDataInputModel : BaseDataModel
     
    {
        public string roleCode { get; set; } = "";
        public string clientType { get; set; } = "";
        public string guid { get; set; } = "";
        public string refCode { get; set; } = "";
        public CRMRequestPersonalCleansingIdDataInputModel profileInfo { get; set; }
        public CRMRequestCorporateCleansingIdDataInputModel profileHeader { get; set; }
    }
    public class CRMRequestPersonalCleansingIdDataInputModel:BaseDataModel
    {
 
        public string personalCode { get; set; } = ""; // 0000 (Master Data salutation)
        public string titlePersonalid { get; set; } = "";
        public string titlePersonalname { get; set; } = "";
        public string titlePersonaltype { get; set; } = "";
       
        public string firstname { get; set; } = "";
        public string lastname { get; set; } = "";
      
        public string gendercode{ get; set; } = "100000000";
        public string citizenId { get; set; } = "";
        public string telephone1{ get; set; } = "";
        public string telephone2 { get; set; } = "";
        public string telephone3{ get; set; } = "";
        public string fax { get; set; } = "";
        public string mobilePhone1{ get; set; } = "";
        public string emailaddress1 { get; set; } = "";
        
    }

    public class CRMRequestCorporateCleansingIdDataInputModel : BaseDataModel
    {
     
      
        public string accountName { get; set; } = ""; 
        public string name1 { get; set; } = "";
        public string name2 { get; set; } = "";
        public string taxno { get; set; } = "";
    
        public string taxbranch { get; set; } = "";
        public string lastname { get; set; } = "";
 
        public string telephone1 { get; set; } = "";
        public string telephone2 { get; set; } = "";
        public string telephone3 { get; set; } = "";
        public string fax { get; set; } = "";
        public string mobilePhone1 { get; set; } = "";

        public string emailaddress1 { get; set; } = "";

    }
}
