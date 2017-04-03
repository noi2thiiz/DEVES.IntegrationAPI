using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using Xunit.Abstractions;

namespace TestApi
{
    public class ControllerIntegrationTests
    {
        protected readonly ITestOutputHelper output;


        public bool IsValidJsonSchema(JObject obj, JsonSchema schema)
        {
            return obj.IsValid(schema);
        }

        private void AssertJsonShema(string jsonSchemaFileName, string jsonString, bool isValid)
        {
            output.WriteLine("-------------Expect---isValid{0}----------------------\n",isValid);
            output.WriteLine(jsonString);

            var jsonSchemaString = _LoadSchema(jsonSchemaFileName);

           // output.WriteLine(jsonSchemaString);

            var schema = JSchema.Parse(jsonSchemaString);

            var jsonObject = JObject.Parse(jsonString);

            IList<string> messages;
            var valid = jsonObject.IsValid(schema, out messages);
            //output.WriteLine(valid.ToString());
            output.WriteLine(messages.ToJSON());
            output.WriteLine("--------------Result {0}------------------------\n",valid);
            Assert.Equal(isValid, valid);
        }

        public void AssertValidJsonShema(string jsonSchemaFileName, string jsonString)
        {
            AssertJsonShema(jsonSchemaFileName, jsonString, true);
        }

        public void AssertInValidJsonShema(string jsonSchemaFileName, string jsonString)
        {
            AssertJsonShema(jsonSchemaFileName, jsonString, false);
        }

        public void AssertSuccessPostRequest(string endPoint, string jsonInput)
        {
            var response = postData(endPoint, jsonInput);
            // output.WriteLine(response.ToJSON());

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.statusCode);

            dynamic obj = JObject.Parse(response.content);
            // output.WriteLine(obj.ToString());
            Assert.Equal("200", obj.code.ToString());
           // Assert.Equal("success", obj.message.ToString());
        }

        public void AssertBadRequestPostRequest(string endPoint, string jsonInput)
        {
            var response = postData(endPoint, jsonInput);
            // output.WriteLine(response.ToJSON());

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.statusCode);

            dynamic obj = JObject.Parse(response.content);
            // output.WriteLine(obj.ToString());
            Assert.Equal("400", obj.code.ToString());
        }

        public void AssertNotAcceptEmptyRequest(string endPoint)
        {
            var response = postData(endPoint, @"{}");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.statusCode);

            dynamic obj = JObject.Parse(response.content);
            //output.WriteLine(obj.ToString());
            Assert.Equal("400", obj.code.ToString());
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public ControllerIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        public class PostResult
        {
            public string content { get; set; }
            public HttpStatusCode statusCode { get; set; }
            public HttpWebResponse response { get; set; }
        }

        public PostResult postData(string baseAddress, string jsonString)
        {
            var postResult = new PostResult();
            try
            {
                Console.WriteLine(jsonString);
                HttpWebResponse httpResponse = null;
                String responseText = null;


                var httpWebRequest = (HttpWebRequest) WebRequest.Create(baseAddress);
                httpWebRequest.ContentType = @"application/json;charset=UTF-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json";


                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    Console.WriteLine(jsonString);
                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();


                postResult.statusCode = httpResponse.StatusCode;
                postResult.response = httpResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    postResult.content = streamReader.ReadToEnd();

                    return postResult;
                }
            }
            catch (WebException e)
            {
                var httpResponse = (HttpWebResponse) e.Response;

                postResult.statusCode = httpResponse.StatusCode;
                postResult.response = httpResponse;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    postResult.content = streamReader.ReadToEnd();

                    return postResult;
                }
            }
        }

        public string _LoadSchema(string fileName)
        {
            //output.WriteLine(fileName);
            var folderAppData =
                @"C:\workspace\crm\master\DEVES.IntegrationAPI-dev\DEVES.IntegrationAPI\DEVES.IntegrationAPI.WebApi\App_Data\JsonSchema\";

            var filePath =
                folderAppData + fileName; //"doc-v1.json";//HttpContext.Current.Server.MapPath(folderAppData+fileName);
            //  output.WriteLine(filePath);
            string sj = File.ReadAllText(@"" + filePath);
            return sj;
            //output.WriteLine(sj);

            //   IList<JToken> rates = root["HotelListResponse"]["HotelList"]["HotelSummary"][0]["RoomRateDetailsList"]["RoomRateDetails"]["RateInfos"].Children().ToList();

            //   var result =JsonConvert.DeserializeObject<RateInfo>( rateInfo .ToString() );

            JObject o1 = JObject.Parse(sj);
            var el = o1.GetValue("definitions")["VoiceRecordRequestModel"];
            // output.WriteLine(el.ToString());
            // o1.

            //  output.WriteLine(el.ToJSON());

            return el.ToString();
        }

        public void AssertFileExists( string filePath)
        {

            Assert.Equal(true, File.Exists(filePath));
        }
    }
}