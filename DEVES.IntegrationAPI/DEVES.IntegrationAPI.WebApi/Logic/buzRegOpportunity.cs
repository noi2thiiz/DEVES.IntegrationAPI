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
using DEVES.IntegrationAPI.Model.personSearchModel;
using DEVES.IntegrationAPI.Model.RegClientPersonal;

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
            buzpersonSearch personSearch = new buzpersonSearch();
            personSearchInputModel inputSearch = new personSearchInputModel();
            inputSearch.generalHeader = new DEVES.IntegrationAPI.Model.personSearchModel.GeneralHeaderModel();
            inputSearch.conditions = new DEVES.IntegrationAPI.Model.personSearchModel.ConditionModel();

            inputSearch.conditions.customerType = "P";

            // Output from Inquiry
            personSearchOutputModel outputSearch = new personSearchOutputModel();
            /*
            // TRANSFORM INPUT
            if ( String.IsNullOrEmpty(contentModel.generalHeader.cleansingId) || String.IsNullOrEmpty(contentModel.generalHeader.crmClientId) )
            {
                inputSearch.conditions.crmClientId = contentModel.generalHeader.crmClientId;
                inputSearch.conditions.cleansingId = contentModel.generalHeader.cleansingId;

                outputSearch = (personSearchOutputModel)personSearch.ExecuteInput(inputSearch);
            }

            // check that search by cleansingId or crmClientId is empty or not?  
            if (outputSearch.Persondata.Count == 0) // don't have data -> it's empty
            {
                inputSearch.conditions.crmClientId = null;
                inputSearch.conditions.cleansingId = null;

                // GeneralHeader
                inputSearch.generalHeader.requester = contentModel.generalHeader.requester;
                // Condition
                inputSearch.conditions.fullName = contentModel.contactInfo.firstName + " " + contentModel.contactInfo.lastName;
                inputSearch.conditions.name1 = contentModel.contactInfo.firstName;
                inputSearch.conditions.name2 = contentModel.contactInfo.lastName;
                // inputSearch.conditions.idCard = contentModel.generalHeader.requester;
                // inputSearch.conditions.crmClientId = contentModel.contactInfo.;
                // inputSearch.conditions.line = contentModel.contactInfo.;
                // inputSearch.conditions.facebook = contentModel.generalHeader.requester;
                // inputSearch.conditions.cleansingId = contentModel.generalHeader.requester;
                inputSearch.conditions.email = contentModel.contactInfo.email;
                inputSearch.conditions.phoneNumber = contentModel.contactInfo.mobilePhone;
                // inputSearch.conditions.customerType = contentModel.generalHeader.requester;

                outputSearch = (personSearchOutputModel)personSearch.ExecuteInput(inputSearch);
            }
            
            // If have only 1 data -> do nothing
            if (outputSearch.Persondata.Count == 1)
            {
                // do nothing
            }
            */
            if(false)
            {

            }
            // else (0 data or more than 1) -> create personal in cls
            else
            {
                // Create personal in cls
                buzCRMRegClientPersonal createPerson = new buzCRMRegClientPersonal();

                RegClientPersonalInputModel inputCreate = new RegClientPersonalInputModel();
                //GeneralHeaderModel inputCreate.generalHeader = new GeneralHeaderModel();
                //ProfileInfoModel
                //ContactInfoModel
                //AddressInfoModel

                // Output from Create
                RegClientPersonalContentOutputModel outputCreate = new RegClientPersonalContentOutputModel();

                // Mapping field
                //
                inputCreate.generalHeader.roleCode = "G";
                inputCreate.generalHeader.cleansingId = contentModel.generalHeader.cleansingId;
                inputCreate.generalHeader.polisyClientId = "";
                inputCreate.generalHeader.crmClientId = contentModel.generalHeader.crmClientId;
                inputCreate.generalHeader.clientAdditionalExistFlag = "N";
                inputCreate.generalHeader.notCreatePolisyClientFlag = "Y";
                //
                inputCreate.profileInfo.salutation = contentModel.contactInfo.salutation;
                inputCreate.profileInfo.personalName = contentModel.contactInfo.firstName;
                inputCreate.profileInfo.personalSurname = contentModel.contactInfo.lastName;
                inputCreate.profileInfo.sex = contentModel.contactInfo.sex;
                //inputCreate.profileInfo.idCitizen = "";
                //inputCreate.profileInfo.idPassport = "";
                //inputCreate.profileInfo.idAlien = "";
                //inputCreate.profileInfo.idDriving = "";
                //inputCreate.profileInfo.birthDate = "";
                //inputCreate.profileInfo.nationality = "";
                //inputCreate.profileInfo.language = "";
                //inputCreate.profileInfo.married = "";
                //inputCreate.profileInfo.occupation = "";
                //inputCreate.profileInfo.riskLevel = "";
                //inputCreate.profileInfo.vipStatus = "";
                //inputCreate.profileInfo.remark = "";

                inputCreate.contactInfo.telephone1 = contentModel.contactInfo.businessPhone;
                //inputCreate.contactInfo.telephone1Ext = "";
                //inputCreate.contactInfo.telephone2 = "";
                //inputCreate.contactInfo.telephone2Ext = "";
                //inputCreate.contactInfo.telephone3 = "";
                //inputCreate.contactInfo.telephone3Ext = "";
                inputCreate.contactInfo.mobilePhone = contentModel.contactInfo.mobilePhone;
                inputCreate.contactInfo.fax = contentModel.contactInfo.fax;
                inputCreate.contactInfo.emailAddress = contentModel.contactInfo.email;
                //inputCreate.contactInfo.lineID = "";
                //inputCreate.contactInfo.facebook = "";

                //inputCreate.addressInfo.address1 = "";
                //inputCreate.addressInfo.address2 = "";
                //inputCreate.addressInfo.address3 = "";
                //inputCreate.addressInfo.subDistrictCode = "";
                //inputCreate.addressInfo.districtCode = "";
                //inputCreate.addressInfo.postalCode = "";
                //inputCreate.addressInfo.country = "";
                //inputCreate.addressInfo.addressType = "";
                //inputCreate.addressInfo.latitude = "";
                //inputCreate.addressInfo.longtitude = "";

                outputCreate = (RegClientPersonalContentOutputModel)createPerson.ExecuteInput(inputCreate);
            }

            /*
             * 
            // Create Opp
            try { 
                OpportunityModel opp = new OpportunityModel();
                opp.Create(contentModel);
                opp.Save();
                
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
            */

            if (!String.IsNullOrEmpty(contentModel.generalHeader.leadId))
            {
                try
                {
                    var queryLead = from c in svcContext.LeadSet
                                where c.pfc_lead_id == contentModel.generalHeader.leadId
                                select c;

                    Lead lead = queryLead.FirstOrDefault<Lead>();
                    lead.SalesStage = new Microsoft.Xrm.Sdk.OptionSetValue(0);

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