using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/admin-service")]
    public class AdminServiceController : BaseApiController
    {
        [HttpPost]
        [Route("reload-config")]
        public IHttpActionResult ReloadConfig([FromBody]MyCadentialData securityKey)
        {
            try
            {
                AppConfig.Instance.Reload();
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_SUCCESS,
                    message = AppConst.MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()

                });
            }
            catch (Exception e)
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    stackTrace = e.StackTrace

                });
            }
            
        }

        [HttpGet]
        [Route("getconfigs")]
        public IHttpActionResult GetConfig()
        {
            try
            {
                var config = AppConfig.Instance.GetConfig();
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_SUCCESS,
                    message = AppConst.MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    data = config

                });
            }
            catch (Exception e)
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    stackTrace = e.StackTrace

                });
            }

        }
        [HttpGet]
        [Route("get-application-host")]
        public IHttpActionResult GetApplicationHost()
        {
            try
            {
             
                var data = new Dictionary<string,string>();
                data.Add("VirtualPath", System.Web.Hosting.HostingEnvironment.ApplicationHost.GetVirtualPath());
                data.Add("siteID", System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteID());
                data.Add("Host", HttpContext.Current.Request.Url.Host);
                data.Add("uri", HttpContext.Current.Request.Url.AbsolutePath);
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_SUCCESS,
                    message = AppConst.MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    data = data

                });
            }
            catch (Exception e)
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    stackTrace = e.StackTrace

                });
            }

        }
        
        [HttpPost]
        [Route("change-ewi-configs")]
        public IHttpActionResult ChangeEwiConfigs([FromUri] EwiConfig ewiConfig)
        {
            try
            {
                AppConfig.Instance.UpdateEwiConfig(ewiConfig);
                var config = AppConfig.Instance.GetConfig();
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_SUCCESS,
                    message = AppConst.MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    data = config

                });
            }
            catch (Exception e)
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    stackTrace = e.StackTrace

                });
            }

        }

        [HttpPost]
        [Route("reload-masterData")]
        public IHttpActionResult ReloadMasterData([FromBody]MyCadentialData securityKey)
        {
            try
            {
                CountryMasterData.Instance.InitData();
                NationalityMasterData.Instance.InitData();
                PersonalTitleMasterData.Instance.InitData();
                SubDistrictMasterData.Instance.InitData();
                DistricMasterData.Instance.InitData();
                AddressTypeMasterData.Instance.InitData();
                OccupationMasterData.Instance.InitData();
                ProvinceMasterData.Instance.InitData();
                TypeOfLossMasterData.Instance.InitData();

                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_SUCCESS,
                    message = AppConst.MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()

                });
            }
            catch (Exception e)
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId(),
                    stackTrace = e.StackTrace

                });
            }
        }
       

    }

    public class MyCadentialData
    {
        public string SecurityKey { get; set; }
    }

    public class EwiConfig: MyCadentialData
    {
       
        
        public string EWI_USERNAME { get; set; }
        public string EWI_PASSWORD { get; set; }
        public string EWI_GID { get; set; }
        public string EWI_UID { get; set; }
        public string EWI_HOSTNAME { get; set; }

    }
}