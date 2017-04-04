﻿using System;
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

            RegClientPersonalInputModel contentModel = (RegClientPersonalInputModel)input;

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

            // Search ข้อมูลจาก cleansing มาเก็บภายใน List
            List<string> crmData = SearchCrmContactClientId(data.generalHeader.cleansingId);

            CreateCrmPersonInfoOutputModel dataOutput = new CreateCrmPersonInfoOutputModel();
            // dataOutput.data = new List<CreateCrmPersonInfoOutputModel>();

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
                        contact.pfc_cleansing_cusormer_profile_code = contentModel.generalHeader.cleansingId;
                        contact.pfc_polisy_client_id = contentModel.generalHeader.polisyClientId;
                        contact.pfc_crm_person_id = contentModel.generalHeader.crmClientId;
                        // account.AccountNumber = contentModel.generalHeader.crmClientId;

                        // profileInfo
                        contact.Salutation = contentModel.profileInfo.salutation;
                        contact.FirstName = contentModel.profileInfo.personalName;
                        contact.LastName = contentModel.profileInfo.personalSurname;
                        // contentModel.profileInfo.sex;
                        contact.pfc_citizen_id = contentModel.profileInfo.idCitizen;
                        contact.pfc_passport_id = contentModel.profileInfo.idPassport;
                        contact.pfc_alien_id = contentModel.profileInfo.idAlien;
                        contact.pfc_driver_license = contentModel.profileInfo.idDriving;
                        contact.pfc_date_of_birth = Convert.ToDateTime(contentModel.profileInfo.birthDate);
                        contact.pfc_polisy_nationality_code = contentModel.profileInfo.nationality;
                        // contact.pfc_language = contentModel.profileInfo.language; // optionset
                        // contact.FamilyStatusCode = contentModel.profileInfo.married; // optionset
                        // contentModel.profileInfo.occupation;
                        // contact.pfc_client_legal_status = contentModel.profileInfo.riskLevel; // optionset
                        bool isVIP = false;
                        if(contentModel.profileInfo.vipStatus.Equals("Y"))
                        {
                            isVIP = true;
                        }
                        contact.pfc_customer_vip = isVIP; // bool
                                                          // contentModel.profileInfo.remark;

                        // contactInfo 
                        contact.Telephone1 = contentModel.contactInfo.telephone1 + '#' + contentModel.contactInfo.telephone1Ext;
                        contact.Telephone2 = contentModel.contactInfo.telephone2 + '#' + contentModel.contactInfo.telephone2Ext;
                        contact.Telephone3 = contentModel.contactInfo.telephone3 + '#' + contentModel.contactInfo.telephone3Ext;
                        contact.pfc_moblie_phone1 = contentModel.contactInfo.mobilePhone;
                        contact.EMailAddress1 = contentModel.contactInfo.emailAddress;
                        contact.pfc_line_id = contentModel.contactInfo.lineID;
                        contact.pfc_facebook = contentModel.contactInfo.facebook;


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

                        
                    */
                        ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                        {
                            Requests = new OrganizationRequestCollection(),
                            ReturnResponses = true
                        };

                        CreateRequest createClaimReq = new CreateRequest() { Target = contact };
                        tranReq.Requests.Add(createClaimReq);
                        createClaimReq = new CreateRequest() { Target = account };
                        tranReq.Requests.Add(createClaimReq);
                        ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);
                        
                    }
                    // Update Client Additional Records
                    else if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("Y"))
                    {
                        // logic for update
                        /*
                        ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                        {
                            Requests = new OrganizationRequestCollection(),
                            ReturnResponses = true
                        };

                        UpdateRequest updateCaseReq = new UpdateRequest { Target = contact };
                        tranReq.Requests.Add(updateCaseReq);
                        ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);
                        */
                    }

                }

                List<string> crmIdOutput = SearchCrmContactClientId(data.generalHeader.cleansingId);

                dataOutput.crmClientId = crmIdOutput[0]; // generate from crm
                // dataOutput.data = "Waiting for generating algorithm";
                //dataOutput.data = contentModel.generalHeader.crmClientId;
                dataOutput.code = "200";
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                return dataOutput;
            }
            else if (crmData.Count == 1) // Means List crmData has 1 data
            {
                dataOutput.crmClientId = crmData[0];
                dataOutput.code = "200";
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                return dataOutput;
            }
            else
            {
                dataOutput.code = "500";
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                return dataOutput;
            }
            
        }


    }
}