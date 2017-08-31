using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Threading;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using WebBackgrounder;

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
                ServiceContext svcContext;
                var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

                string backDay =(!string.IsNullOrEmpty(AppConfig.Instance.Get("SMS_ASSESSMENT_BACK_DAY"))) ? AppConfig.Instance.Get("SMS_ASSESSMENT_BACK_DAY") : "30";
                string smsUrl  = (!string.IsNullOrEmpty(AppConfig.Instance.Get("SMS_ASSESSMENT_URL"))) ? AppConfig.Instance.Get("SMS_ASSESSMENT_URL") : "https://csat-qa.deves.co.th/assessment";  

                var result = SpQueryGarageAssessmentFromLocus.Instance.Excecute(new Dictionary<string, string> { { "BACK_DAY", backDay } });
                if (result != null)
                {
                    output.data.success = true;

                    if (result.Data.Any())
                    {
                        foreach (var item in result.Data)
                        {
                            Console.WriteLine("=================foreach==============");
                            output.data.totalRecord += 1;
                            try
                            {
                                var model = SpQueryGarageAssessmentFromLocus.Instance.Tranform(item);

                                var assessmentOwnerGuid = (!string.IsNullOrEmpty(model.AssessmentOwnerGuid))
                                    ? model.AssessmentOwnerGuid
                                    : "72FA6F77-5451-E711-80DA-0050568D615F";

                                Console.WriteLine("===================JSON======================");
                                Console.WriteLine(model.ToJson());

                                Console.WriteLine("===================ADD Raw======================");
                                output.data.raw.Add(model);

                                // string MobileNumber = "" + model?.DriverMobile;
                                //         Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                                //       MobileNumber = rgx.Replace(MobileNumber, "");

                                // if (MobileNumber.Length==10)
                                //{
                                Console.WriteLine("set pfc_assessment");
                                var assessment = new pfc_assessment
                                    {

                                        pfc_incidentId = new EntityReference("incident", new Guid(model.Id)),
                                        pfc_ticketnumber = model?.TicketNumber ?? "",
                                        pfc_claim_noti_number = model?.ClaimNotiNumber,

                                        pfc_assessment_ref_code = model.AssessmentRefCode ?? "",
                                        pfc_assessment_questionnaireid = new EntityReference("pfc_questionnair", new Guid(model?.QuestionGuid)),
                                        pfc_assessment_survey_sms = new OptionSetValue(model.SurveySmsOption),
                                        pfc_assessment_expected_to_call_date = DateTime.Now,
                                        pfc_assessment_type = new OptionSetValue(100000001),
                                        pfc_assessment_sms_name = model?.DriverFullname ?? "",
                                        pfc_assessment_sms_number = model?.DriverMobile,
                                        pfc_assessment_sms_url = $"{smsUrl}/?ref=" + model?.AssessmentRefCode + "2",
                                        pfc_assessment_user_url = $"{smsUrl}/?ref=" + model?.AssessmentRefCode + "2",
                                        pfc_assessment_status = new OptionSetValue(100000000),
                                        pfc_assessment_garage_status = new OptionSetValue(100000000),
                                        pfc_assessee_code = model?.AssesseeCode ?? "",
                                        pfc_assessee_name = model?.AssesseeName ?? "",
                                        OwnerId = new EntityReference("team", new Guid(assessmentOwnerGuid)),

                                    };
                                //Optional
                                Console.WriteLine("Set pfc_assessment_policy_additionalId");
                                if (string.IsNullOrEmpty(model?.PolicyGuid))
                                    {
                                        assessment.pfc_assessment_policy_additionalId =
                                            new EntityReference("pfc_policy_addtional", new Guid(model?.PolicyGuid));
                                    }

                                    _serviceProxy.Create(assessment);
                                     Console.WriteLine("Createed" + model?.ClaimNotiNumber);
                                    AddDebugInfo("Createed" + model?.ClaimNotiNumber);
                                // }

                            }
                            catch (Exception e)
                            {
                                output.description += (e.Message + e.StackTrace);
                                Console.WriteLine("Exception" + e.Message + e.StackTrace);
                                AddDebugInfo("Exception"+ e.Message + e.StackTrace);
                            }

                        }
                    }

                }


                output.code = AppConst.CODE_SUCCESS;
                output.message = AppConst.MESSAGE_SUCCESS;
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
                return output;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
                output.code = AppConst.CODE_FAILED;
                output.message += e.Message;
                output.description += e.StackTrace;
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
                
                return output;
            }


        }
    }

  
}