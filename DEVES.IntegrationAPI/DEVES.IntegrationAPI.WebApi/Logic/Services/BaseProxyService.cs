using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class BaseProxyService
    {
        private string GetEwiUsername()
        {

            return AppConfig.Instance.Get("EWI_USERNAME") ?? "sysdynamic";
        }
        private string GetEwiPassword()
        {

            return AppConfig.Instance.Get("EWI_PASSWORD") ?? "SzokRk43cEM=";
        }

        private string GetEwiUid()
        {

            return AppConfig.Instance.Get("EWI_UID") ?? "DevesClaim";
        }
        private string GetEwiGid()
        {

            return AppConfig.Instance.Get("EWI_GID") ?? "DevesClaim";
        }

        public RESTClientResult SendRequest(object JSON,string endpoint)
        {
            EWIRequest reqModel = new EWIRequest()
            {
                //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                username = GetEwiUsername(),
                password = GetEwiPassword(),
                uid = GetEwiUid(),
                gid = GetEwiGid(),
                token = "",
                content = JSON

            };

            
            var client = new RESTClient(endpoint);
            var result = client.Execute(reqModel);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }

            return result;
        }
    }
}