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
                // lead.LeadSourceCode = contentModel.generalHeader.requester;
                lead.Subject = contentModel.generalHeader.topic;

                // contactInfo
                lead.Salutation = contentModel.contactInfo.salutation;
                // lead.pfc_sex = contentModel.contactInfo.sex;
                lead.FirstName = contentModel.contactInfo.firstName;
                lead.LastName = contentModel.contactInfo.lastName;
                lead.EMailAddress1 = contentModel.contactInfo.email;
                lead.MobilePhone = contentModel.contactInfo.mobilePhone;
                lead.Telephone1 = contentModel.contactInfo.businessPhone;
                lead.Fax = contentModel.contactInfo.fax;
                lead.Description = contentModel.contactInfo.description;
                lead.JobTitle = contentModel.contactInfo.jobTitle;
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
                lead.pfc_product_category_code = contentModel.productInfo.categoryCode;
                lead.pfc_product_category_name = contentModel.productInfo.categoryName;
                // lead.pfc_product_price = contentModel.productInfo.price;

                // insuredInfo
                lead.pfc_insured_fullname = contentModel.insuredInfo.insuredFullName;
                lead.pfc_insured_mobile = contentModel.insuredInfo.insuredMobilePhone;
                lead.pfc_insured_issue_date = contentModel.insuredInfo.dtinsuredIssueDate;

                // vehicleInfo
                lead.pfc_brand_code = contentModel.vehicleInfo.brandCode;
                lead.pfc_brand_name = contentModel.vehicleInfo.brandName;
                lead.pfc_model_code = contentModel.vehicleInfo.modelCode;
                lead.pfc_model_name = contentModel.vehicleInfo.modelName;
                lead.pfc_vechicle_year = contentModel.vehicleInfo.year;

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