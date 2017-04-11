using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataGateway
{
    public class CorperateDataGateWay : BaseCrmSdkTableGateWay<CorperateEntity>
    {
        public CorperateDataGateWay()
        {
            //name
            this.EntityName = "account";
        }
        public CorperateEntity FindByPolisyClientId(string polisyClientId)
        {
            

            var _p = getOrganizationServiceProxy();
            var columnSet = this.GetColumnSet();

            QueryExpression query = new QueryExpression(EntityName)
            {
                ColumnSet = GetColumnSetByAttributes()
            };
            query.Criteria.AddCondition("pfc_polisy_client_id", ConditionOperator.Equal, polisyClientId);

            EntityCollection result = _p.RetrieveMultiple(query);
            var item = TranformEntityWithAttribute(result[0]);
            item.Id = result[0].Id;
            return item;
        }

        public CorperateEntity GetDefault()
        {
            return FindByPolisyClientId("16960851");//10077508 16960851 //10077508

        }

    }
    //16960851
    public class CorperateEntity
    {
        [XrmAttributeMapping("accountid", EntityFieldKey.PK)]
        public Guid Id { get; set; }
        [XrmAttributeMapping("accountnumber")]
        public string CrmClientId { get; set; }

        [XrmAttributeMapping("name")]
        public string Name { get; set; }

        [XrmAttributeMapping("pfc_polisy_client_id")]
        public string PolisyClientId { get; set; }
        
    }
}