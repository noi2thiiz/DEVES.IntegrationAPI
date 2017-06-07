using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCreateCrmClientCorporate : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            try
            {

                RegClientCorporateInputModel contentModel = (RegClientCorporateInputModel) input;

                // 1. ให้ search crmClientID ก่อน ว่ามีอยู่ใน CRM ไหม
                // 1.1 (ถ้ามี) return cleansingID เลย แต่ไม่ต้องสร้างข้อมูลใหม่ใน CRM
                // 1.2 (ถ้าไม่มี) ให้ทำการ Create crmClientID ใน CRM แล้ว map ข้อมูลจาก input ใส่ลงใน CRM

                // เตรียมข้อมูล (จาก input -> json)

                RegClientCorporateInputModel data = new RegClientCorporateInputModel();
                data.generalHeader = new GeneralHeaderModel();
                data.profileHeader = new ProfileHeaderModel();
                data.contactHeader = new ContactHeaderModel();
                data.addressHeader = new AddressHeaderModel();
                data.asrhHeader = new AsrhHeaderModel();

                data.generalHeader = contentModel.generalHeader;
                data.profileHeader = contentModel.profileHeader;
                data.contactHeader = contentModel.contactHeader;
                data.addressHeader = contentModel.addressHeader;
                data.asrhHeader = contentModel.asrhHeader;

                // Search ข้อมูลจาก cleansing มาเก็ยภายใน List
                List<string> crmData = SearchCrmAccountClientId(data.generalHeader.cleansingId);

                CreateCrmCorporateInfoOutputModel dataOutput = new CreateCrmCorporateInfoOutputModel();
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;
                // dataOutput.data = new List<RegClientCorporateDataOutputModel_Pass>();

                if (string.IsNullOrEmpty(data.generalHeader.cleansingId))
                {
                    dataOutput.code = AppConst.CODE_FAILED;
                    dataOutput.description = "ไม่มี CleansingID";
                    dataOutput.transactionId = TransactionId;
                    dataOutput.transactionDateTime = DateTime.Now;

                    return dataOutput;
                }
           
                if (crmData.Count == 0) // Means List crmData is empty
                {
                    using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
                    {
                       
                        Account account = new Account();
                        // Address address = new Address();
                        crmSvc.EnableProxyTypes();
                       
                        //Create Client Additional Records
                        if (contentModel?.generalHeader?.clientAdditionalExistFlag != "Y")
                        {
                           
                            // generalHeader
                            account.pfc_cleansing_cusormer_profile_code = contentModel?.generalHeader?.cleansingId;
                            account.pfc_polisy_client_id = contentModel?.generalHeader?.polisyClientId;
                            // account.AccountNumber = contentModel.generalHeader.crmClientId;
                           
                            // profileHeader
                            //account.Name = contentModel.profileHeader.corporateName1 + " " + contentModel.profileHeader.corporateName2;
                            account.pfc_long_surname = contentModel.profileHeader.corporateName1;
                            account.pfc_long_giving_name = contentModel.profileHeader.corporateName2;
                            account.pfc_contact_personal = contentModel.profileHeader.contactPerson;
                            account.pfc_register_no =
                                contentModel.profileHeader.idRegCorp; // contentModel.profileHeader.idRegCorp;
                            account.pfc_tax_no = contentModel.profileHeader.idTax; // contentModel.profileHeader.idTax;
                            
                            if (contentModel?.profileHeader?.dateInCorporate != null)
                            {
                                account.pfc_date_of_birth = contentModel.profileHeader.dateInCorporate;
                            }

                            
                            account.pfc_tac_branch =
                                contentModel.profileHeader
                                    .corporateBranch; // contentModel.profileHeader.corporateBranch;
                            if (!string.IsNullOrEmpty(contentModel?.profileHeader?.econActivity))
                            {

                                account.pfc_economic_type =
                                    OptionSetConvertor.GetOptionsetValue(contentModel?.profileHeader?.econActivity);
                            }
                           
                            account.pfc_polisy_nationality_code = contentModel.profileHeader.countryOrigin;

                            account.pfc_language =
                                OptionSetConvertor.GetLanguageOptionSetValue(contentModel?.profileHeader?.language);

                         
                            if (!string.IsNullOrEmpty(contentModel.profileHeader.riskLevel))
                            {
                                account.pfc_AMLO_flag =
                                    OptionSetConvertor.GetRiskLevelOptionSetValue(
                                        contentModel?.profileHeader?.riskLevel);
                            }

                            

                            // contact
                            bool isVIP = contentModel?.profileHeader?.vipStatus == "Y";
                            account.pfc_customer_vip = isVIP; // bool

                            // contactHeader 

                            account.Telephone1 = TelephoneConvertor(contentModel?.contactHeader?.telephone1,
                                contentModel?.contactHeader?.telephone1Ext);
                            account.Telephone2 = TelephoneConvertor(contentModel?.contactHeader?.telephone2,
                                contentModel?.contactHeader?.telephone2Ext);
                            account.Telephone3 = TelephoneConvertor(contentModel?.contactHeader?.telephone3,
                                contentModel?.contactHeader?.telephone3Ext);
                            account.pfc_moblie_phone1 = contentModel?.contactHeader?.mobilePhone;
                            account.EMailAddress1 = contentModel?.contactHeader?.emailAddress;
                            account.pfc_line_id = contentModel?.contactHeader?.lineID;
                            account.pfc_facebook = contentModel?.contactHeader?.facebook;
                            account.pfc_source_data = OptionSetConvertor.GetIntegrationSourceDataOptionSetValue();

                            // new 6 parameters from P'Guide
                            account.pfc_telephone1 = contentModel?.contactHeader?.telephone1;
                            account.pfc_telephone2 = contentModel?.contactHeader?.telephone2;
                            account.pfc_telephone3 = contentModel?.contactHeader?.telephone3;
                            account.pfc_fax = contentModel?.contactHeader?.fax;
                            account.pfc_moblie_phone = contentModel?.contactHeader?.mobilePhone;
                            account.pfc_emailaddress1 = contentModel?.contactHeader?.emailAddress;


                            ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                            {
                                Requests = new OrganizationRequestCollection(),
                                ReturnResponses = true
                            };

                            CreateRequest createClaimReq = new CreateRequest() {Target = account};
                            tranReq.Requests.Add(createClaimReq);
                            ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse) crmSvc.Execute(tranReq);
                           
                        }

                        // Update Client Additional Records
                        else if (contentModel?.generalHeader?.clientAdditionalExistFlag == "Y")
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

                    List<string> crmIdOutput = SearchCrmAccountClientId(data.generalHeader.cleansingId);

                    if (crmIdOutput != null && crmIdOutput.Count > 0)
                    {
                        dataOutput.crmClientId = crmIdOutput[0]; // generate from crm
                        // dataOutput.data = "Waiting for generating algorithm";
                        //dataOutput.data = contentModel.generalHeader.crmClientId;
                        dataOutput.code = "200";
                    }
                    else
                    {
                        dataOutput.code = "500";
                        dataOutput.message =
                            string.Format("No account with cleansingId {0} found after create CRM account.",
                                data.generalHeader.cleansingId);
                    }
                    return dataOutput;
                }
                else if (crmData.Count == 1) // Means List crmData has 1 data
                {
                    dataOutput.crmClientId = crmData[0];
                }
                dataOutput.code = "200";
                return dataOutput;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
                CreateCrmCorporateInfoOutputModel dataOutput = new CreateCrmCorporateInfoOutputModel();
                dataOutput.code = AppConst.CODE_FAILED;
                dataOutput.description = "Error on Create CRM";
                dataOutput.transactionId = TransactionId;
                dataOutput.transactionDateTime = DateTime.Now;

                return dataOutput;

            }

        }

        public string TelephoneConvertor(string tel, string ext)
        {
            string telNum = "";

            if (ext == null || ext.Equals(""))
            {
                telNum = tel;
            }
            else
            {
                telNum = tel + "#" + ext;
            }

            return telNum;
        }

        
    }
}