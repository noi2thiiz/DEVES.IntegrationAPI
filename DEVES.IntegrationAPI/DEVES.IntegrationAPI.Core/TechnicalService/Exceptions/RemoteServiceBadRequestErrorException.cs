﻿namespace DEVES.IntegrationAPI.WebApi.Core.Exceptions
{
    public class RemoteServiceBadRequestErrorException:ServiceFailException
    {
        public RemoteServiceBadRequestErrorException(ServiceFailResult r)
        {
            r.setHeaderProperty("code","404");
            this.Result = r;
        }

    }
}