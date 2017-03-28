using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class EWIResponseWrapper: BaseEWIResponse
    {
        public object content { set; get; }

        public EWIResponseWrapper( EWIRequest req , BaseContentOutputModel contentModel)
        {
            this.content = content;
            this.username = req.username;
            this.uid = req.uid;
            this.gid = req.gid;
            if (contentModel.code == "200")
            {
                this.success = true;
            }
            else
            {
                this.success = false;
            }

        }
    }
}