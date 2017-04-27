using System;
using System.Reflection;
using DEVES.IntegrationAPI.Core.TechnicalService.DataManager;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using DEVES.IntegrationAPI.WebApi.Core.Attributes;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;
using Microsoft.Xrm.Sdk.Query;

namespace DEVES.IntegrationAPI.Core.DataAdepter
{
    public class CrmDataAdepter : IDataAdapter
    {
        protected string ConnectionString;
        protected string EntityName;
        protected XrmAttributeBinder EntityBinder = new XrmAttributeBinder();
       

        protected OrganizationServiceProxy Proxy ;


        protected OrganizationServiceProxy GetOrganizationServiceProxy(string connectionString)
        {
            if (Proxy != null) return Proxy;
            var client = new CrmServiceClient(connectionString);
            Proxy = client.OrganizationServiceProxy;

            return Proxy;
        }

        public CrmDataAdepter(string connectionString)
        {
            ConnectionString = connectionString;
            GetOrganizationServiceProxy(connectionString);



        }

        public Guid Create<TModelClass>(TModelClass entity)
        {
            var info = typeof(TModelClass);
            var attr = info.GetCustomAttribute<XrmEntityMappingAttribute>();
            var xrmEntity = new Entity(attr.EntityName);

            var bindedEntity = EntityBinder.Bind<TModelClass>(xrmEntity);

            var guid = Proxy.Create(bindedEntity);

            return guid;
        }


        public  Entity Find<TModelClass>(Guid guid)
        {
                var reflector = new XrmAttributeReflector();
         
                var attributes = reflector.GetColumnSetByAttributes<TModelClass>();

            Console.WriteLine(attributes.ToJSON());

                    var info = typeof(TModelClass);
                    var attr = info.GetCustomAttribute<XrmEntityMappingAttribute>();
                 EntityName = attr.EntityName;


                var entity = Proxy.Retrieve(EntityName, guid, attributes);
               
                return entity;
          

        }
        public Entity Retrieve(Guid guid, ColumnSet attributes)
        {
           

            var entity = Proxy.Retrieve(EntityName, guid, attributes);
            return entity;
        }


        public DbResult FetchAll(DbRequest req)
        {
            throw new NotImplementedException();
        }

        public DbResult FetchRow(DbRequest req)
        {
            throw new NotImplementedException();
        }

        public DbResult Find(string id)
        {
            throw new NotImplementedException();
        }
    }
}
