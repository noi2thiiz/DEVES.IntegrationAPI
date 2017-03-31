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

            RegClientPersonalInputModel contentModel = DeserializeJson<RegClientPersonalInputModel>(input.ToString());

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
                    crmSvc.EnableProxyTypes();

                    Contact contact = new Contact();

                    /*
                    // generalHeader
                    contentModel.generalHeader.roleCode; // contact
                    contentModel.generalHeader.cleansingId; // account, contact
                    contentModel.generalHeader.polisyClientId; // account
                    contentModel.generalHeader.crmClientId; // account, contact
                    contentModel.generalHeader.clientAdditionalExistFlag;

                    // profileInfo
                    contentModel.profileInfo.salutation; // account, contact
                    contentModel.profileInfo.personalName; // contact
                    contentModel.profileInfo.personalSurname; // contact
                    contentModel.profileInfo.sex;
                    contentModel.profileInfo.idCitizen; // contact
                    contentModel.profileInfo.idPassport; // contact
                    contentModel.profileInfo.idAlien; // contact
                    contentModel.profileInfo.idDriving; // contact
                    contentModel.profileInfo.birthDate; // contact
                    contentModel.profileInfo.nationality; // account, contact
                    contentModel.profileInfo.language; // account, contact
                    contentModel.profileInfo.married; // contact
                    contentModel.profileInfo.occupation;
                    contentModel.profileInfo.riskLevel; // contact
                    contentModel.profileInfo.vipStatus; // contact
                    contentModel.profileInfo.remark;

                    // contactInfo 
                    contentModel.contactInfo.telephone1; // account, contact
                    contentModel.contactInfo.telephone1Ext; // account, contact
                    contentModel.contactInfo.telephone2; // account , contact
                    contentModel.contactInfo.telephone2Ext; // account, contact
                    contentModel.contactInfo.telephone3; // account, contact
                    contentModel.contactInfo.telephone3Ext; // account, contact
                    contentModel.contactInfo.mobilePhone; // account, contact
                    contentModel.contactInfo.fax; // account, contact
                    contentModel.contactInfo.emailAddress; // account, contact
                    contentModel.contactInfo.lineID; // account, contact
                    contentModel.contactInfo.facebook; // account, contact

                    // addressInfo
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
                    ExecuteTransactionResponse tranRes = (ExecuteTransactionResponse)crmSvc.Execute(tranReq);

                }
            }
            else // Means List crmData is not empty (1 or many)
            {
                CRMRegClientPersonalOutputDataModel output = new CRMRegClientPersonalOutputDataModel();
                output.cleansingId = contentModel.generalHeader.cleansingId;
                output.polisyClientId = contentModel.generalHeader.polisyClientId;
                output.crmClientId = contentModel.generalHeader.crmClientId;
                output.personalName = contentModel.profileInfo.personalName;
                output.personalSurname = contentModel.profileInfo.personalSurname;
            }

            return dataOutput;
        }
    }
}