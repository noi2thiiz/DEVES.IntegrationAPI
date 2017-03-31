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

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCreateCrmClientCorporate : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            RegClientCorporateInputModel contentModel = DeserializeJson<RegClientCorporateInputModel>(input.ToString());

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

            RegClientCorporateOutputModel dataOutput = new RegClientCorporateOutputModel();
            dataOutput.data = new List<RegClientCorporateDataOutputModel_Pass>();
            if (crmData.Count == 0) // Means List crmData is empty
            {
                // Create new one and map input value to CRM
                using (OrganizationServiceProxy crmSvc = GetCrmServiceProxy())
                {
                    crmSvc.EnableProxyTypes();

                    Contact contact = new Contact();

                    /*
                    // generalHeader
                    contentModel.generalHeader.roleCode; // contact
                    contentModel.generalHeader.cleansingId; // account, contact
                    contentModel.generalHeader.polisyClientId; // account
                    contentModel.generalHeader.crmClientId; // account, contact
                    contentModel.generalHeader.clientAdditionalExistFlag;
                    contentModel.generalHeader.assessorFlag;
                    contentModel.generalHeader.solicitorFlag;
                    contentModel.generalHeader.repairerFlag;
                    contentModel.generalHeader.hospitalFlag;

                    // profileHeader
                    contentModel.profileHeader.corporateName1;
                    contentModel.profileHeader.corporateName2;
                    contentModel.profileHeader.contactPerson; 
                    contentModel.profileHeader.idRegCorp;
                    contentModel.profileHeader.idTax;
                    contentModel.profileHeader.dateInCorporate;
                    contentModel.profileHeader.corporateBranch;
                    contentModel.profileHeader.econActivity;
                    contentModel.profileHeader.countryOrigin;
                    contentModel.profileHeader.language; // account, contact
                    contentModel.profileHeader.riskLevel; // contact
                    contentModel.profileHeader.vipStatus; // contact

                    // contactHeader 
                    contentModel.contactHeader.telephone1; // account, contact
                    contentModel.contactHeader.telephone1Ext; // account, contact
                    contentModel.contactHeader.telephone2; // account , contact
                    contentModel.contactHeader.telephone2Ext; // account, contact
                    contentModel.contactHeader.telephone3; // account, contact
                    contentModel.contactHeader.telephone3Ext; // account, contact
                    contentModel.contactHeader.mobilePhone; // account, contact
                    contentModel.contactHeader.fax; // account, contact
                    contentModel.contactHeader.emailAddress; // account, contact
                    contentModel.contactHeader.lineID; // account, contact
                    contentModel.contactHeader.facebook; // account, contact

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
                    ExecuteTransactionRequest tranReq = new ExecuteTransactionRequest()
                    {
                        Requests = new OrganizationRequestCollection(),
                        ReturnResponses = true
                    };

                    CreateRequest createClaimReq = new CreateRequest() { Target = contact };
                    tranReq.Requests.Add(createClaimReq);
                    ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);

                }
            }
            else // Means List crmData is not empty (1 or many)
            {
                RegClientCorporateDataOutputModel_Pass output = new RegClientCorporateDataOutputModel_Pass();
                output.cleansingId = contentModel.generalHeader.cleansingId;
                output.polisyClientId = contentModel.generalHeader.polisyClientId;
                output.crmClientId = contentModel.generalHeader.crmClientId;
                output.corporateName1 = contentModel.profileHeader.corporateName1;
                output.corporateName2 = contentModel.profileHeader.corporateName2;
                output.corporateBranch = contentModel.profileHeader.corporateBranch;
            }
            return dataOutput;
        }
    }
}