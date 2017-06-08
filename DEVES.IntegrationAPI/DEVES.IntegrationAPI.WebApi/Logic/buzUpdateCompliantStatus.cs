using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.UpdateCompliantStatus;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzUpdateCompliantStatus : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            // Deserialize Input
            UpdateCompliantStatusInputModel contentInput = (UpdateCompliantStatusInputModel)input;

            // Preparation Variable
            UpdateCompliantStatusOutputModel_Pass output = new UpdateCompliantStatusOutputModel_Pass();

            // Preparation Linq query to CRM
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            var query = from c in svcContext.IncidentSet
                        where c.TicketNumber == contentInput.caseNo
                        select c;

            // After query need to check data
            // Case 1: query doesn't contain data -> return error message
            if (query.FirstOrDefault<Incident>() == null)
            {
                output.code = CONST_CODE_FAILED;
                output.message = "ไม่สามารถ Update ได้";
                output.description = "TicketNumber หรือ tempID ไม่มีในระบบ CRM";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
                output.data = new UpdateCompliantStatusDataOutputModel_Pass();

                return output;
            }
            // Case 2: Contain data -> Update
            else
            {
                Incident incident = query.FirstOrDefault<Incident>();

                Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, incident.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                string check = retrievedIncident.pfc_complaint_temp_id + "";
                try
                {
                    retrievedIncident.pfc_complaint_temp_id = contentInput.tempID;
                    retrievedIncident.pfc_complaint_no = contentInput.complaintNo;
                    switch (contentInput.compliantStatus)
                    {
                        case "1": retrievedIncident.pfc_complaint_status = new Microsoft.Xrm.Sdk.OptionSetValue(1); break;
                        case "2": retrievedIncident.pfc_complaint_status = new Microsoft.Xrm.Sdk.OptionSetValue(2); break;
                        case "3": retrievedIncident.pfc_complaint_status = new Microsoft.Xrm.Sdk.OptionSetValue(3); break;
                    }
                    switch (contentInput.compliantStep)
                    {
                        case "01": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000000); break;
                        case "02": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000001); break;
                        case "31": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000002); break;
                        case "32": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000003); break;
                        case "33": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000004); break;
                        case "34": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000005); break;
                        case "35": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000006); break;
                        case "04": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000007); break;
                        case "05": retrievedIncident.pfc_complaint_step = new Microsoft.Xrm.Sdk.OptionSetValue(100000008); break;
                    }

                    retrievedIncident.pfc_complaint_step_date = Convert.ToDateTime(contentInput.complaintStepdate);


                    output.code = CONST_CODE_SUCCESS;
                    output.message = "Update Complaint Status เรียบร้อยแล้ว";
                    output.description = "";
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now;
                    output.data = new UpdateCompliantStatusDataOutputModel_Pass();
                    output.data.message = "Update Complaint Status เรียบร้อยแล้ว";

                    return output;
                }
                catch (Exception e)
                {
                    output.code = CONST_CODE_FAILED;
                    output.message = "ไม่สามารถ Update ได้";
                    output.description = e.Message;
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now;
                    output.data = new UpdateCompliantStatusDataOutputModel_Pass();

                    return output;
                }
            }

        }


    }
}