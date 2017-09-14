using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class transformCLSInquiryPersonalClientOutputModel_to_InquiryCRMPayeeListInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            CLSInquiryPersonalClientOutputModel inp  = (CLSInquiryPersonalClientOutputModel)input;
            InquiryCRMPayeeListInputModel oup;
            if (output == null)
                oup = new InquiryCRMPayeeListInputModel();
            else
                oup = (InquiryCRMPayeeListInputModel)output;
            
            oup.assessorFlag = "";
            oup.clientType = inp.clientType;
          
            oup.emcsCode = "";
            oup.emcsMemId = "";
            oup.emcsMemHeadId =  "";
            oup.fullname = inp.cls_full_name ?? "";
            oup.hospitalFlag = "";
            oup.polisyClientId = inp.clntnum ?? "";
            oup.repairerFlag = "";
            oup.requester = "";
            oup.roleCode = "";
            oup.sapVendorCode = "";
            oup.solicitorFlag = "";
            oup.taxNo = inp.cls_citizen_id_new ?? "";
            oup.taxBranchCode = "";
            oup.cleansingId = inp.cleansing_id ?? "";

            return oup;
        }
    }
}