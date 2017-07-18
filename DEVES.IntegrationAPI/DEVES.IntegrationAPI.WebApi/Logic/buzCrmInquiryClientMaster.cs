﻿using System;
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
       
        /*
        public override BaseDataModel ExecuteNew(object input)
        {
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
            BaseCommand cmd = new NullCommand();

            if (contentModel.conditionHeader.roleCode == "G")
            { 
                cmd = new BuzInquiryCrmGeneralClient();
                cmd.TransactionId = TransactionId;
            }
            else 
            {
                cmd = new BuzInquiryCrmAsrhClientMaster();
                cmd.TransactionId = TransactionId;
            }
            
            return  cmd.Execute(input);
          
        }
       */
        public override BaseDataModel Execute(object input)
        {
            //+ Deserialize Input
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
            BaseCommand cmd = new NullCommand();
            switch (contentModel.conditionHeader.clientType)
            {
                case "P":
                    cmd = new buzCrmInquiryPersonalClientMaster();
                    cmd.TransactionId = TransactionId;
                    break;
                case "C":
                    cmd = new buzCrmInquiryCorporateClientMaster();
                    cmd.TransactionId = TransactionId;
                    break;
                default:
                    break;
            }


            return cmd.Execute(input);
        }
    }
}