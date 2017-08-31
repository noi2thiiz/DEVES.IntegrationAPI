using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCOMPInquiryClientMasterContentClientListModel_to_CLIENTUpdateCorporateClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            COMPInquiryClientMasterClientModel src = ((COMPInquiryClientMasterContentClientListModel)input).clientList;
            CLIENTUpdateCorporateClientAndAdditionalInfoInputModel trgt = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)output;

            if (src != null)
            {
                trgt.cleansingId = src.cleansingId ;
                trgt.assessorFlag = src.assessorFlag ;
                trgt.solicitorFlag = src.solicitorFlag ;
                trgt.repairerFlag = src.repairerFlag ;
                trgt.hospitalFlag = src.hospitalFlag ;

                trgt.corporateName1 = src.name1;
                trgt.corporateName2 = src.name2 ;
                trgt.remark = src.remark ;
                trgt.idCard = src.idCard ;
                trgt.taxId = src.taxId ;

                trgt.dateInCorporate = src.dateInCorporate;
                trgt.corporateStaffNo = src.corporateStaffNo ;
                trgt.econActivity = src.econActivity ;
                trgt.countryOrigin = src.countryOrigin ;
                trgt.language = src.language ;
                trgt.riskLevel = src.riskLevel ;
                trgt.vipStatus = src.vipStatus ;

                trgt.telephones = src.telephone1 ;
                trgt.telephone2 = src.telephone2 ;
                trgt.telex = src.telex ;
                trgt.telegram = src.mobilePhone ;
                trgt.facsimile = src.fax ;
                trgt.emailAddress = src.emailAddress ;
                trgt.lineId = src.lineId ;
                trgt.facebook = src.facebook ;

                trgt.address1 = src.address1 ;
                trgt.address2 = src.address2 ;
                trgt.address3 = src.address3 ;
                trgt.address4 = src.address4 ;
                trgt.address5 = src.address5 ;
                trgt.postCode = src.postCode ;
                trgt.country = src.country ;
                trgt.latitude = src.latitude ;
                trgt.longtitude = src.longtitude;

                trgt.assessorOregNum = src.assessorOregNum ;
                trgt.assessorBlackListFlag = src.assessorBlackListFlag ;
                trgt.assessorDelistFlag = src.assessorDelistFlag ;
                trgt.assessorTerminateDate = src.assessorTerminateDate ;

                trgt.repairerTerminateDate = src.repairerTerminateDate ;
                trgt.repairerBlackListFlag = src.repairerBlackListFlag ;
                trgt.repairerOregNum = src.repairerOregNum ;
                trgt.repairerDelistFlag = src.repairerDelistFlag ;

                trgt.solicitorOregNum = src.solicitorOregNum ;
                trgt.solicitorBlackListFlag = src.solicitorBlackListFlag ;
                trgt.solicitorDelistFlag = src.solicitorDelistFlag ;
                trgt.solicitorTerminateDate = src.solicitorTerminateDate ;

                trgt.fao = src.fao;
                trgt.clientStatus = src.clientStatus;
                trgt.sTax = src.sTax;
                trgt.capital = src.capital;
                trgt.mailing = src.mailing;
                trgt.directMail = src.directMail;
                trgt.taxInNumber = src.taxIdNumber;
                trgt.specialIndicator = src.specialIndicator;

                trgt.passportId = src.passportId;
                trgt.alientId = src.alientId;
                trgt.driverlicense = src.driverlicense;

                trgt.clientNumber = src.clientNumber; 
            }

            return trgt;
        }
    }
}