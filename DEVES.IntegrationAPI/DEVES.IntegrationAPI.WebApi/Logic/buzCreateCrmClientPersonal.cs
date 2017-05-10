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

            RegClientPersonalInputModel contentModel = (RegClientPersonalInputModel)input;

            // 1. ให้ search crmClientID ก่อน ว่ามีอยู่ใน CRM ไหม
            // 1.1 (ถ้ามี) return cleansingID เลย แต่ไม่ต้องสร้างข้อมูลใหม่ใน CRM
            // 1.2 (ถ้าไม่มี) ให้ทำการ Create crmClientID ใน CRM แล้ว map ข้อมูลจาก input ใส่ลงใน CRM

            // เตรียมข้อมูล (จาก input -> json)
            RegClientPersonalInputModel data = new RegClientPersonalInputModel();

            data.generalHeader = contentModel.generalHeader ?? new GeneralHeaderModel();
            data.profileInfo = contentModel.profileInfo ?? new ProfileInfoModel();
            data.contactInfo = contentModel.contactInfo ?? new ContactInfoModel();
            data.addressInfo = contentModel.addressInfo ?? new AddressInfoModel();

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

                    crmSvc.EnableProxyTypes();

                    //Create Client Additional Records
                    if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("N"))
                    {


                        
                        // generalHeader
                        contact.pfc_cleansing_cusormer_profile_code = contentModel.generalHeader.cleansingId;
                        contact.pfc_polisy_client_id = contentModel.generalHeader.polisyClientId;
                        contact.pfc_crm_person_id = contentModel.generalHeader.crmClientId;

                        // profileInfo
                        contact.Salutation = contentModel.profileInfo.salutation;
                        contact.FirstName = contentModel.profileInfo.personalName;
                        contact.LastName = contentModel.profileInfo.personalSurname;
                        Console.WriteLine(contentModel.profileInfo.sex.ToUpper());
                        if (contentModel.profileInfo.sex == null) contentModel.profileInfo.sex = "U";
                        switch (contentModel.profileInfo.sex.ToUpper())
                        {
                            case "M": contact.GenderCode = new OptionSetValue(1) ; break;
                            case "F": contact.GenderCode = new OptionSetValue(2) ; break;
                            case "U": contact.GenderCode = new OptionSetValue(100000000) ; break;

                        }

                        contact.pfc_citizen_id = contentModel.profileInfo.idCitizen;
                        contact.pfc_passport_id = contentModel.profileInfo.idPassport;
                        contact.pfc_alien_id = contentModel.profileInfo.idAlien;
                        contact.pfc_driver_license = contentModel.profileInfo.idDriving;


                        if(contentModel.profileInfo?.birthDate != null)
                        {

                            contact.pfc_date_of_birth = contentModel.profileInfo.birthDate;

                        }

                        contact.pfc_polisy_nationality_code = contentModel.profileInfo.nationality;
                        if (contentModel.profileInfo.language == null) contentModel.profileInfo.language = "";
                        switch (contentModel.profileInfo.language.ToUpper())
                        {
                            case "T": contact.pfc_language = new OptionSetValue(100000003) ; break;
                            case "E": contact.pfc_language = new OptionSetValue(100000001) ; break;
                            case "J": contact.pfc_language = new OptionSetValue(100000002); break;

                        }

                        if (contentModel.profileInfo.married == null) contentModel.profileInfo.married = "";
                        switch (contentModel.profileInfo.married.ToUpper())
                        {
                            case "S": contact.FamilyStatusCode = new OptionSetValue(1); break;
                            case "M": contact.FamilyStatusCode = new OptionSetValue(2); break;
                            case "D": contact.FamilyStatusCode = new OptionSetValue(3); break;
                            case "W": contact.FamilyStatusCode = new OptionSetValue(4); break;

                        }

                        //contact.pfc_occupation = new EntityReference(contact.pfc_occupation.LogicalName, contentModel.profileInfo.occupation);

                        if (string.IsNullOrEmpty(contentModel.profileInfo.riskLevel))
                        {
                            // do nothing
                        }
                        else
                        {
                            switch (contentModel.profileInfo.riskLevel.ToUpper())
                            {
                                case "A": contact.pfc_AMLO_flag = new OptionSetValue(100000001); break; // A
                                case "B": contact.pfc_AMLO_flag = new OptionSetValue(100000002); break; // B
                                case "C1": contact.pfc_AMLO_flag = new OptionSetValue(100000003); break;
                                case "C2": contact.pfc_AMLO_flag = new OptionSetValue(100000004); break;
                                case "R1": contact.pfc_AMLO_flag = new OptionSetValue(100000005); break;
                                case "R2": contact.pfc_AMLO_flag = new OptionSetValue(100000006); break;
                                case "R3": contact.pfc_AMLO_flag = new OptionSetValue(100000007); break;
                                case "R4": contact.pfc_AMLO_flag = new OptionSetValue(100000008); break;
                                case "RL1": contact.pfc_AMLO_flag = new OptionSetValue(100000009); break;
                                case "RL2": contact.pfc_AMLO_flag = new OptionSetValue(100000010); break;
                                case "RL3": contact.pfc_AMLO_flag = new OptionSetValue(100000011); break;
                                case "U": contact.pfc_AMLO_flag = new OptionSetValue(100000012); break; // U
                                case "X": contact.pfc_AMLO_flag = new OptionSetValue(100000013); break;
                                default: contact.pfc_AMLO_flag = new OptionSetValue(); break;

                            }
                        }
                        

                        bool isVIP = false;
                        if (contentModel.profileInfo.vipStatus == null) contentModel.profileInfo.vipStatus = "";
                        if(contentModel.profileInfo.vipStatus.Equals("Y"))
                        {
                            isVIP = true;
                        }
                        contact.pfc_customer_vip = isVIP; // bool
                                                          // contentModel.profileInfo.remark;

                        // contactInfo 
                        contact.Telephone1 = TelephoneConvertor(contentModel.contactInfo.telephone1, contentModel.contactInfo.telephone1Ext);
                        contact.Telephone2 = TelephoneConvertor(contentModel.contactInfo.telephone2, contentModel.contactInfo.telephone2Ext);
                        contact.Telephone3 = TelephoneConvertor(contentModel.contactInfo.telephone3, contentModel.contactInfo.telephone3Ext);
                        contact.pfc_moblie_phone1 = contentModel.contactInfo.mobilePhone;
                        contact.EMailAddress1 = contentModel.contactInfo.emailAddress;
                        contact.pfc_line_id = contentModel.contactInfo.lineID;
                        contact.pfc_facebook = contentModel.contactInfo.facebook;

                        ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                        {
                            Requests = new OrganizationRequestCollection(),
                            ReturnResponses = true
                        };

                        CreateRequest createClaimReq = new CreateRequest() { Target = contact };
                        tranReq.Requests.Add(createClaimReq);
                        ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);
                        
                    }
                    // Update Client Additional Records

                    else if ( string.IsNullOrEmpty(contentModel.generalHeader.clientAdditionalExistFlag)
                              && contentModel.generalHeader.clientAdditionalExistFlag.Equals("Y"))
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
                dataOutput.code =CONST_CODE_SUCCESS;
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                return dataOutput;
            }
            else if (crmData.Count == 1) // Means List crmData has 1 data
            {
                dataOutput.crmClientId = crmData[0];
                dataOutput.code = CONST_CODE_SUCCESS;
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                return dataOutput;
            }
            else
            {
                dataOutput.code =CONST_CODE_FAILED;
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                return dataOutput;
            }
            
        }

        public string TelephoneConvertor(string tel, string ext)
        {
            string telNum = "";

            if(ext == null || ext.Equals(""))
            {
                telNum = tel;
            }
            else
            {
                telNum = tel + "#" + ext;
            }

            return telNum;
        }

        public string OptionsetConvertor(string val)
        {
            string opVal = "";

            if (val.Length == 1)
            {
                opVal = "10000000" + val;
            }
            else
            {
                opVal = "1000000" + val;
            }

            return opVal;
        }

    }
}