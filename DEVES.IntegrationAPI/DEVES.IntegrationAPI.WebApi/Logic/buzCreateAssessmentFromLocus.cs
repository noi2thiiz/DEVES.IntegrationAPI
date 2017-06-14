using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCreateAssessmentFromLocus:BuzCommand
    {
       
        public override BaseDataModel ExecuteInput(object input)
        {
            // Preparation Variable
            CreateAssessmentFromLocusOutputModel output = new CreateAssessmentFromLocusOutputModel();
            try
            {
              
                // Deserialize Input
                // SubmitSurveyAssessmentResultInputModel contentModel = DeserializeJson<SubmitSurveyAssessmentResultInputModel>(input.ToString());
                CreateAssessmentFromLocusInputModel inputModel = (CreateAssessmentFromLocusInputModel)input;


                // Connect SDK 
                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var result = SpQueryGarageAssessmentFromLocus.Instance.Excecute(new Dictionary<string, string> { { "BACK_DAY", "30" } });
                var smsUrl = "https://csat-qa.deves.co.th/assessment"; 
                //ConfigurationManager.AppSettings["SMS_ASSESSMENT_URL"].ToString();
                if (result.Data.Any())
                {
                    foreach (var item in result.Data)
                    {
                       var model =  SpQueryGarageAssessmentFromLocus.Instance.Tranform(item);

                        
                        Console.WriteLine(model.ToJson());
                        var assessment = new pfc_assessment
                        {
                            pfc_incidentId = new EntityReference("incident", new Guid(model.Id)),
                            pfc_ticketnumber = model?.TicketNumber??"",
                            pfc_claim_noti_number = model?.ClaimNotiNumber,
                            pfc_assessment_ref_code =model.AssessmentRefCode??"",
                            pfc_assessment_questionnaireid= new EntityReference("pfc_questionnair", new Guid(model?.QuestionGuid)),
                            pfc_assessment_survey_sms = new OptionSetValue(100000002),
                            pfc_assessment_expected_to_call_date = DateTime.Now,
                            pfc_assessment_type = new OptionSetValue(100000001),
                            pfc_assessment_sms_name = model?.DriverFullname??"",
                            pfc_assessment_sms_number = model?.DriverMobile,
                            pfc_assessment_sms_url = $"{smsUrl}/?ref=" + model?.AssessmentRefCode+"2",
                            pfc_assessment_user_url = $"{smsUrl}/?ref=" + model?.AssessmentRefCode+"2",
                            pfc_assessment_status = new OptionSetValue(100000000),
                            pfc_assessment_garage_status = new OptionSetValue(100000000),


                        };
                        _serviceProxy.Create(assessment);
                    }
                }
              //  var assessment = new pfc_assessment();
             
                //

                output.code = AppConst.CODE_SUCCESS;
                output.message = AppConst.MESSAGE_SUCCESS;
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
                return output;

            }
            catch (Exception e)
            {
                output.code = AppConst.CODE_FAILED;
                output.message = e.Message;
                output.description = e.StackTrace;
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
                return output;
            }


        }
    }
}