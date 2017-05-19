using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformInquiryCRMPayeeListInputModel_to_CLSInquiryPersonalClientInputModel: BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            CLSInquiryPersonalClientInputModel trgt = (CLSInquiryPersonalClientInputModel)output;

            trgt.clientId = ""+src.polisyClientId;
            trgt.roleCode = ""+src.roleCode;

            trgt.personalFullName = ""+src.fullname;
            trgt.idCitizen = ""+src.taxNo;
            trgt.backDay = "15";
            trgt.telephone="";
            trgt.emailAddress="";

            return trgt;

        }

    }
}