using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class PfcCategoryDataGateWay : BaseCrmSdkTableGateWay<CaseCategoryEntity>
    {
        public PfcCategoryDataGateWay()
        {
            // name
            // incidentEntity.pfc_categoryid = new EntityReference("pfc_category", new Guid("1DCB2B21-0AAB-E611-80CA-0050568D1874"));
            this.EntityName = "pfc_category";
        }

        public CaseCategoryEntity FindByCode(string categoryCode)
        {


            var _p = getOrganizationServiceProxy();
            var columnSet = this.GetColumnSet();

            QueryExpression query = new QueryExpression(EntityName)
            {
                ColumnSet = GetColumnSetByAttributes()
            };
            query.Criteria.AddCondition("pfc_category_code", ConditionOperator.Equal, categoryCode);

            EntityCollection result = _p.RetrieveMultiple(query);
            var item = TranformEntityWithAttribute(result[0]);
            item.Id = result[0].Id;
            return item;
        }

        public CaseCategoryEntity GetDefaultForRVP()
        {
            return FindByCode("0201");//10077508 16960851

        }
    }

    public class CaseCategoryEntity
    {
        [XrmAttributeMapping("pfc_calltypeid", EntityFieldKey.PK)]
        public Guid Id { get; set; }

        [XrmAttributeMapping("pfc_category_code")]
        public string Code { get; set; }

        [XrmAttributeMapping("pfc_category_name")]
        public string Name { get; set; }

        [XrmAttributeMapping("pfc_calltypeid")]
        public string CallTypeId { get; set; }

        [XrmAttributeMapping("statuscode")]
        public int Status { get; set; }

    }
}