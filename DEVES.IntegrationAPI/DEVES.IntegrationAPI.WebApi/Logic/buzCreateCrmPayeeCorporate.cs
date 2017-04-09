﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCreateCrmPayeeCorporate : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            RegPayeeCorporateInputModel contentModel = (RegPayeeCorporateInputModel)input;

            // 1. ให้ search crmPayeeID ก่อน ว่ามีอยู่ใน CRM ไหม
            // 1.1 (ถ้ามี) return cleansingID เลย แต่ไม่ต้องสร้างข้อมูลใหม่ใน CRM
            // 1.2 (ถ้าไม่มี) ให้ทำการ Create crmPayeeID ใน CRM แล้ว map ข้อมูลจาก input ใส่ลงใน CRM

            // เตรียมข้อมูล (จาก input -> json)

            RegPayeeCorporateInputModel data = new RegPayeeCorporateInputModel();
            data.generalHeader = new GeneralHeaderModel();
            data.profileHeader = new ProfileHeaderModel();
            data.contactHeader = new ContactHeaderModel();
            data.addressHeader = new AddressHeaderModel();
            //data.asrhHeader = new AsrhHeaderModel();

            data.generalHeader = contentModel.generalHeader;
            data.profileHeader = contentModel.profileHeader;
            data.contactHeader = contentModel.contactHeader;
            data.addressHeader = contentModel.addressHeader;
            //data.asrhHeader = contentModel.asrhHeader;

            using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
            {
                // Search ข้อมูลจาก cleansing มาเก็ยภายใน List
                List<string> crmData = SearchCrmAccountPayeeId( crmSvc , data.generalHeader.cleansingId);

                CreateCrmCorporateInfoOutputModel dataOutput = new CreateCrmCorporateInfoOutputModel();
                // dataOutput.data = new List<RegPayeeCorporateDataOutputModel_Pass>();

                if (crmData.Count == 0) // Means List crmData is empty
                {

                    Contact contact = new Contact();
                    Account account = new Account();
                    // Address address = new Address();
                    crmSvc.EnableProxyTypes();

                    //Create Payee Additional Records
                    if (contentModel.generalHeader.clientAdditionalExistFlag.Equals("N"))
                    {

                        // generalHeader
                        account.pfc_cleansing_cusormer_profile_code = contentModel.generalHeader.cleansingId;
                        account.pfc_polisy_client_id = contentModel.generalHeader.polisyClientId;
                        // account.AccountNumber = contentModel.generalHeader.crmPayeeId;

                        // profileHeader
                        //contentModel.profileHeader.corporateName1;
                        //contentModel.profileHeader.corporateName2;
                        account.Name = contentModel.profileHeader.corporateName1 + " " + contentModel.profileHeader.corporateName2;
                        // contentModel.profileHeader.contactPerson; 
                        account.pfc_register_no = contentModel.profileHeader.idRegCorp; // contentModel.profileHeader.idRegCorp;
                        account.pfc_tax_no = contentModel.profileHeader.idTax; // contentModel.profileHeader.idTax;
                        // contentModel.profileHeader.dateInCorporate;
                        account.pfc_tac_branch = contentModel.profileHeader.corporateBranch; // contentModel.profileHeader.corporateBranch;
                        // contentModel.profileHeader.econActivity;
                        // contentModel.profileHeader.countryOrigin;
                        // contentModel.profileHeader.language; // account, contact
                        // contentModel.profileHeader.riskLevel; // contact
                        bool isVIP = false;
                        if (contentModel.profileHeader.vipStatus.Equals("Y"))
                        {
                            isVIP = true;
                        }
                        account.pfc_customer_vip = isVIP; // bool

                        // contactHeader 

                        account.Telephone1 = contentModel.contactHeader.telephone1 + '#' + contentModel.contactHeader.telephone1Ext;
                        account.Telephone2 = contentModel.contactHeader.telephone2 + '#' + contentModel.contactHeader.telephone2Ext;
                        account.Telephone3 = contentModel.contactHeader.telephone3 + '#' + contentModel.contactHeader.telephone3Ext;
                        account.pfc_moblie_phone1 = contentModel.contactHeader.mobilePhone;
                        account.EMailAddress1 = contentModel.contactHeader.emailAddress;
                        account.pfc_line_id = contentModel.contactHeader.lineID;
                        account.pfc_facebook = contentModel.contactHeader.facebook;

                        /*
                        // addressHeader
                        contentModel.addressHeader.address1;
                        contentModel.addressHeader.address2;
                        contentModel.addressHeader.address3;
                        contentModel.addressHeader.subDistrictCode;
                        contentModel.addressHeader.districtCode;
                        contentModel.addressHeader.provinceCode;
                        contentModel.addressHeader.postalCode;
                        contentModel.addressHeader.country;
                        contentModel.addressHeader.addressType;
                        contentModel.addressHeader.latitude;
                        contentModel.addressHeader.longtitude;
                        */

                        /*
                        // asrhHeader
                        contentModel.asrhHeader.assessorOregNum;
                        contentModel.asrhHeader.assessorDelistFlag;
                        contentModel.asrhHeader.assessorBlackListFlag;
                        contentModel.asrhHeader.assessorTerminateDate;
                        contentModel.asrhHeader.solicitorOregNum;
                        contentModel.asrhHeader.solicitorDelistFlag;
                        contentModel.asrhHeader.solicitorBlackListFlag;
                        contentModel.asrhHeader.solicitorTerminateDate;
                        contentModel.asrhHeader.repairerOregNum;
                        contentModel.asrhHeader.repairerDelistFlag;
                        contentModel.asrhHeader.repairerBlackListFlag;
                        contentModel.asrhHeader.repairerTerminateDate;
                        */

                        //ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                        //{
                        //    Requests = new OrganizationRequestCollection(),
                        //    ReturnResponses = true
                        //};

                        //CreateRequest createClaimReq = new CreateRequest() { Target = contact };
                        //tranReq.Requests.Add(createClaimReq);
                        //createClaimReq = new CreateRequest() { Target = account };
                        //tranReq.Requests.Add(createClaimReq);
                        //ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);

                        CreateRequest createReq = new CreateRequest() { Target = account };
                        CreateResponse res = (CreateResponse)crmSvc.Execute(createReq);
                        //res.id 
                        Microsoft.Xrm.Sdk.Query.ColumnSet colSet = new Microsoft.Xrm.Sdk.Query.ColumnSet();
                        colSet.AddColumn("accountnumber");
                        Entity newAccount = crmSvc.Retrieve(Account.EntityLogicalName, res.id, colSet);

                        dataOutput.crmClientId = newAccount["accountnumber"]?.ToString();
                        dataOutput.code = "200";
                        dataOutput.transactionId = TransactionId;
                        dataOutput.transactionDateTime = DateTime.Now;


                    }
                    // Update Payee Additional Records
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


                    //List<string> crmIdOutput = SearchCrmAccountPayeeId(data.generalHeader.cleansingId);

                    //dataOutput.crmClientId = crmIdOutput[0]; // generate from crm
                    //// dataOutput.data = "Waiting for generating algorithm";
                    ////dataOutput.data = contentModel.generalHeader.crmPayeeId;
                    //dataOutput.code = "200";
                    //dataOutput.transactionId = TransactionId;
                    //dataOutput.transactionDateTime = DateTime.Now;
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

        private List<string> SearchCrmAccountPayeeId(OrganizationServiceProxy crmSvc, string cleansingId)
        {
            List<string> l = new List<string>();
            using (ServiceContext sc = new ServiceContext(crmSvc))
            {

                var accounts = (from a in sc.AccountSet
                                where a.pfc_cleansing_cusormer_profile_code == cleansingId
                                select new Account { AccountNumber = a.AccountNumber }).Take(2);
                if (accounts != null)
                {
                    foreach (Account a in accounts)
                    {
                        l.Add(a.AccountNumber);
                    }
                }
            }
            return l.ToList();
        }
    }
}