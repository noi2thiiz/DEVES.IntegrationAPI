using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegLead;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzRegLead : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Preparation Variable
            RegLeadOutputModel output = new RegLeadOutputModel();

            // Deserialize Input
            RegLeadInputModel contentModel = (RegLeadInputModel)input;

            // Connect SDK
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            try
            {
                Lead lead = new Lead();

                // Waiting for field to create

                // generalHeader
                switch(contentModel.generalHeader.requester.ToLower())
                {
                    case "advertisement" : lead.LeadSourceCode = new OptionSetValue(1); break;
                    case "employee referral": lead.LeadSourceCode = new OptionSetValue(2); break;
                    case "external referral": lead.LeadSourceCode = new OptionSetValue(3); break;
                    case "partner": lead.LeadSourceCode = new OptionSetValue(4); break;
                    case "public relations": lead.LeadSourceCode = new OptionSetValue(5); break;
                    case "seminar": lead.LeadSourceCode = new OptionSetValue(6); break;
                    case "trade show": lead.LeadSourceCode = new OptionSetValue(7); break;
                    case "web": lead.LeadSourceCode = new OptionSetValue(8); break;
                    case "word of mouth": lead.LeadSourceCode = new OptionSetValue(9); break;
                    default: lead.LeadSourceCode = new OptionSetValue(10); break;
                }
                // lead.LeadSourceCode = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.generalHeader.requester));
                lead.Subject = contentModel.generalHeader.topic;

                // contactInfo
                lead.Salutation = contentModel.contactInfo.salutation;
                switch (contentModel.contactInfo.sex)
                {
                    case "M": lead.pfc_sex = new OptionSetValue(1); break;
                    case "F": lead.pfc_sex = new OptionSetValue(2); break;
                    default: lead.pfc_sex = new OptionSetValue(100000000); break;
                }
                // lead.pfc_sex = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.contactInfo.sex));
                lead.FirstName = contentModel.contactInfo.firstName;
                lead.LastName = contentModel.contactInfo.lastName;
                lead.EMailAddress1 = contentModel.contactInfo.email;
                lead.MobilePhone = contentModel.contactInfo.mobilePhone;
                lead.Telephone1 = contentModel.contactInfo.businessPhone;
                lead.Fax = contentModel.contactInfo.fax;
                lead.Description = contentModel.contactInfo.description;
                lead.JobTitle = contentModel.contactInfo.jobTitle;
                switch (contentModel.contactInfo.preferredMethodOfContact)
                {
                    case "Email": lead.PreferredContactMethodCode = new OptionSetValue(2); break;
                    case "Phone": lead.PreferredContactMethodCode = new OptionSetValue(3); break;
                    case "Fax": lead.PreferredContactMethodCode = new OptionSetValue(4); break;
                    case "Mail": lead.PreferredContactMethodCode = new OptionSetValue(5); break;
                    default: lead.PreferredContactMethodCode = new OptionSetValue(1); break;
                }
                // lead.PreferredContactMethodCode = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.contactInfo.preferredMethodOfContact));
                lead.pfc_line = contentModel.contactInfo.line;
                lead.pfc_facebook = contentModel.contactInfo.facebookId;

                // companyInfo
                lead.CompanyName = contentModel.companyInfo.companyName;
                lead.WebSiteUrl = contentModel.companyInfo.websiteUrl;
                // lead.Address1_Composite = contentModel.companyInfo.address;

                // productInfo
                lead.pfc_product_group_code = contentModel.productInfo.groupCode;
                lead.pfc_product_group_name = contentModel.productInfo.groupName;
                lead.pfc_product_code = contentModel.productInfo.productCode;
                lead.pfc_product_name = contentModel.productInfo.productName;
                lead.pfc_product_category_code = contentModel.productInfo.categoryCode;
                lead.pfc_product_category_name = contentModel.productInfo.categoryName;
                lead.BudgetAmount = new Money(contentModel.productInfo.budgetAmount);

                // insuredInfo
                lead.pfc_insured_fullname = contentModel.insuredInfo.insuredFullName;
                lead.pfc_insured_mobile = contentModel.insuredInfo.insuredMobilePhone;
                // string dateString = contentModel.insuredInfo.insuredIssueDate;
                lead.pfc_insured_issue_date = contentModel.insuredInfo.insuredIssueDate;

                // vehicleInfo
                lead.pfc_brand_code = contentModel.vehicleInfo.brandCode;
                lead.pfc_brand_name = contentModel.vehicleInfo.brandName;
                lead.pfc_model_code = contentModel.vehicleInfo.modelCode;
                lead.pfc_model_name = contentModel.vehicleInfo.modelName;
                lead.pfc_vechicle_year = contentModel.vehicleInfo.year;

                _serviceProxy.Create(lead);

                output.code = AppConst.CODE_SUCCESS;
                output.message = AppConst.MESSAGE_SUCCESS;
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;

                return output;
            }
            catch(Exception)
            {
                output.code = AppConst.CODE_FAILED;
                output.message = "ไม่สามารถสร้าง Lead ได้";
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;

                return output;
            }

        }
    }
}