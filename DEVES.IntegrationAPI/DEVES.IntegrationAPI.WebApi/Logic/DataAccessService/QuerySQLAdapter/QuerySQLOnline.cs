using DEVES.IntegrationAPI.Model.QuerySQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter
{
    public class QuerySQLOnline : QuerySQLStretagy
    {
        public QuerySQLOutputModel GetQuery(string databaseName, string sqlCommand)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://crmappdev.deves.co.th/proxy/xml.ashx?http://192.168.8.121/QueryAPI/api/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 3, 0);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "QuerySQL");
            string content = "{\"databaseName\": " + "\"" + databaseName + "\"" + "," +
                "\"sqlCommand\": " + "\"" + sqlCommand + "\"" + "}"
                ;
            req.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            req.RequestUri = new Uri("https://crmappdev.deves.co.th/proxy/xml.ashx?http://192.168.8.121/QueryAPI/api/QuerySQL");
            // response.RequestMessage.RequestUri = new Uri("https://crmappdev.deves.co.th/proxy/xml.ashx?http://192.168.8.121/QueryAPI/api/QuerySQL");

            HttpResponseMessage response = client.SendAsync(req).Result;
            response.EnsureSuccessStatusCode();
            var sql = response.Content.ReadAsStringAsync().Result;

            QuerySQLOutputModel output = new QuerySQLOutputModel();
            output = JsonConvert.DeserializeObject<QuerySQLOutputModel>(sql);

            return output;

        }
    }
}