using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class PfcSubCategoryDataGateWay : BaseCrmSdkTableGateWay<CaseSubCategoryEntity>
    {
        public PfcSubCategoryDataGateWay()
        {
            //  incidentEntity.pfc_sub_categoryid = new EntityReference("pfc_sub_category", new Guid("92D632D8-29AB-E611-80CA-0050568D1874"));
            //  name
            this.EntityName = "pfc_sub_category";
        }

        public CaseSubCategoryEntity FindByPolisyClientId(string categoryCode)
        {


            var _p = getOrganizationServiceProxy();
            var columnSet = this.GetColumnSet();

            QueryExpression query = new QueryExpression(EntityName)
            {
                ColumnSet = GetColumnSetByAttributes()
            };
            query.Criteria.AddCondition("pfc_sub_category_code", ConditionOperator.Equal, categoryCode);

            EntityCollection result = _p.RetrieveMultiple(query);
            var item = TranformEntityWithAttribute(result[0]);
            item.Id = result[0].Id;
            return item;
        }

        public CaseSubCategoryEntity GetDefault()
        {
            return FindByPolisyClientId("10077508");//10077508 16960851

        }
    }

    public class CaseSubCategoryEntity
    {
        [XrmAttributeMapping("pfc_sub_categoryId", EntityFieldKey.PK)]
        public Guid Id { get; set; }
  
        [XrmAttributeMapping("pfc_sub_category_code")]
        public string Code { get; set; }

        [XrmAttributeMapping("pfc_sub_category_name")]
        public string Name { get; set; }

        [XrmAttributeMapping("statuscode")]
        public int Status { get; set; }
      
    }
}