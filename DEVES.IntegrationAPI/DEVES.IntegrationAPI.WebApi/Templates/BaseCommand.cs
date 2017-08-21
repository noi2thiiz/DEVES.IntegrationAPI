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
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.ConnectCRM;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public abstract class BaseCommand
    {
        internal const string CONST_CODE_SUCCESS = "200";
        internal const string CONST_MESSAGE_SUCCESS = "SUCCESS";
        internal const string CONST_CODE_FAILED = "500";
        internal const string CONST_MESSAGE_INTERNAL_ERROR = "An error has occurred";
        internal const string CONST_DEFAULT_UID = "uid";
        internal const string CONST_CODE_INVALID_INPUT = "400";
        internal const string CONST_MESSAGE_INVALID_INPUT = "Invalid input(s)";
        internal const string CONST_DESC_INVALID_INPUT = "Some of your input is invalid. Please recheck again";


        public string TransactionId { get; set; } = "";
        public string ControllerName { get; set; } = "";

        private OrganizationServiceProxy _serviceProxy;
        private IOrganizationService _service;

        private Guid CurrentUserId { get; set; }

        private const string CONST_JSON_SCHEMA_FILE = "JSON_SCHEMA_{0}";

        // variables after this line will be contained in LOG
        private HttpClient client = new HttpClient();

        private const string appName = "xrmAPI"; // Application
        private string serviceName = "";
        private const string activity = "consume"; // Activity
        private string user = ""; // User
        private string machineName = Environment.MachineName; // Machine
        private string ip = ""; // RequestIpAddress;
        private string reqContentType = ""; // RequestContentType = context.Request.ContentType;
        private string jsonReqModel = ""; // RequestContentBody
        private string uri = ""; // RequestUri
        private string reqMethod = ""; // RequestMethod
        private string routeTemplate = ""; // RequestRouteTemplate
        private string requestRouteData = ""; // RequestRouteData
        private string reqHeader = ""; // RequestHeaders
        private DateTime reqTime = new DateTime(); // RequestTimestamp
        private string resContentType = ""; // ResponseContentType
        private string resBody = ""; // ResponseContentBody
        private string resStatus = ""; // ResponseStatusCode
        private string resHeader = ""; // ResponseHeaders
        private DateTime resTime = new DateTime(); // ResponseTimestamp


        //This is like the Main() function. And need to be implemented.
        public abstract Model.BaseDataModel Execute(object input);

        //internal Model.EWI.EWIResponse WrapModel(Model.BaseDataModel model)
        //{
        //    Model.EWI.EWIResponse res = new EWIResponse();
        //    res.content = model;
            
        //}

        private bool ValidateJSON(string strJSON)
        {
            string typeName = this.GetType().Name;
            throw new NotImplementedException();
        }
        /*
        private string GetEwiUsername()
        {

            return AppConfig.Instance.Get("EWI_USERNAME") ?? "sysdynamic";
        }
        private string GetEwiPassword()
        {

            return AppConfig.Instance.Get("EWI_PASSWORD") ?? "REZOJUNtN04=";
        }

        private string GetEwiUid()
        {

            return AppConfig.Instance.Get("EWI_UID") ?? "DevesClaim";
        }
        private string GetEwiGid()
        {

            return AppConfig.Instance.Get("EWI_GID") ?? "DevesClaim";
        }
        */

        internal BaseContentJsonProxyOutputModel CallDevesJsonProxy<T1>(string EWIendpointKey, BaseDataModel JSON, string UID=CONST_DEFAULT_UID) where T1 : BaseEWIResponseModel 
        {
            EWIRequest reqModel = new EWIRequest()
            {
                //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                username = AppConfig.GetEwiUsername(),
                password = AppConfig.GetEwiPassword(),
                uid = AppConfig.GetEwiUid(),
                gid = AppConfig.GetEwiGid(),
                token = GetLatestToken(),
                content = JSON
            };

            jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            var media = new MediaTypeWithQualityHeaderValue("application/json") { CharSet = "utf-8" };
            client.DefaultRequestHeaders.Accept.Add(media);
            client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

            // + ENDPOINT
            string EWIendpoint = CommonConstant.PROXY_ENDPOINT+ GetEWIEndpoint(EWIendpointKey);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");
            //request.Headers.Add("ContentType", "application/json; charset=UTF-8");
            // เช็ค check reponse 
            LogAsync(request);
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
           
            //  Console.WriteLine("==========jsonReqModel========");
            // Console.WriteLine(EWIendpoint);
            //Console.WriteLine(jsonReqModel.ToJson());

            T1 ewiRes = response.Content.ReadAsAsync<T1>().Result;
            resBody = ewiRes.ToJson();
            var sv = EWIendpoint.Split('/');
            serviceName = sv[sv.Length - 1];

            LogAsync(request, response);


            // Console.WriteLine("==========response========");
            // Console.WriteLine(ewiRes.ToJson());

            BaseContentJsonProxyOutputModel output =  (BaseContentJsonProxyOutputModel)typeof(T1).GetProperty("content").GetValue(ewiRes);
            if(output.message == null)
            {
                output.message = typeof(T1).GetProperty("responseCode").GetValue(ewiRes).ToString() + ": " + typeof(T1).GetProperty("responseMessage").GetValue(ewiRes).ToString();
            }
            return output;
        }

        internal T2 CallDevesServiceProxy<T1, T2>(string EWIendpointKey, BaseDataModel JSON, string UID = CONST_DEFAULT_UID) 
                                        where T1 : BaseEWIResponseModel 
                                        //where T2 : BaseContentJsonProxyOutputModel, BaseContentJsonServiceOutputModel
        {
            var limitTry = 3;
            var countTry = 0;
           
               
                while (countTry<= limitTry)
                {
                    countTry += 1;
                    try
                    {
                        
                    
                    

                    EWIRequest reqModel = new EWIRequest()
                    {
                        //user & password must be switch to get from calling k.Ton's API rather than fixed values.
                        username = AppConfig.GetEwiUsername(),
                        password = AppConfig.GetEwiPassword(),
                        uid = AppConfig.GetEwiUid(),
                        gid = AppConfig.GetEwiGid(),
                        token = GetLatestToken(),
                        content = JSON

                    };


                    jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented,
                        new EWIDatetimeConverter(JSON.DateTimeCustomFormat));

                    client = new HttpClient();

                    client.DefaultRequestHeaders.Accept.Clear();
                    var media = new MediaTypeWithQualityHeaderValue("application/json") {CharSet = "utf-8"};
                    client.DefaultRequestHeaders.Accept.Add(media);
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");


                    // + ENDPOINT
                    string EWIendpoint = CommonConstant.PROXY_ENDPOINT + GetEWIEndpoint(EWIendpointKey);
                    reqTime = DateTime.Now;
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
                    // Console.WriteLine("==========jsonReqModel========");
                    //Console.WriteLine(jsonReqModel.ToJson());
                    //request.Headers.Add("ContentType", "application/json; charset=UTF-8");
                    request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");
                    //Console.WriteLine("==========request========");
                    //Console.WriteLine(request.ToJson());
                    // เช็ค check reponse 
                    LogAsync(request);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    resTime = DateTime.Now;

                    response.EnsureSuccessStatusCode();

                    T1 ewiRes = response.Content.ReadAsAsync<T1>().Result;
                    resBody = ewiRes.ToJson();
                    string[] splitServiceName = ewiRes.ToString().Split('.');
                    var sv = EWIendpoint.Split('/');
                    serviceName = sv[sv.Length - 1];
                    LogAsync(request, response);


                    //Console.WriteLine("==========response========");
                    // Console.WriteLine(ewiRes.ToJson());
                    if (ewiRes.success)
                    {

                        T2 output = (T2) typeof(T1).GetProperty("content").GetValue(ewiRes);
                        return output;
                    }
                    else
                    {
                        //CLSCreateCorporateClientOutputModel
                        throw new BuzErrorException(
                            "500",
                            $"Error:{ewiRes.responseCode}, Message:{ewiRes.responseMessage}",
                            $"An error occurred from the external service ({serviceName})",

                            serviceName,
                            TransactionId);
                        //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
                    }

                    }
                    catch (Exception)
                    {
                        if (countTry > limitTry)
                        {
                            throw;
                        }
                    }
            } 
            
            return default(T2);
        }

        protected async Task<ApiLogEntry> LogAsync(HttpRequestMessage req, HttpResponseMessage res)
        {
            try
            {

                // Request
                // var reqContext = ((HttpContextBase)req.Properties["MS_HttpContext"]);

                // Map Request vaule to global Variables (Request)
                //user = reqContext.User.Identity.Name;
                //ip = reqContext.Request.UserHostAddress;
                //reqContentType = reqContext.Request.ContentType;
                uri = req.RequestUri.ToString();
                Console.WriteLine("======LOG======");
                Console.WriteLine("======LOG======");
                Console.WriteLine("======LOG======");
                Console.WriteLine("RequestUri" + uri);
                Console.WriteLine("RequestContentBody" + jsonReqModel);
                reqMethod = req.Method.Method;
                // routeTemplate = req.GetRouteData().Route.RouteTemplate;
                reqHeader = req.Headers.ToString();

               


                // Map Request vaule to global Variables (Response)
                resContentType = "";
                // resBody = res.Content.Headers.ToString();
                resStatus = "";
                if (res != null)
                {
                    resHeader = res.Headers.ToString();
                }
                var sv = uri.Split('/');
                serviceName = sv[sv.Length - 1];

                var apiLogEntry = new ApiLogEntry
                {
                    Application = appName,
                    TransactionID = TransactionId,
                    Controller = "",
                    ServiceName = serviceName,
                    Activity = "consume:Receive response",
                    User = user,
                    Machine = machineName,
                    RequestIpAddress = ip,
                    RequestContentType = client.DefaultRequestHeaders?.Accept.ToString(),
                    RequestContentBody = jsonReqModel,
                    RequestUri = uri,
                    RequestMethod = reqMethod,
                    RequestRouteTemplate = routeTemplate,
                    RequestRouteData = requestRouteData,
                    RequestHeaders = reqHeader,
                    RequestTimestamp = reqTime,
                    ResponseContentType = resContentType,
                    ResponseContentBody = resBody,
                    ResponseStatusCode = res?.StatusCode.ToString(),
                    ResponseHeaders = resHeader,
                    ResponseTimestamp = DateTime.Now
                };
                InMemoryLogData.Instance.AddLogEntry(apiLogEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
            }

            return null;
        }

        protected async Task<ApiLogEntry> LogAsync(HttpRequestMessage req)
        {

            // Request
            // var reqContext = ((HttpContextBase)req.Properties["MS_HttpContext"]);

            // Map Request vaule to global Variables (Request)
            //user = reqContext.User.Identity.Name;
            //ip = reqContext.Request.UserHostAddress;
            //reqContentType = reqContext.Request.ContentType;
            uri = req.RequestUri.ToString();
            Console.WriteLine("======LOG======");
            Console.WriteLine("======LOG======");
            Console.WriteLine("======LOG======");
            Console.WriteLine("RequestUri" + uri);
            Console.WriteLine("RequestContentBody" + jsonReqModel);
            reqMethod = req.Method.Method;
            // routeTemplate = req.GetRouteData().Route.RouteTemplate;
            reqHeader = req.Headers.ToString();

            try
            {
                // requestRouteData = req.GetRouteData().ToJson();
            }
            catch (Exception e)
            {
                // do nothing
            }
            var sv = uri.Split('/');
            serviceName = sv[sv.Length - 1];

            // Map Request vaule to global Variables (Response)
            resContentType = "";
            // resBody = res.Content.Headers.ToString();
            resStatus = "";
         


            var apiLogEntry = new ApiLogEntry
            {
                Application = appName,
                TransactionID = TransactionId,
                Controller = "",
                ServiceName = serviceName,
                Activity = "consume:Send request",
                User = user,
                Machine = machineName,
                RequestIpAddress = ip,
                RequestContentType = client.DefaultRequestHeaders?.Accept.ToString(),
                RequestContentBody = jsonReqModel,
                RequestUri = uri,
                RequestMethod = reqMethod,
                RequestRouteTemplate = routeTemplate,
                RequestRouteData = requestRouteData,
                RequestHeaders = reqHeader,
                RequestTimestamp = reqTime,
            
                ResponseTimestamp = DateTime.Now
            };
            InMemoryLogData.Instance.AddLogEntry(apiLogEntry);

            return null;
        }


        //internal string CallEWIService(string EWIendpointKey, BaseDataModel JSON)
        //{
        //    string UID = "ClaimMotor";
        //    EWIRequest reqModel = new EWIRequest()
        //    {
        //        //user & password must be switch to get from calling k.Ton's API rather than fixed values.
        //        username = "sysdynamic",
        //        password = "REZOJUNtN04=",
        //        uid = UID,
        //        gid = UID,
        //        token = GetLatestToken(),
        //        content = JSON
        //    };

        //    string jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

        //    HttpClient client = new HttpClient();

        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

        //    // + ENDPOINT
        //    string EWIendpoint = GetEWIEndpoint(EWIendpointKey);
        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
        //    request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");

        //    // เช็ค check reponse 
        //    HttpResponseMessage response = client.SendAsync(request).Result;
        //    response.EnsureSuccessStatusCode();

        //    string strResult = response.Content.ReadAsStringAsync().Result;
        //    return strResult;
        //}

        internal T DeserializeJson<T>(string contentText)
        {
            Console.WriteLine(contentText.ToString());
            T contentModel;
            string validateResult = string.Empty;
            Type objType = typeof(T);
            string key = string.Format(CONST_JSON_SCHEMA_FILE, objType.Name );
            var filePath = "";
            try
            {
                filePath = HttpContext.Current.Server.MapPath(GetAppConfigurationSetting(key));
            }
            catch (Exception e)
            {
                //C:\Users\patiw\Source\Repos\Production1\DEVES.IntegrationAPI\DEVES.IntegrationAPI.WebApiTests1\bin\Release
                string startupPath = Environment.CurrentDirectory?.Replace(@"Tests1\bin\Release", "");

                filePath = startupPath + "/App_Data/JsonSchema/" + GetAppConfigurationSetting(key).Replace("~/App_Data/JsonSchema/","");
            }

           // var filePath = HttpContext.Current.Server.MapPath(GetAppConfigurationSetting(key));
            Console.WriteLine(filePath);
            if (JsonHelper.TryValidateJson(contentText, filePath, out validateResult))
            {
                Console.WriteLine("DeserializeObject");
                contentModel = JsonConvert.DeserializeObject<T>(contentText);
                return contentModel;
            }
            else
            {
                Console.WriteLine("Validation Error!");
                Console.WriteLine(validateResult.ToJson());
                throw new JsonSerializationException("Validation Error!");
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
                cmdSQL.CommandTimeout = 60;
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
     
                using (_serviceProxy = ConnectionThreadSafe.GetOrganizationProxy())
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();


                    return _serviceProxy;
                }
  
        }

        string GetLatestToken()
        {
            return "";
        }

        internal string GetAppConfigurationSetting(string key)
        {
            try
            {
                Console.WriteLine("==================key=================");
                Console.WriteLine(key);
                Console.WriteLine("==================End Key=================");
                return !string.IsNullOrEmpty(AppConfig.Instance.Get(key))
                    ? AppConfig.Instance.Get(key).ToString() : System.Configuration.ConfigurationManager.AppSettings[key]
                          .ToString();

            }
            catch (Exception e)
            {
                return "";
            }


        }

        string GetEWIEndpoint(string key)
        {
            try
            {
                Console.WriteLine("==================key=================");
                Console.WriteLine(key);
                Console.WriteLine("==================End Key=================");
                return !string.IsNullOrEmpty(AppConfig.Instance.Get(key))
                    ? AppConfig.Instance.Get(key).ToString() : System.Configuration.ConfigurationManager.AppSettings[key]
                        .ToString();

            }
            catch (Exception e)
            {
                return "";
            }

          
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

                var ret = cmdSQL.ExecuteScalar();
                if (ret != null && DBNull.Value != ret)
                {
                    return (string)ret;
                }
                else
                {
                    return string.Empty;
                }
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


        internal  bool IsOutputSuccess( BaseContentJsonProxyOutputModel content )
        {
            bool success=false;
            if (content.code == "200")
                success = true;
            return success;
        }

        public List<string> SearchCrmContactClientId(string cleansingId)
        {

            // Console.WriteLine("SearchCrmContactClientId");
            // For performance, until we found the way to cache the ServiceProxy, we prefer SQL rather than Crm
           
            
                return SpApiCustomerClient.Instance.SearchCrmContactClientId("P", cleansingId);
            
            
           
            /*
            using (OrganizationServiceProxy sp = GetCrmServiceProxy())
            {
                ServiceContext sc = new ServiceContext(sp);
                var q = from contacts in sc.ContactSet
                        where contacts.pfc_cleansing_cusormer_profile_code == cleansingId && contacts.StateCode == ContactState.Active
                        select contacts.pfc_crm_person_id;
                List<string> lstCrmClientId = q.ToList<string>();




                return lstCrmClientId;
            }
            */
            


            //List<string> lstCrmClientId = new List<string>();
            //List<CommandParameter> lstParam = new List<CommandParameter>();
            //lstParam.Add(new CommandParameter("clientType", "P"));
            //lstParam.Add(new CommandParameter("cleansingId", cleansingId));

            //DataTable dt = CallSQLStoredProc("sp_CustomApp_GetCrmClientid_byCleansingId" ,  lstParam);
            //foreach (DataRow row in dt.Rows)
            //{
            //    lstCrmClientId.Add(row[0].ToString());
            //}
            //return lstCrmClientId;

        }

        internal List<string> SearchContactPolisyId(string cleansingId)
        {

            // Console.WriteLine("SearchCrmContactClientId");
            // For performance, until we found the way to cache the ServiceProxy, we prefer SQL rather than Crm
            
            using (OrganizationServiceProxy sp = GetCrmServiceProxy())
            {
                ServiceContext sc = new ServiceContext(sp);
                var q = from contacts in sc.ContactSet
                        where contacts.pfc_cleansing_cusormer_profile_code == cleansingId && contacts.StateCode == ContactState.Active
                        select contacts.pfc_polisy_client_id;
                List<string> lstCrmClientId = q.ToList<string>();

                return lstCrmClientId;
            }
            
            
        }

        internal List<string> SearchCrmAccountClientId(string cleansingId)
        {
            //List<string> lstCrmClientId = new List<string>();
            //List<CommandParameter> lstParam = new List<CommandParameter>();
            //lstParam.Add(new CommandParameter("clientType", "C"));
            //lstParam.Add(new CommandParameter("cleansingId", cleansingId));

            //DataTable dt = CallSQLStoredProc("sp_CustomApp_GetCrmClientid_byCleansingId", lstParam);
            //foreach (DataRow row in dt.Rows)
            //{
            //    lstCrmClientId.Add(row[0].ToString());
            //}
            //return lstCrmClientId;

            // For performance, until we found the way to cache the ServiceProxy, we prefer SQL rather than Crm
            return SpApiCustomerClient.Instance.SearchCrmContactClientId("C", cleansingId);
            /*
            using (OrganizationServiceProxy sp = GetCrmServiceProxy())
            {
                ServiceContext sc = new ServiceContext(sp);
                var q = from accounts in sc.AccountSet
                        where accounts.pfc_cleansing_cusormer_profile_code == cleansingId && accounts.StateCode == AccountState.Active
                        select accounts.AccountNumber; 
                List<string> lstCrmClientId = q.ToList<string>();
                return lstCrmClientId;
            }
            */
            
        }

        internal List<string> SearchAccountPolisyId(string cleansingId)
        {
            using (OrganizationServiceProxy sp = GetCrmServiceProxy())
            {
                ServiceContext sc = new ServiceContext(sp);
                var q = from accounts in sc.AccountSet
                        where accounts.pfc_cleansing_cusormer_profile_code == cleansingId && accounts.StateCode == AccountState.Active
                        select accounts.pfc_polisy_client_id;
                List<string> lstCrmClientId = q.ToList<string>();
                return lstCrmClientId;
            }

        }

        protected OrganizationServiceProxy GetOrganizationServiceProxy(out ServiceContext svcContext)
        {
          //  var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = ConnectionThreadSafe.GetOrganizationProxy();
            svcContext = new ServiceContext(_serviceProxy);
            _serviceProxy.EnableProxyTypes();

            return _serviceProxy;
        }

    }

    public  class TestCommand: BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            throw new NotImplementedException();
        }
    }
}
 