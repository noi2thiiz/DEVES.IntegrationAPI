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
        public override BaseDataModel Execute(object input)
        {
            //+ Deserialize Input
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
            BaseCommand cmd = new NullCommand();

            if (contentModel.conditionHeader.roleCode == "G")
            { 
                cmd = new BuzInquiryCrmGeneralClient();
                cmd.TransactionId = TransactionId;
            }
            else /*ASRH*/
            {
                cmd = new BuzInquiryCrmAsrhClientMaster();
                cmd.TransactionId = TransactionId;
            }
            
            return  cmd.Execute(input);
          
        }
    }
}