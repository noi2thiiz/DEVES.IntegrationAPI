using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegLead
{
    public class RegLeadInputModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ContactInfoModel contactInfo { get; set; }
        public CompanyInfoModel companyInfo { get; set; }
    }

    public class GeneralHeaderModel
    {
        public string requester { get; set; }
        public string topic { get; set; }
    }

    public class ContactInfoModel
    {
        public string salutation { get; set; }
        public string sex { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string mobilePhone { get; set; }
        public string businessPhone { get; set; }
        public string fax { get; set; }
        public string description { get; set; }
        public string jobTitle { get; set; }
        public string preferredMethodOfContact { get; set; }
        public string doNotAllowEmails { get; set; }
        public string doNotAllowBulkEmails { get; set; }
        public string doNotAllowPhone { get; set; }
        public string doNotAllowMails { get; set; }
    }

    public class CompanyInfoModel
    {
        public string companyName { get; set; }
        public string websiteUrl { get; set; }
        public string address { get; set; }
    }
}
