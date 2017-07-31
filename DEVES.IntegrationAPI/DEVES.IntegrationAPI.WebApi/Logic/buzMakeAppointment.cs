using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.MakeAppointment;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using System.Configuration;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzMakeAppointment : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            throw new NotImplementedException();

            // Preparation Variable
            MakeAppointmentOutputModel output = new MakeAppointmentOutputModel();

            // Deserialize Input
            MakeAppointmentInputModel contentModel = (MakeAppointmentInputModel)input;

            // Connect SDK
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            // Inquiry regarding

            // if found  refType or regRefId not found 
            /*
            if (found) {
            }
            // return error
            else {
                output.code = AppConst.CODE_FAILED;
                output.message = "ไม่สามารถกำหนด Appointment ได้";
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;

                return output;
            }
            */
        }
    }
}