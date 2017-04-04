using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryClientMasterInputModel src = (InquiryClientMasterInputModel)input;
            CLSInquiryCorporateClientInputModel trgt = (CLSInquiryCorporateClientInputModel)output;

            trgt.roleCode = src.conditionHeader.roleCode;
            trgt.clientId = src.conditionDetail.polisyClientId;
            trgt.corporateFullName = src.conditionDetail.clientFullname;
            trgt.taxNo = src.conditionDetail.idCard;
            trgt.telephone = ""; 
            trgt.emailAddress = "";
            trgt.backDay = "30";

            return output;
        }
    }
}