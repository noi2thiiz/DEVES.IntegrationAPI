using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCLSPersonalClientInput : BaseTransformer
    {

        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {

            InquiryClientMasterInputModel src = (InquiryClientMasterInputModel)input;
            CLSInquiryPersonalClientInputModel trgt = (CLSInquiryPersonalClientInputModel)output;

            TraceDebugLogger.Instance.AddLog("TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCLSPersonalClientInput", input);

            trgt.roleCode = src.conditionHeader.roleCode??"";
            trgt.clientId = src.conditionDetail.polisyClientId ?? "";
            trgt.personalFullName = src.conditionDetail.clientFullname ?? "";
            trgt.idCitizen = src.conditionDetail.idCard ?? "";
            trgt.telephone = "";
            trgt.emailAddress = "";
            trgt.cleansingId = src.conditionDetail.cleansingId ?? "";
           
            trgt.backDay = AppConst.COMM_BACK_DAY.ToString();

            if (string.IsNullOrEmpty(trgt.personalFullName))
            {
                if (string.IsNullOrEmpty(src.conditionDetail.clientName1))
                {
                    trgt.personalFullName = src.conditionDetail.clientName2;
                }
                else 
                {

                    trgt.personalFullName = src.conditionDetail.clientName1;
                }
               
            }

            trgt.personalFullName.ReplaceMultiplSpacesWithSingleSpace();

            return trgt;
        }

    }
}