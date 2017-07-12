using DEVES.IntegrationAPI.Model.RegOpportunity;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic.Model
{
    public class OpportunityModel
    {
        Opportunity opp;

        object connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
        //OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
        //ServiceContext svcContext = new ServiceContext(_serviceProxy);

        public OpportunityModel()
        {
            opp = new Opportunity();
        }

        public void Create(RegOpportunityInputModel input)
        {

        }

        // Query in XRM from oppId
        public void Retrieve(string oppId)
        {
            
        }

        // Method for save through XRM
        public void Save()
        {

        }

        public void Developed()
        {

        }

        public void Proposed()
        {

        }

        public void Contacted()
        {

        }

        public void CloseAsWon()
        {

        }

        public void CloseAsLoss()
        {

        }

    }
}