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
    [RoutePrefix("api/internal-service")]
    public class InternalServiceController : BaseApiController
    {
        [HttpPost]
        [Route("reload-config")]
        public IHttpActionResult ReloadConfig(CadentialData securityKey)
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
        [HttpPost]
        [Route("reload-masterData")]
        public IHttpActionResult ReloadMasterData(CadentialData securityKey)
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

    public class CadentialData
    {
        private string SecurityKey { get; set; }
    }
}