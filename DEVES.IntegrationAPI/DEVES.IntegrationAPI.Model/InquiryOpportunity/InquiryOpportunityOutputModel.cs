using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.InquiryOpportunity
{
    public class InquiryOpportunityOutputModel : BaseContentJsonProxyOutputModel
    {
        public List<InquiryOpportunityDataModel> data { get; set; }
    }

    public class InquiryOpportunityDataModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public ContactInfoModel contactInfo { get; set; }
        public OpportunityInfoModel opportunityInfo { get; set; }
        public CompanyInfoModel companyInfo { get; set; }
        public ProductionInfoModel productionInfo { get; set; }
    }

    public class GeneralHeaderModel
    {
        public string topic { get; set; }
        public string opportunityId { get; set; }
    }

    public class ContactInfoModel
    {
        public string salutationCode { get; set; }
        public string salutationText { get; set; }
        public string sexCode { get; set; }
        public string sexText { get; set; }
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

    public class OpportunityInfoModel
    {
        public int budgetAmount { get; set; }
        public string purchaseProcess { get; set; }
        public string purchasetimeframe { get; set; }
        public string currentSituation { get; set; }
        public string customerNeed { get; set; }
        public string proposedSolution { get; set; }
        public string salesStage { get; set; }

    }

    public class CompanyInfoModel
    {
        public string companyName { get; set; }
        public string websiteUrl { get; set; }
        public string address { get; set; }
    }

    public class ProductionInfoModel
    {
        public string productGroup { get; set; }
        public List<ProductLineModel> productLine { get; set; }
    }

    public class ProductLineModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public int price { get; set; }
    }
}
