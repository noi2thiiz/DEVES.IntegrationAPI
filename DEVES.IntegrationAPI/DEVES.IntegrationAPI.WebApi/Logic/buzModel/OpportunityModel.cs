using DEVES.IntegrationAPI.Model.RegOpportunity;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic.buzModel
{
    public class OpportunityModel
    {
        Opportunity opp;

        CrmServiceClient connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
        // OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
        OrganizationServiceProxy _serviceProxy;
        // ServiceContext svcContext = new ServiceContext(_serviceProxy);
        ServiceContext svcContext;

        public OpportunityModel()
        {
            opp = new Opportunity();
        }

        public void Create(RegOpportunityInputModel input)
        {
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
        }

        // Query in XRM from oppId
        public void Retrieve(string oppId)
        {
            
        }

        // Method for save through XRM
        public void Save()
        {
            _serviceProxy = connection.OrganizationServiceProxy;
            svcContext = new ServiceContext(_serviceProxy);

            if (String.IsNullOrEmpty(opp.OpportunityId.ToString()))
            {
                _serviceProxy.Create(opp);
            }
            else
            {
                _serviceProxy.Update(opp);
            }
            
        }

        public void Developed()
        {

        }

        public void Proposed()
        {

        }

        public void Contacted()
        {

        }

        public void CloseAsWon()
        {

        }

        public void CloseAsLoss()
        {

        }

    }
}