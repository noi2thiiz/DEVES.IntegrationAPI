using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using System.Configuration;
using System.ServiceModel.Description;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public abstract class BaseCommand
    {
        private OrganizationServiceProxy _serviceProxy;
        private IOrganizationService _service;

        private Guid CurrentUserId { get; set; }

        private const string CONST_JSON_SCHEMA_FILE = "JSON_SCHEMA_{0}";

        //This is like the Main() function. And need to be implemented.
        public abstract BaseDataModel Execute(object input);


        private bool ValidateJSON(string strJSON)
        {
            string typeName = this.GetType().Name;
            throw new NotImplementedException();
        }

        internal EWIResponseContent CallEWIService(string EWIendpointKey, BaseDataModel JSON , string UID)
        {
            EWIRequest reqModel = new EWIRequest()
            {
                //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                username = "sysdynamic",
                password = "REZOJUNtN04=",
                uid = UID,
                gid = UID,
                token = GetLatestToken(),
                content = JSON
            };

            string jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

            // + ENDPOINT
            string EWIendpoint = GetEWIEndpoint(EWIendpointKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");

            // เช็ค check reponse 
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            EWIResponse ewiRes = response.Content.ReadAsAsync<EWIResponse>().Result;
            EWIResponseContent output = ewiRes.content;
            return output;
        }

        internal T DeserializeJson<T>(string contentText)
        {
            T contentModel;
            string validateResult = string.Empty;
            Type objType = typeof(T);
            string key = string.Format(CONST_JSON_SCHEMA_FILE, objType.Name );
            var filePath = HttpContext.Current.Server.MapPath(GetAppConfigurationSetting(key));

            if (JsonHelper.TryValidateJson(contentText, filePath, out validateResult))
            {
                contentModel = JsonConvert.DeserializeObject<T>(contentText);
                return contentModel;
            }
            else
            {
                throw new JsonSerializationException("Cannot ");
            }
        }


        internal void FillModelUsingSQL(ref BaseDataModel target, string storedProcName, List<CommandParameter> listParams)
        {
            /*
             * For better performance, revise the code to avoid using DataTable
            */
            System.Data.DataTable dt = CallSQLStoredProc(storedProcName, listParams);
            if (dt.Rows.Count > 0)
                FillDataModel(ref target, dt.Rows[0]);
        }

        internal System.Data.DataTable CallSQLStoredProc(string storedProcName, List<CommandParameter> listParams)
        {
            //string strSql = string.Format(SQL_01, ticketNo);
            using (SqlConnection cnnSQL = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString()))
            {

                SqlCommand cmdSQL = new SqlCommand(storedProcName, cnnSQL);
                cmdSQL.CommandTimeout = 600;
                cmdSQL.CommandType = CommandType.StoredProcedure ;
                foreach (CommandParameter p in listParams)
                {
                    cmdSQL.Parameters.Add ( new SqlParameter(p.Name, p.Value));
                }

                cnnSQL.Open();
                SqlDataReader dr = cmdSQL.ExecuteReader();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(dr);
                return dt;
            }
        }

        //internal void FillObjectFromDataTable(ref Object target, DataTable dt)
        //{
        //    Type objType = target.GetType();
        //    if (objType.IsDefined(typeof(CrmClassToMapDataAttribute), false))
        //    {
        //        Object targetChild = target;
        //        FillObjectFromDataTable(ref targetChild, dt);
        //    }
        //    foreach (PropertyInfo prop in objType.GetProperties())
        //    {
                
        //    }
        //}
        static void FillDataModel(ref BaseDataModel node, DataRow dr)
        {

            if (node is BaseDataModel)
            {

                Type nodeType = node.GetType();
                PropertyInfo[] props = nodeType.GetProperties();

                foreach (PropertyInfo prop in props)
                {

                    object propValue = prop.GetValue(node, null);
                    string pValue = propValue == null ? "" : propValue.ToString();

                    CrmMappingAttribute columnMapping = (CrmMappingAttribute)prop.GetCustomAttributes<Attribute>(false).FirstOrDefault<Attribute>(a => a.GetType() == typeof(CrmMappingAttribute));
                    if (columnMapping != null && columnMapping.Source == ENUMDataSource.srcSQL)
                    {
                        Type propType = prop.PropertyType;
                        var underlyingType = Nullable.GetUnderlyingType(propType);

                        if (!(dr[columnMapping.FieldName] is DBNull))
                            prop.SetValue(node, Convert.ChangeType(dr[columnMapping.FieldName], underlyingType ?? propType));
                    }
                    else if (propValue is BaseDataModel)
                    {
                        BaseDataModel childNode = (BaseDataModel)propValue;
                        FillDataModel(ref childNode, dr);
                    }

                }
            }
        }


        internal void SearchCrm( string fetchXML , ref BaseDataModel[] target )
        {

        }

        internal void SearchCrm<T>(QueryExpression query, ref List<T> listTarget) where T : new()
        {
            listTarget = new List<T>(); 
            using (OrganizationServiceProxy serviceProxy = GetCrmServiceProxy())
            {
                EntityCollection colEn = serviceProxy.RetrieveMultiple(query) ;
                foreach (Entity en in colEn.Entities)
                {
                    T data = new T();
                    

                }
            }
        }

        internal void InsertCRM( BaseDataModel model , string entityType)
        {

        }

        internal void UpdateCRM(BaseDataModel model, string entityType , string searchField)
        {

        }


        internal OrganizationServiceProxy GetCrmServiceProxy()
        {
            using (var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString))
            {
                using (_serviceProxy = connection.OrganizationServiceProxy)
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    //_service = (IOrganizationService)_serviceProxy;
                    //ServiceContext svcContext = new ServiceContext(_serviceProxy);

                    return _serviceProxy;
                }
            }
            //throw new NotImplementedException();
        }

        string GetLatestToken()
        {
            return "";
        }

        internal string GetAppConfigurationSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }

        string GetEWIEndpoint(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }

        enum ENUM_f_GetSystemUserByFieldInfo_Attr
        {
            DomainName,
            EmployeeId,
            FullName,
            Branch
        }
        string CallFn_f_GetSystemUserByFieldInfo(Guid UserId, ENUM_f_GetSystemUserByFieldInfo_Attr attr )
        {
            using (SqlConnection cnnSQL = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString()))
            {

                SqlCommand cmdSQL = new SqlCommand("Select dbo.f_GetSystemUserByFieldInfo( @TargetAttrName , @SystemUserId )", cnnSQL);
                cmdSQL.CommandTimeout = 600;

                SqlParameter param1 = new SqlParameter("@TargetAttrName", SqlDbType.NVarChar);
                param1.Value = attr.ToString() ;
                cmdSQL.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@SystemUserId", SqlDbType.UniqueIdentifier);
                param2.Value = UserId;
                cmdSQL.Parameters.Add(param2);

                cnnSQL.Open();
                return (string)cmdSQL.ExecuteScalar();
            }
        }

        internal string GetDomainName(Guid UserId)
        {
            return CallFn_f_GetSystemUserByFieldInfo(UserId, ENUM_f_GetSystemUserByFieldInfo_Attr.DomainName);
        }

        internal string GetEmployeeCode(Guid UserId)
        {
            return CallFn_f_GetSystemUserByFieldInfo(UserId, ENUM_f_GetSystemUserByFieldInfo_Attr.EmployeeId);
        }

    }
}