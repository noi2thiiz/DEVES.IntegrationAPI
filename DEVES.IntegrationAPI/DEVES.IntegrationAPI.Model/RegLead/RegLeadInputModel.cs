using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ProductInfoModel productInfo { get; set; }
        public InsuredInfoModel insuredInfo { get; set; }
        public VehicleInfoModel vehicleInfo { get; set; }
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
        public string line { get; set; }
        public string facebookId { get; set; }
    }

    public class CompanyInfoModel
    {
        public string companyName { get; set; }
        public string websiteUrl { get; set; }
        public string address { get; set; }
    }

    public class ProductInfoModel
    {
        public string groupCode { get; set; }
        public string groupName { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string categoryCode { get; set; }
        public string categoryName { get; set; }
        public int budgetAmount { get; set; }
    }

    public class InsuredInfoModel : BaseDataModel
    {
        public string insuredFullName { get; set; }
        public string insuredMobilePhone { get; set; }
        /*
        public string insuredIssueDate
        {
            get
            {
                string s = "";
                if (dtinsuredIssueDate != null)
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    s = dtinsuredIssueDate.Value.ToString(DateTimeCustomFormat, enUS);

                }
                return s;
            }
        }
        */
        public DateTime? insuredIssueDate { get; set; }
    }

    public class VehicleInfoModel
    {
        public string brandCode { get; set; }
        public string brandName { get; set; }
        public string modelCode { get; set; }
        public string modelName { get; set; }
        public int year { get; set; }
    }

}
