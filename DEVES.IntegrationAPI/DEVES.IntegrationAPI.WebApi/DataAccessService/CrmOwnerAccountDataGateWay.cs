using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//http://www.magnetismsolutions.com/blog/roshanmehta/2012/04/30/dynamics_crm_2011_querying_data_with_queryexpression
namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class CrmOwnerAccountDataGateWay : BaseCrmSdkTableGateWay<UserAccountEntity>
    {
        public CrmOwnerAccountDataGateWay()
        {
            this.EntityName = "systemuser";
        }

        public UserAccountEntity FindByDomainName(string domainName)
        {
            if (domainName.Substring(0, 3).ToUpper() != "DVS")
            {
                domainName = $"DVS\\{domainName}";
            }

            var _p = getOrganizationServiceProxy();
            var columnSet = this.GetColumnSet();

            QueryExpression query = new QueryExpression(EntityName)
            {
                ColumnSet = GetColumnSetByAttributes()
            };
            query.Criteria.AddCondition("domainname",ConditionOperator.Equal, domainName);
         
            EntityCollection result = _p.RetrieveMultiple(query);
            var item = TranformEntityWithAttribute(result[0]);
            item.Id = result[0].Id;
            return item;
        }

        public UserAccountEntity GetDefault()
        {
            return FindByDomainName(@"sasipa.b");
          
        }
    }

    

    public class UserAccountEntity {
        //Guid Id { get; set; }

        [XrmAttributeMapping("systemuserid", EntityFieldKey.PK)]
        public Guid Id { get; set; }
        [XrmAttributeMapping("domainname")]
        public string DomainName { get; set; }
        [XrmAttributeMapping("firstname")]
        public string FirstName { get; set; }
        [XrmAttributeMapping("fullname")]
        public string FullName { get; set; }
        [XrmAttributeMapping("lastname")]
        public string LastName { get; set; }
        [XrmAttributeMapping("internalemailaddress")]
        public string MobilephoneNumber { get; set; }
        [XrmAttributeMapping("address1_telephone1")]
        public string TelephoneNumber { get; set; }

        [XrmAttributeMapping("internalemailaddress")]
        public string InternalEmailaddress { get; set; }
        
          

    }

   
}