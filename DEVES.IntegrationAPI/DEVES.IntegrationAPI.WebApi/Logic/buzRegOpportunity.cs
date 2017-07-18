using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegOpportunity;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using DEVES.IntegrationAPI.WebApi.Logic.buzModel;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzRegOpportunity : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Preparation Variable
            RegOpportunityOutputModel output = new RegOpportunityOutputModel();

            // Deserialize Input
            RegOpportunityInputModel contentModel = (RegOpportunityInputModel)input;

            // Connect SDK
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);


            // Inquiry Personal

            // If have only 1 data -> do nothing
            if (false)
            {

            }
            // else (0 data or more than 1) -> create personal in cls
            else
            {

            }

            // Create Opp
            try { 
                OpportunityModel opp = new OpportunityModel();
                // opp.Create(contentModel);
                // opp.Save();
                /*
                // generalHeader
                // opp.Subject = contentModel.generalHeader.topic;
                // contactInfo
                opp.Salutation = contentModel.contactInfo.salutation;
                //opp. = contentModel.contactInfo.sex;
                opp.FirstName = contentModel.contactInfo.firstName;
                opp.LastName = contentModel.contactInfo.lastName;
                opp.EMailAddress1 = contentModel.contactInfo.email;
                opp.MobilePhone = contentModel.contactInfo.mobilePhone;
                // opp. = contentModel.contactInfo.businessPhone;
                opp.Fax = contentModel.contactInfo.fax;
                opp.Description = contentModel.contactInfo.description;
                opp.JobTitle = contentModel.contactInfo.jobTitle;
                // opp.PreferredContactMethodCode = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.contactInfo.preferredMethodOfContact));
                // opp.DoNotEMail = contentModel.contactInfo.doNotAllowEmails;
                // opp.DoNotBulkEMail = contentModel.contactInfo.doNotAllowBulkEmails;
                // opp.DoNotPhone = contentModel.contactInfo.doNotAllowPhone;
                // opp.DoNotEMail = contentModel.contactInfo.doNotAllowEmails;
                // companyInfo
                opp.CompanyName = contentModel.companyInfo.companyName;
                opp.WebSiteUrl = contentModel.companyInfo.websiteUrl;
                // opp.Address1_Composite = contentModel.companyInfo.address; // Permission can get only
                */
                // _serviceProxy.Create(opp);

            }
            catch
            {
                output.code = AppConst.CODE_FAILED;
                output.message = "ไม่สามารถสร้าง Opportunity ได้";
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;

                return output;
            }

            if (!String.IsNullOrEmpty(contentModel.generalHeader.leadId))
            {
                try
                {
                    var queryLead = from c in svcContext.LeadSet
                                where c.LeadId == new Guid(contentModel.generalHeader.leadId)
                                select c;

                    Lead lead = queryLead.FirstOrDefault<Lead>();
                    // lead.StatusCode = new Microsoft.Xrm.Sdk.OptionSetValue();

                    // _serviceProxy.Update(lead);
                }
                catch
                {
                    // not update
                }
            }

            output.code = AppConst.CODE_SUCCESS;
            output.message = AppConst.MESSAGE_SUCCESS;
            output.description = "";
            output.transactionId = TransactionId;
            output.transactionDateTime = DateTime.Now;

            return output;
        }
    }
}