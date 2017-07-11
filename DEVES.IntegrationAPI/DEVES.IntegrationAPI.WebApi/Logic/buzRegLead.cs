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
                lead.Subject = contentModel.generalHeader.topic;
                // contactInfo
                lead.Salutation = contentModel.contactInfo.salutation;
                //lead. = contentModel.contactInfo.sex;
                lead.FirstName = contentModel.contactInfo.firstName;
                lead.LastName = contentModel.contactInfo.lastName;
                lead.EMailAddress1 = contentModel.contactInfo.email;
                lead.MobilePhone = contentModel.contactInfo.mobilePhone;
                // lead. = contentModel.contactInfo.businessPhone;
                lead.Fax = contentModel.contactInfo.fax;
                lead.Description = contentModel.contactInfo.description;
                lead.JobTitle = contentModel.contactInfo.jobTitle;
                // lead.PreferredContactMethodCode = new Microsoft.Xrm.Sdk.OptionSetValue(Convert.ToInt32(contentModel.contactInfo.preferredMethodOfContact));
                // lead.DoNotEMail = contentModel.contactInfo.doNotAllowEmails;
                // lead.DoNotBulkEMail = contentModel.contactInfo.doNotAllowBulkEmails;
                // lead.DoNotPhone = contentModel.contactInfo.doNotAllowPhone;
                // lead.DoNotEMail = contentModel.contactInfo.doNotAllowEmails;
                // companyInfo
                lead.CompanyName = contentModel.companyInfo.companyName;
                lead.WebSiteUrl = contentModel.companyInfo.websiteUrl;
                // lead.Address1_Composite = contentModel.companyInfo.address; // Permission can get only

                // _serviceProxy.Create(lead);

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