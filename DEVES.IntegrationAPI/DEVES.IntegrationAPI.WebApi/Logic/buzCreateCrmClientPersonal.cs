using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCreateCrmClientPersonal : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {

            RegClientPersonalInputModel contentModel = JsonConvert.DeserializeObject<RegClientPersonalInputModel>(input.ToString());

            // 1. ให้ search crmClientID ก่อน ว่ามีอยู่ใน CRM ไหม
            // 1.1 (ถ้ามี) return cleansingID เลย แต่ไม่ต้องสร้างข้อมูลใหม่ใน CRM
            // 1.2 (ถ้าไม่มี) ให้ทำการ Create crmClientID ใน CRM แล้ว map ข้อมูลจาก input ใส่ลงใน CRM

            // เตรียมข้อมูล (จาก input -> json)
            RegClientPersonalInputModel data = new RegClientPersonalInputModel();
            data.generalHeader = new GeneralHeaderModel();
            data.profileInfo = new ProfileInfoModel();
            data.contactInfo = new ContactInfoModel();
            data.addressInfo = new AddressInfoModel();

            data.generalHeader = contentModel.generalHeader;
            data.profileInfo = contentModel.profileInfo;
            data.contactInfo = contentModel.contactInfo;
            data.addressInfo = contentModel.addressInfo;

            // Search ข้อมูลจาก cleansing มาเก็ยภายใน List
            List<string> crmData = SearchCrmContactClientId(data.generalHeader.cleansingId);

            CRMRegClientPersonalOutputModel dataOutput = new CRMRegClientPersonalOutputModel();
            dataOutput.data = new List<CRMRegClientPersonalOutputDataModel>();

            if (crmData.Count == 0) // Means List crmData is empty
            {
                // Create new one and map input value to CRM
                using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
                {

                    Contact contact = new Contact();
                    Account account = new Account();
                    // Address address = new Address();
                    crmSvc.EnableProxyTypes();

                    //Create Client Additional Records
                    if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("N"))
                    {
                        // generalHeader
                        // contentModel.generalHeader.roleCode; 
                        // account, contact
                        contact.pfc_cleansing_cusormer_profile_code = contentModel.generalHeader.cleansingId;
                        account.pfc_cleansing_cusormer_profile_code = contentModel.generalHeader.cleansingId;
                        // account, contact
                        contact.pfc_polisy_client_id = contentModel.generalHeader.polisyClientId;
                        account.pfc_polisy_client_id = contentModel.generalHeader.polisyClientId;
                        // account, contact
                        contact.pfc_crm_person_id = contentModel.generalHeader.crmClientId;
                        account.AccountNumber = contentModel.generalHeader.crmClientId;

                        // profileInfo
                        // account, contact
                        contact.Salutation = contentModel.profileInfo.salutation;
                        account.pfc_saluation = contentModel.profileInfo.salutation;
                        // contact
                        contact.FirstName = contentModel.profileInfo.personalName;
                        contact.LastName = contentModel.profileInfo.personalSurname;
                        // contentModel.profileInfo.sex;
                        // contact
                        contact.pfc_citizen_id = contentModel.profileInfo.idCitizen;
                        // contact
                        contact.pfc_passport_id = contentModel.profileInfo.idPassport;
                        // contact
                        contact.pfc_alien_id = contentModel.profileInfo.idAlien;
                        // contact
                        contact.pfc_driver_license = contentModel.profileInfo.idDriving;
                        // contact
                        contact.pfc_date_of_birth = Convert.ToDateTime(contentModel.profileInfo.birthDate);
                        // account, contact
                        // contact.pfc_nationalityId = contentModel.profileInfo.nationality; // Lookup
                        // account.pfc_nationalityId = contentModel.profileInfo.nationality; // Lookup
                        // account, contact
                        // account.pfc_language = contentModel.profileInfo.language; // optionset
                        // contact.pfc_language = contentModel.profileInfo.language; // optionset
                        // contact
                        // contact.FamilyStatusCode = contentModel.profileInfo.married; // optionset
                        // contentModel.profileInfo.occupation;
                        // contact
                        // contact.pfc_client_legal_status = contentModel.profileInfo.riskLevel; // optionset
                        // contact
                        // contact.pfc_customer_vip = contentModel.profileInfo.vipStatus; // bool
                        // contentModel.profileInfo.remark;

                        // contactInfo 
                        // account, contact
                        contact.Telephone1 = contentModel.contactInfo.telephone1 + '#' + contentModel.contactInfo.telephone1Ext;
                        contact.Telephone2 = contentModel.contactInfo.telephone2 + '#' + contentModel.contactInfo.telephone2Ext;
                        contact.Telephone3 = contentModel.contactInfo.telephone3 + '#' + contentModel.contactInfo.telephone3Ext;
                        // account, contact
                        contact.pfc_moblie_phone1 = contentModel.contactInfo.mobilePhone;
                        account.pfc_moblie_phone1 = contentModel.contactInfo.mobilePhone;
                        // account, contact
                        contact.EMailAddress1 = contentModel.contactInfo.emailAddress;
                        account.EMailAddress1 = contentModel.contactInfo.emailAddress;
                        // account, contact
                        contact.pfc_line_id = contentModel.contactInfo.lineID;
                        account.pfc_line_id = contentModel.contactInfo.lineID;
                        // account, contact
                        contact.pfc_facebook = contentModel.contactInfo.facebook;
                        account.pfc_facebook = contentModel.contactInfo.facebook;


                        // addressInfo
                        /*
                        contentModel.addressInfo.address1;
                        contentModel.addressInfo.address2;
                        contentModel.addressInfo.address3;
                        contentModel.addressInfo.subDistrictCode;
                        contentModel.addressInfo.districtCode;
                        contentModel.addressInfo.provinceCode;
                        contentModel.addressInfo.postalCode;
                        contentModel.addressInfo.country;
                        contentModel.addressInfo.addressType;
                        contentModel.addressInfo.latitude;
                        contentModel.addressInfo.longtitude;


                        ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                        {
                            Requests = new OrganizationRequestCollection(),
                            ReturnResponses = true
                        };

                        CreateRequest createClaimReq = new CreateRequest() { Target = contact };
                        tranReq.Requests.Add(createClaimReq);
                        ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);
                        */
                    }
                    // Update Client Additional Records
                    else if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("Y"))
                    {

                        ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                        {
                            Requests = new OrganizationRequestCollection(),
                            ReturnResponses = true
                        };

                        UpdateRequest updateCaseReq = new UpdateRequest { Target = contact };
                        tranReq.Requests.Add(updateCaseReq);
                        ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);
                    }
                    
                    CRMRegClientPersonalOutputDataModel output = new CRMRegClientPersonalOutputDataModel();

                    output.cleansingId = contentModel.generalHeader.cleansingId;
                    output.polisyClientId = contentModel.generalHeader.polisyClientId;
                    output.crmClientId = contentModel.generalHeader.crmClientId;
                    output.personalName = contentModel.profileInfo.personalName;
                    output.personalSurname = contentModel.profileInfo.personalSurname;

                    dataOutput.data.Add(output);
                }
            }
            else // Means List crmData is not empty (1 or many)
            {
                CRMRegClientPersonalOutputDataModel output = new CRMRegClientPersonalOutputDataModel();

                output.cleansingId = contentModel.generalHeader.cleansingId;
                output.polisyClientId = contentModel.generalHeader.polisyClientId;
                output.crmClientId = contentModel.generalHeader.crmClientId;
                // output.personalName = contentModel.profileInfo.personalName;
                foreach (object obj in crmData)
                {
                    output.personalName += obj;
                    // loop body
                }
                output.personalName = crmData[0];
                // output.personalSurname = contentModel.profileInfo.personalSurname;
                output.personalSurname = crmData[0].ToString();

                dataOutput.data.Add(output);
            }

            return dataOutput;
        }
    }
}