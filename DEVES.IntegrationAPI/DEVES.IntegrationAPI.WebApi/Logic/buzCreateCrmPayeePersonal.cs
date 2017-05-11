using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCreateCrmPayeePersonal : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {

            RegPayeePersonalInputModel contentModel = (RegPayeePersonalInputModel)input;

            // 1. ให้ search crmPayeeID ก่อน ว่ามีอยู่ใน CRM ไหม
            // 1.1 (ถ้ามี) return cleansingID เลย แต่ไม่ต้องสร้างข้อมูลใหม่ใน CRM
            // 1.2 (ถ้าไม่มี) ให้ทำการ Create crmPayeeID ใน CRM แล้ว map ข้อมูลจาก input ใส่ลงใน CRM

            // เตรียมข้อมูล (จาก input -> json)
            RegPayeePersonalInputModel data = new RegPayeePersonalInputModel();
            data.generalHeader = new GeneralHeaderModel();
            data.profileInfo = new ProfileInfoModel();
            data.contactInfo = new ContactInfoModel();
            data.addressInfo = new AddressInfoModel();

            data.generalHeader = contentModel.generalHeader;
            data.profileInfo = contentModel.profileInfo;
            data.contactInfo = contentModel.contactInfo;
            data.addressInfo = contentModel.addressInfo;

            // Search ข้อมูลจาก cleansing มาเก็บภายใน List
            //List<string> crmData = SearchCrmContactPayeeId(data.generalHeader.cleansingId);

            CreateCrmPersonInfoOutputModel dataOutput = new CreateCrmPersonInfoOutputModel();
            // dataOutput.data = new List<CreateCrmPersonInfoOutputModel>();
    
            // Create new one and map input value to CRM
            using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
            {
                crmSvc.EnableProxyTypes();
                List<string> crmData = SearchCrmContactPayeeId(crmSvc, data.generalHeader.cleansingId);
                if (crmData.Count == 0) // Means List crmData is empty
                {
                    Contact contact = new Contact();
                    Account account = new Account();
                    // Address address = new Address();

                    //Create Payee Additional Records
                    //if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("N"))
                    //{

                        // generalHeader
                        // contentModel.generalHeader.roleCode; 
                        contact.pfc_cleansing_cusormer_profile_code = contentModel.generalHeader.cleansingId;
                        contact.pfc_polisy_client_id = contentModel.generalHeader.polisyClientId;
                        contact.pfc_crm_person_id = contentModel.generalHeader.crmPersonId;
                        // account.AccountNumber = contentModel.generalHeader.crmPayeeId;

                        // profileInfo
                        contact.Salutation = contentModel.profileInfo.salutation;
                        contact.FirstName = contentModel.profileInfo.personalName;
                        contact.LastName = contentModel.profileInfo.personalSurname;
                        // contentModel.profileInfo.sex;
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
                        if (contentModel?.profileInfo?.birthDate != null)
                        {
                            contact.pfc_date_of_birth = Convert.ToDateTime(contentModel.profileInfo.birthDate);

                        }

                        contact.pfc_polisy_nationality_code = contentModel.profileInfo.nationality;
                        // contact.pfc_language = contentModel.profileInfo.language; // optionset
                        if (contentModel.profileInfo.language == null) contentModel.profileInfo.language = "";
                        switch (contentModel.profileInfo.language.ToUpper())
                        {
                            case "T": contact.pfc_language = new OptionSetValue(100000003) ; break;
                            case "E": contact.pfc_language = new OptionSetValue(100000001) ; break;
                            case "J": contact.pfc_language = new OptionSetValue(100000002); break;

                        }
                        // contact.FamilyStatusCode = contentModel.profileInfo.married; // optionset
                        if (contentModel.profileInfo.married == null) contentModel.profileInfo.married = "";
                        switch (contentModel.profileInfo.married.ToUpper())
                        {
                            case "S": contact.FamilyStatusCode = new OptionSetValue(1); break;
                            case "M": contact.FamilyStatusCode = new OptionSetValue(2); break;
                            case "D": contact.FamilyStatusCode = new OptionSetValue(3); break;
                            case "W": contact.FamilyStatusCode = new OptionSetValue(4); break;

                        }
                        // contentModel.profileInfo.occupation;
                        // contact.pfc_client_legal_status = contentModel.profileInfo.riskLevel; // optionset
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
                        if (contentModel.profileInfo.vipStatus.Equals("Y"))
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
                    contact.pfc_source_data = new OptionSetValue(100000003);

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
                        CreateResponse res = (CreateResponse)crmSvc.Execute(createClaimReq);
                        //res.id 
                        Microsoft.Xrm.Sdk.Query.ColumnSet colSet = new Microsoft.Xrm.Sdk.Query.ColumnSet();
                        colSet.AddColumn("pfc_crm_person_id");
                        Entity newContact = crmSvc.Retrieve(Contact.EntityLogicalName, res.id, colSet);

                        dataOutput.crmClientId = newContact["pfc_crm_person_id"].ToString() ;
                        dataOutput.code =AppConst.CODE_SUCCESS;
                        dataOutput.transactionId = TransactionId;
                        dataOutput.transactionDateTime = DateTime.Now;

                        //tranReq.Requests.Add(createClaimReq);
                        //createClaimReq = new CreateRequest() { Target = account };
                        //tranReq.Requests.Add(createClaimReq);
                        //ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);

                    //}
                    //// Update Payee Additional Records
                    //else if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("Y"))
                    //{
                    //    // logic for update
                    //    /*
                    //    ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                    //    {
                    //        Requests = new OrganizationRequestCollection(),
                    //        ReturnResponses = true
                    //    };

                    //    UpdateRequest updateCaseReq = new UpdateRequest { Target = contact };
                    //    tranReq.Requests.Add(updateCaseReq);
                    //    ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);
                    //    */
                    //}

                    //List<string> crmIdOutput = SearchCrmContactPayeeId(crmSvc, data.generalHeader.cleansingId);

                    //if(crmIdOutput.Count == 1)
                    //{
                    //    dataOutput.crmClientId = crmIdOutput[0]; // generate from crm
                    //                                             // dataOutput.data = "Waiting for generating algorithm";
                    //                                             //dataOutput.data = contentModel.generalHeader.crmPayeeId;
                    //    dataOutput.code = "200";
                    //    dataOutput.transactionId = TransactionId;
                    //    dataOutput.transactionDateTime = DateTime.Now;
                    //}
                    return dataOutput;

                }
                else if (crmData.Count == 1) // Means List crmData has 1 data
                {
                    dataOutput.crmClientId = crmData[0];
                    dataOutput.code = AppConst.CODE_SUCCESS;
                    dataOutput.transactionId = TransactionId;
                    dataOutput.transactionDateTime = DateTime.Now;
                    return dataOutput;
                }
                else
                {
                    dataOutput.code = AppConst.CODE_FAILED;
                    dataOutput.transactionId = TransactionId;
                    dataOutput.transactionDateTime = DateTime.Now;
                    return dataOutput;
                }
            }

        }

        private enum ENUM_CLIENT_TYPE
        {
            Personal,
            Corporate
        }
        private List<string> SearchCrmContactPayeeId(OrganizationServiceProxy crmSvc , string cleansingId, ENUM_CLIENT_TYPE clientType = ENUM_CLIENT_TYPE.Personal)
        {
            List<string> l = new List<string>();
            using (ServiceContext sc = new ServiceContext(crmSvc))
            {

                var contacts = (from c in sc.ContactSet
                                where c.pfc_cleansing_cusormer_profile_code == cleansingId
                                select new Contact { pfc_crm_person_id = c.pfc_crm_person_id }).Take(2);
                if (contacts != null)
                {
                    foreach (Contact c in contacts)
                    {
                        l.Add(c.pfc_crm_person_id);
                    }
                }

                //var q = (from a in sc.AccountSet
                //                 where a.pfc_cleansing_cusormer_profile_code == cleansingId
                //                 select new { CrmClientId = a.AccountNumber });
                //        if (q != null)
                //        {
                //            l = q.ToList<string>().Take(2);
                //        }
            }
            return l.ToList();
        }
    }

}