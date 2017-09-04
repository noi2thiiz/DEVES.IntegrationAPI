using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using Swashbuckle.Swagger.Annotations;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class MasterDataServiceController : ApiController
    {
        [HttpGet]
        [Route("Country")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CountryEntity))]
        public IHttpActionResult Country()
        {
            var data = CountryMasterData.Instance.GetList();
            return Ok(data);
        }


        [HttpGet]
        [Route("AddressType")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AddressTypeEntity))]
        public IHttpActionResult AddressType()
        {
            var data = AddressTypeMasterData.Instance.GetList();
            return Ok(data);
        }


        [HttpGet]
        [Route("Distric")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DistrictEntity))]
        public IHttpActionResult Distric()
        {
            var data = DistricMasterData.Instance.GetList();
            return Ok(data);
        }

        [HttpGet]
        [Route("SubDistrict")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubDistrictEntity))]
        public IHttpActionResult SubDistrict()
        {
            var data = SubDistrictMasterData.Instance.GetList();
            return Ok(data);
        }

        [HttpGet]
        [Route("Province")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProvinceEntity))]
        public IHttpActionResult Province()
        {
            var data = ProvinceMasterData.Instance.GetList();
            return Ok(data);
        }

        [HttpGet]
        [Route("PersonalTitle")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PersonalTitleEntity))]
        public IHttpActionResult PersonalTitle()
        {
            var data = PersonalTitleMasterData.Instance.GetList();
            return Ok(data);
        }

        [HttpGet]
        [Route("Nationality")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(NationalityEntity))]
        public IHttpActionResult Nationality()
        {
            var data = NationalityMasterData.Instance.GetList();
            return Ok(data);
        }

        [HttpGet]
        [Route("Occupation")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OccupationEntity))]
        public IHttpActionResult Occupation()
        {
            var data = OccupationMasterData.Instance.GetList();
            return Ok(data);
        }

    }
}