using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCrmInquiryClientMaster:BaseCommand
    {
        public override BaseContentOutputModel Execute(object input)
        {
            //+ Deserialize Input
            InquiryClientMasterInputModel contentModel = DeserializeJson<InquiryClientMasterInputModel>(input.ToString());
            BaseCommand cmd = new NullCommand();
            switch (contentModel.conditionHeader.clientType)
            {
                case "P":
                    cmd = new buzCrmInquiryPersonalClientMaster();
                    break;
                case "C":
                    cmd = new buzCrmInquiryCorporateClientMaster();
                    break;
                default:
                    break;
            }

            BaseContentOutputModel res = cmd.Execute(input);
            return res;
        }
    }
}