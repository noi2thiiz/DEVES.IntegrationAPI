using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.UpdateClaimNo;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzUpdateClaimNo : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Deserialize Input
            UpdateClaimNoInputModel contentInput = (UpdateClaimNoInputModel)input;

            // Preparation Variable
            UpdateClaimNoOutputModel_Pass output = new UpdateClaimNoOutputModel_Pass();

            // Preparation Linq query to CRM
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            var queryCase = from c in svcContext.IncidentSet
                            where c.TicketNumber == contentInput.ticketNo
                            select c;

        }
    }
}