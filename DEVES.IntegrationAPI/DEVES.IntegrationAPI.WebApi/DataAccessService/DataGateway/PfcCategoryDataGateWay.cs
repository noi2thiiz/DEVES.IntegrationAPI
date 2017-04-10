using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class PfcCategoryDataGateWay : BaseCrmSdkTableGateWay<CaseSubCategoryEntity>
    {
        public PfcCategoryDataGateWay()
        {
            // name
            // incidentEntity.pfc_categoryid = new EntityReference("pfc_category", new Guid("1DCB2B21-0AAB-E611-80CA-0050568D1874"));
            this.EntityName = "pfc_category";
        }

        public CaseCategoryEntity FindByCode(string SubCategoryCode)
        {
            try
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class CaseCategoryEntity
    {
        [XrmAttributeMapping("pfc_calltypeId", EntityFieldKey.PK)]
        public Guid Id { get; set; }

        [XrmAttributeMapping("pfc_category_code")]
        public string Code { get; set; }

        [XrmAttributeMapping("pfc_category_name")]
        public string Name { get; set; }

        [XrmAttributeMapping("pfc_calltypeId")]
        public string CallTypeId { get; set; }

        [XrmAttributeMapping("statuscode")]
        public int Status { get; set; }

    }
}