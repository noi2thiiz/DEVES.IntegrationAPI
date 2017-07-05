using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegOpportunity;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzRegOpportunity : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            // Preparation Variable
            RegOpportunityOutputModel output = new RegOpportunityOutputModel();
            // output.data = new SubmitSurveyAssessmentResultDataModel_Pass();

            // Deserialize Input
            RegOpportunityInputModel contentModel = (RegOpportunityInputModel)input;

            // Connect SDK and query
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            
            // Inquiry Personal

            // If have only 1 data

            // else (0 data or more than 1)

            

            return output;
        }
    }
}