using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformEWIResCOMPInquiryClientMasterContentModel_to_CLIENTUpdateCorporateClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            EWIResCOMPInquiryClientMasterContentModel src = (EWIResCOMPInquiryClientMasterContentModel)input;
            CLIENTUpdateCorporateClientAndAdditionalInfoInputModel trgt = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)output;


            if (src.clientListCollection.Count>1)
            {
                throw new Exception("Queries returning multiple result sets in Tranform EWIResCOMPInquiryClientMasterContentModel to CLIENTUpdateCorporateClientAndAdditionalInfoInputModel");
            }

            if (src.clientListCollection.Count==1)
            {
                var data = src.clientListCollection[0].clientList;

                trgt.telephones = data.telephone1;
                //"telephones2": "0833333333",
                trgt.telephones2 = data.telephone2;
            //"remark": "ทดสอบผ่าน Service Proxy Fulfill Create and Update Corporate",
                trgt.remark = data.remark;
            //"address1": "43/21 ม.7",
                trgt.address1 = data.address1;
             //"address2": "เขตท่าจันทร์",
                trgt.address2 = data.address2;
            //"specialIndicator": "",
                trgt.specialIndicator = data.specialIndicator;
            //"address3": "แขวงกรุงธน",
                trgt.address3 = data.address3;
             //"capital": "สมุทรปราการ",
                trgt.capital = data.capital;
            //"hospitalFlag": "Y",
                trgt.hospitalFlag = data.hospitalFlag;
            //"facebook": "https://www.facebook.com/iknowyou5678",
                trgt.facebook = data.facebook;
            //"solicitorBlackListFlag": "Y",
                trgt.solicitorBlackListFlag = data.solicitorBlackListFlag;
            //"solicitorDelistFlag": "Y",
                trgt.solicitorDelistFlag = data.solicitorDelistFlag;
            //"corporateStaffNo": "96",
                trgt.corporateStaffNo = data.corporateStaffNo;
            //"vipStatus": "Y",
                trgt.vipStatus = data.vipStatus;
            //"passportId": "4447889211",
                trgt.passportId = data.passportId;
            //"emailAddress": "iknowyou5678@gmail.com",
                trgt.emailAddress = data.emailAddress;
            //"fao": "0233333333",
                trgt.fao = data.fao;
            //"telegram": "0233333333",
                trgt.telegram = data.telegram;
            //"sTax": "0233333333",
                trgt.sTax = data.sTax;
                //"country": "กรุงเทพมหานคร",
                trgt.country = data.country;
            //"repairerTerminateDate": "Y",
                trgt.repairerTerminateDate = data.repairerTerminateDate;
            //"solicitorOregNum": "96",
                trgt.solicitorOregNum = data.solicitorOregNum;
            //"taxId": "559897",
                trgt.taxId = data.taxId;
            //"longtitude": "144770.0",
                trgt.longtitude = data.longtitude;
            //"assessorDelistFlag": "Y",
                trgt.assessorDelistFlag = data.assessorDelistFlag;
            //"directMail": "isaidiknowyou5678@yahoo.com",
                trgt.directMail = data.directMail;
            //"language": "T",
                trgt.language = data.language;
            //"latitude": "1.3554787",
                trgt.latitude = data.latitude;
            //"corporateName2": "Corporate Proxy",
                trgt.corporateName2 = data.name2;
            //"corporateName1": "Case 2 Create and Update Corporate JSON",
                trgt.corporateName1 = data.name1;
            //"dateInCorporate": "04042016",
                trgt.dateInCorporate = null;
            //"mailing": "isaidyouknowme@hotmail.com",
                trgt.mailing = data.mailing;
            //"riskLevel": "R3",
                trgt.riskLevel = data.riskLevel;
            //"assessorOregNum": "96",
                trgt.assessorOregNum = data.assessorOregNum;
             //"telex": "023333333",
                trgt.telex = data.telex;
           
            //"repairerFlag": "Y",
                trgt.repairerFlag = data.repairerFlag;
            //"clientNumber": "16960644",
                trgt.clientNumber = data.clientNumber;
            //"assessorTerminateDate": "04042016",
                trgt.assessorTerminateDate = data.assessorTerminateDate;
            //"repairerOregNum": "96",
                trgt.repairerOregNum = data.repairerOregNum;
            //"repairerDelistFlag": "Y",
                trgt.repairerDelistFlag = data.repairerDelistFlag;
            //"countryOrigin": "N",
                trgt.countryOrigin = data.countryOriginText;
            //"lineId": "789410",
                trgt.lineId = data.lineId;
            //"assessorBlackListFlag": "Y",
                trgt.assessorBlackListFlag = data.assessorBlackListFlag;
            //"idCard": "1101500772509",
                trgt.idCard = data.idCard;
            //"facsimile": "15000",
                trgt.facsimile = data.facsimile;
             //"clientStatus": "Y",
                trgt.clientStatus = data.clientStatus;
            //"postCode": "10530",
                trgt.postCode = data.postCode;
             //"repairerBlackListFlag": "Y",
                trgt.repairerBlackListFlag = data.repairerBlackListFlag;
            //"econActivity": "Y",
                trgt.econActivity = data.econActivity;
             //"checkFlag": "UPDATE",
                trgt.checkFlag = "UPDATE";
            //"cleansingId": "CLS-33333",
                trgt.cleansingId = data.cleansingId;
             //"address5": "10530",
                trgt.address5 = data.address5;
            //"solicitorFlag": "Y",
                trgt.solicitorFlag = data.solicitorFlag;
             //"address4": "ประเทศไทย",
                trgt.address4 = data.address4;
            //"alientId": "-",
                trgt.alientId = data.alientId;
             //"taxInNumber": "96",
                trgt.taxInNumber = data.taxIdNumber;
            //"driverlicense": "96",
                trgt.driverlicense = data.driverlicense;
             //"assessorFlag": "Y",
                trgt.assessorFlag = data.assessorFlag;
            //"solicitorTerminateDate": "04042016"
                trgt.solicitorTerminateDate = data.solicitorTerminateDate;
            }

           

           

            return trgt;
        }
    }
}