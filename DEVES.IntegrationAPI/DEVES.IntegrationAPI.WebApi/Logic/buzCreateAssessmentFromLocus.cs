using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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

                string backDay =(!string.IsNullOrEmpty(AppConfig.Instance.Get("SMS_ASSESSMENT_BACK_DAY"))) ? AppConfig.Instance.Get("SMS_ASSESSMENT_BACK_DAY") : "30";
                string smsUrl  = (!string.IsNullOrEmpty(AppConfig.Instance.Get("SMS_ASSESSMENT_URL"))) ? AppConfig.Instance.Get("SMS_ASSESSMENT_URL") : "https://csat-qa.deves.co.th/assessment";  


                var result = SpQueryGarageAssessmentFromLocus.Instance.Excecute(new Dictionary<string, string> { { "BACK_DAY", backDay } });
                

                output.data.success = true;
                
                if (result.Data.Any())
                {
                    foreach (var item in result.Data)
                    {
                        output.data.totalRecord += 1;
                       var model =  SpQueryGarageAssessmentFromLocus.Instance.Tranform(item);

                        var assessmentOwnerGuid = (!string.IsNullOrEmpty(model.AssessmentOwnerGuid))
                            ? model.AssessmentOwnerGuid
                            : "72FA6F77-5451-E711-80DA-0050568D615F";
                       
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
                            pfc_assessee_code = model?.AssesseeCode??"",
                            pfc_assessee_name = model?.AssesseeName ??"",
                            OwnerId = new EntityReference("team", new Guid(assessmentOwnerGuid)),
                            
                           
                         
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

    public static class AssessmentJobHandle
    {

        private static readonly Timer timer = new Timer(OnTimerElapsed);
        private static readonly Timer timerStart = new Timer(StartLoop);
        private static readonly AssessmentJob assessmentJob = new AssessmentJob();
        private static int msUntilTime = 0;


        public static void Start()
        {


            // Figure how much time until 7:00
            double TimeOfExecution = 7;
            var cultureInfo = new CultureInfo("th-TH");

            DateTime now = DateTime.Now;
            Console.WriteLine(now.ToString());
            DateTime sevenOClock = DateTime.Today.AddHours(TimeOfExecution);

            // If it's already past 7:00, wait until 7:00 tomorrow    
            if (now > sevenOClock)
            {
                sevenOClock = sevenOClock.AddDays(1.0);
            }
            Console.WriteLine("Set the timer to" + sevenOClock.ToString(cultureInfo));

            msUntilTime = (int) ((sevenOClock - now).TotalMilliseconds);

            // Set the timer to elapse only once, at 7:00.
            Console.WriteLine("Assessment Job Start");
            SendSmsService.Instance
                .SendMessage("Set the Assessment timer to" + sevenOClock.ToString(cultureInfo),
                    "0943481249");
            timer.Change(msUntilTime, Timeout.Infinite);
            // timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(msUntilTime));


        }

        public static void StartLoop(object sender)
        {

            // Set the timer to elapse only once, at 7:00.
            Console.WriteLine("Assessment Job Start Loop");
            //timer.Change(msUntilTime, Timeout.Infinite);
            timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(msUntilTime));


        }

        private static void processAssessment()
        {
            countTry++;
            if (countTry <= 5)
            {
                try
                {
                    Console.WriteLine("processAssessment:" + countTry);
                    var cmd = new buzCreateAssessmentFromLocus();
                    var result = (BaseContentJsonProxyOutputModel) cmd.Execute(new CreateAssessmentFromLocusInputModel
                    {
                        requestId = "job"
                    });
                    if (result.code != AppConst.CODE_SUCCESS)
                    {
                        processAssessment();
                    }
                }
                catch (Exception e)
                {
                    processAssessment();
                }


            }

        }

        static int countTry = 0;

        private static void OnTimerElapsed(object sender)
        {

            assessmentJob.DoWork(() =>
            {
                processAssessment();
                Console.WriteLine("Assessment Job DoWork");
                SendSmsService.Instance
                    .SendMessage("Assessment Job DoWork",
                        "0943481249");

            });
        }

        public static void Stop()
        {
            assessmentJob.Stop(true);
        }
    }

    public class AssessmentJob : System.Web.Hosting.IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;

        public AssessmentJob()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void DoWork(Action work)
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                work();
            }
        }
    }
}