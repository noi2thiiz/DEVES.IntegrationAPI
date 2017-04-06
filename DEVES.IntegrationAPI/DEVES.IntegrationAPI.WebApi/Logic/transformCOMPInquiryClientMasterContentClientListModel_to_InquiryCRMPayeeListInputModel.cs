using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class transformCOMPInquiryClientMasterContentClientListModel_to_InquiryCRMPayeeListInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            COMPInquiryClientMasterContentClientListModel inp = (COMPInquiryClientMasterContentClientListModel)input;
            InquiryCRMPayeeListInputModel oup ;

            if (output == null)
                oup = new InquiryCRMPayeeListInputModel();
            else
                oup = (InquiryCRMPayeeListInputModel)output;

            oup.assessorFlag = inp.clientList.assessorFlag ?? "";
            oup.emcsCode = "";
            oup.emcsMemId = "";
            oup.emcsMemHeadId = "";
            oup.fullname = inp.clientList.fullName ?? "";
            oup.hospitalFlag = inp.clientList.hospitalFlag ?? "";
            oup.polisyClientId = inp.clientList.clientNumber ?? "";
            oup.repairerFlag = inp.clientList.repairerFlag ?? "";
            oup.requester = "";
            oup.roleCode = "";
            oup.sapVendorCode = "";
            oup.solicitorFlag = inp.clientList.solicitorFlag ?? "";
            oup.taxNo = inp.clientList.taxId ?? "";
            oup.taxBranchCode = inp.clientList.corporateStaffNo ?? "";
            oup.clientType = inp.clientList.clientType ?? "";
            oup.cleansingId = inp.clientList.cleansingId;
            return oup;
        }
    }
}