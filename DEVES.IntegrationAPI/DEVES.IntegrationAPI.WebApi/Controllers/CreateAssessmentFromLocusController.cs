using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/CreateAssessmentFromLocus")]
    public class CreateAssessmentFromLocusController:BaseApiController
    {
        [HttpPost]
        [Route("")]
        public object Post([FromBody]object value)
        {

            return ProcessRequest<buzCreateAssessmentFromLocus, CreateAssessmentFromLocusInputModel>
                (value, "CreateAssessmentFromLocus_Input_Schema.json");


        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var cmd = new buzCreateAssessmentFromLocus();
            cmd.TransactionId = GetTransactionId();
            var result = (BaseContentJsonProxyOutputModel)cmd.Execute(new CreateAssessmentFromLocusInputModel
            {
                requestId = "api"
            });

            return Ok(result);


        }

        private static bool _isNotSsl;
        private static bool _isStart;
        [HttpGet]
        [Route("invork")]
        public IHttpActionResult  Invork()
        {
            var virtualPath = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetVirtualPath() ?? "";
            _isNotSsl = !HttpContext.Current.Request.Url.AbsoluteUri.Contains("https");
            if (_isStart != true)
            {
                // ให้ทำงานแค่เรื่องเดียว ไม่งั้นจะเกิดการสร้าง assessment ซ้ำซ้อน
                //start log job
                // start เครื่องเดียว
                if (Environment.MachineName == AppConst.PRO2_SERVER_NAME || !AppConst.IS_SERVER)
                {

                   
                    if (_isNotSsl)
                    {
                        if (HttpContext.Current.Request.Url.Host == "localhost")
                        {
                            AssessmentJobHandle.Start();
                            _isStart = true;
                        }
                        else if (HttpContext.Current.Request.Url.Host == "192.168.8.121" && virtualPath?.ToLower() == "/xrmapi")
                        {
                            AssessmentJobHandle.Start();
                            _isStart = true;
                        }
                        else if (HttpContext.Current.Request.Url.Host == "api.deves.co.th" && virtualPath?.ToLower() == "/claim-service")
                        {
                            AssessmentJobHandle.Start();
                            _isStart = true;
                        }
                    }


                }

                //_isStart = true;
               
            }
            

            return Ok(new OutputGenericDataModel<object>
            {
                code = AppConst.CODE_SUCCESS,
                message = AppConst.MESSAGE_SUCCESS,
                transactionDateTime = DateTime.Now,
                transactionId = GetTransactionId(),
                data = new
                {
                    _isStart,
                    host =  HttpContext.Current.Request.Url.Host,
                    virtualPath= virtualPath?.ToLower(),
                    _isNotSsl,
                    machineName=Environment.MachineName,
                    absoluteUri= HttpContext.Current.Request.Url.AbsoluteUri
                }
            });

        }


    }
}