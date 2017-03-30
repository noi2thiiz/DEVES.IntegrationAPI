using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzInquiryCRMPayeeList : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            buzCrmInquiryClientMaster searchCleansing = new buzCrmInquiryClientMaster();
            BaseContentJsonProxyOutputModel contentSearchCleansing = (BaseContentJsonProxyOutputModel)searchCleansing.Execute(input);



            return contentSearchCleansing;
        }
    }
}