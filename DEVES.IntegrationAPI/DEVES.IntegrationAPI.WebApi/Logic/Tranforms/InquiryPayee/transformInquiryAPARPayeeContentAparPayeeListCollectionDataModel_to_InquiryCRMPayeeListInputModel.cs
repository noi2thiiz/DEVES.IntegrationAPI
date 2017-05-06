using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class transformInquiryAPARPayeeContentAparPayeeListCollectionDataModel_to_InquiryCRMPayeeListInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryAPARPayeeListModel inp = ((InquiryAPARPayeeContentAparPayeeListCollectionDataModel) input).aparPayeeList;
            InquiryCRMPayeeListInputModel oup = (InquiryCRMPayeeListInputModel)output;
            
            oup.clientType = "C";
            oup.roleCode = "";
            oup.polisyClientId = inp.polisyClntnum;
            oup.sapVendorCode = inp.vendorCode ;
            oup.fullname = inp.fullName;
            oup.taxNo = inp.taxNo;
            oup.taxBranchCode = inp.taxBranchCode;
            oup.requester = "";
            oup.emcsCode = "";

            return oup;
        }
    }
}