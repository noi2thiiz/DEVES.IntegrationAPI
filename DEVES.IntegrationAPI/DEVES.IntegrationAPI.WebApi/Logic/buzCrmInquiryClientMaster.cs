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
        public override BaseDataModel Execute(object input)
        {
            //+ Deserialize Input
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
            BaseCommand cmd = new NullCommand();
            switch (contentModel.conditionHeader.clientType)
            {
                case "P":
                    cmd = new buzCrmInquiryPersonalClientMaster();
                    break;
                case "C":
                    if (contentModel.conditionHeader.roleCode == "G")
                    {
                        cmd = new buzCrmInquiryCorporateClientMaster();
                    }
                    else // A, S, R, H
                    {
                        throw new NotImplementedException("CrmInquiryClientMaster for A,S,R,H");
                    }
                    break;
                default:
                    break;
            }

            BaseContentJsonProxyOutputModel res = (BaseContentJsonProxyOutputModel)cmd.Execute(input);
            return res;
        }
    }
}