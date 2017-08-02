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
            output.data = new RegLeadDataModel();

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
                if (!String.IsNullOrEmpty(contentModel.contactInfo.salutation))
                {
                    lead.Salutation = contentModel.contactInfo.salutation;
                }
                switch (contentModel.contactInfo.sex)
                {
                    case "M": lead.pfc_sex = new OptionSetValue(1); break;
                    case "F": lead.pfc_sex = new OptionSetValue(2); break;
                    default: lead.pfc_sex = new OptionSetValue(100000000); break;
                }
                // lead.pfc_sex = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.contactInfo.sex));
                lead.FirstName = contentModel.contactInfo.firstName;
                lead.LastName = contentModel.contactInfo.lastName;
                if(!String.IsNullOrEmpty(contentModel.contactInfo.email))
                {
                    lead.EMailAddress1 = contentModel.contactInfo.email;
                }
                if (!String.IsNullOrEmpty(contentModel.contactInfo.mobilePhone))
                {
                    lead.MobilePhone = contentModel.contactInfo.mobilePhone;
                }
                if (!String.IsNullOrEmpty(contentModel.contactInfo.businessPhone))
                {
                    lead.Telephone1 = contentModel.contactInfo.businessPhone;
                }
                if (!String.IsNullOrEmpty(contentModel.contactInfo.fax))
                {
                    lead.Fax = contentModel.contactInfo.fax;
                }
                if (!String.IsNullOrEmpty(contentModel.contactInfo.description))
                {
                    lead.Description = contentModel.contactInfo.description;
                }
                if (!String.IsNullOrEmpty(contentModel.contactInfo.jobTitle))
                {
                    lead.JobTitle = contentModel.contactInfo.jobTitle;
                }
                switch (contentModel.contactInfo.preferredMethodOfContact)
                {
                    case "Email": lead.PreferredContactMethodCode = new OptionSetValue(2); break;
                    case "Phone": lead.PreferredContactMethodCode = new OptionSetValue(3); break;
                    case "Fax": lead.PreferredContactMethodCode = new OptionSetValue(4); break;
                    case "Mail": lead.PreferredContactMethodCode = new OptionSetValue(5); break;
                    default: lead.PreferredContactMethodCode = new OptionSetValue(1); break;
                }
                // lead.PreferredContactMethodCode = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.contactInfo.preferredMethodOfContact));
                if (!String.IsNullOrEmpty(contentModel.contactInfo.line))
                {
                    lead.pfc_line = contentModel.contactInfo.line;
                }
                if (!String.IsNullOrEmpty(contentModel.contactInfo.facebookId))
                {
                    lead.pfc_facebook = contentModel.contactInfo.facebookId;
                }

                // companyInfo
                if (!String.IsNullOrEmpty(contentModel.companyInfo.companyName))
                {
                    lead.CompanyName = contentModel.companyInfo.companyName;
                }
                if (!String.IsNullOrEmpty(contentModel.companyInfo.websiteUrl))
                {
                    lead.WebSiteUrl = contentModel.companyInfo.websiteUrl;
                }
                //lead.Address1_Composite = contentModel.companyInfo.address;

                // productInfo
                lead.pfc_product_group_code = contentModel.productInfo.groupCode;
                if (!String.IsNullOrEmpty(contentModel.productInfo.groupName))
                {
                    lead.pfc_product_group_name = contentModel.productInfo.groupName;
                }
                lead.pfc_product_code = contentModel.productInfo.productCode;
                lead.pfc_product_name = contentModel.productInfo.productName;
                if (!String.IsNullOrEmpty(contentModel.productInfo.categoryCode))
                {
                    lead.pfc_product_category_code = contentModel.productInfo.categoryCode;
                }
                if (!String.IsNullOrEmpty(contentModel.productInfo.categoryName))
                {
                    lead.pfc_product_category_name = contentModel.productInfo.categoryName;
                }
                if ( contentModel.productInfo.budgetAmount.HasValue )
                {
                    decimal budget =contentModel.productInfo.budgetAmount.Value;
                    lead.BudgetAmount = new Money(Math.Round(budget,2));
                }

                // insuredInfo
                lead.pfc_insured_fullname = contentModel.insuredInfo.insuredFullName;
                if (!String.IsNullOrEmpty(contentModel.insuredInfo.insuredMobilePhone))
                {
                    lead.pfc_insured_mobile = contentModel.insuredInfo.insuredMobilePhone;
                }
                // string dateString = contentModel.insuredInfo.insuredIssueDate;
                if ( contentModel.insuredInfo.insuredIssueDate.HasValue )
                {
                    lead.pfc_insured_issue_date = contentModel.insuredInfo.insuredIssueDate;
                }

                // vehicleInfo
                if (!String.IsNullOrEmpty(contentModel.vehicleInfo.brandCode))
                {
                    lead.pfc_brand_code = contentModel.vehicleInfo.brandCode;
                }
                if (!String.IsNullOrEmpty(contentModel.vehicleInfo.brandName))
                {
                    lead.pfc_brand_name = contentModel.vehicleInfo.brandName;
                }
                if (!String.IsNullOrEmpty(contentModel.vehicleInfo.modelCode))
                {
                    lead.pfc_model_code = contentModel.vehicleInfo.modelCode;
                }
                if (!String.IsNullOrEmpty(contentModel.vehicleInfo.modelName))
                {
                    lead.pfc_model_name = contentModel.vehicleInfo.modelName;
                }
                if ( contentModel.vehicleInfo.year.HasValue )
                {
                    int y = contentModel.vehicleInfo.year.Value;
                    lead.pfc_vechicle_year = y;
                }

                _serviceProxy.Create(lead);

                var query = from c in svcContext.LeadSet
                            where c.FirstName == contentModel.contactInfo.firstName && c.LastName == contentModel.contactInfo.lastName && c.Subject == contentModel.generalHeader.topic
                            select c;
                Lead getLead = new Lead();
                try {
                    getLead = query.FirstOrDefault<Lead>();
                } 
                catch(Exception)
                {
                    output.code = AppConst.CODE_FAILED;
                    output.message = AppConst.MESSAGE_INTERNAL_ERROR;
                    output.description = "ไม่สามารถ get ค่า LeadId ได้";
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now;
                }


                output.code = AppConst.CODE_SUCCESS;
                output.message = AppConst.MESSAGE_SUCCESS;
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
                output.data.firstName = contentModel.contactInfo.firstName;
                output.data.lastName = contentModel.contactInfo.lastName;
                output.data.leadId = getLead.pfc_lead_id;

                return output;
            }
            catch(Exception e)
            {
                output.code = AppConst.CODE_FAILED;
                output.message = "ไม่สามารถสร้าง Lead ได้";
                output.description = e.Message;
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;

                return output;
            }

        }

    }
}