using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Description;
using DEVES.IntegrationAPI.WebApi.Services.Core.Exceptions;
using DEVES.IntegrationAPI.WebApi.Services.Sms;
using ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.SwaggerUi;
using TestApi;
using Xunit;
using Xunit.Abstractions;

namespace DEVES.IntegrationAPI.WebApi.Services.TechnicalService
{
    public class RESTClient
    {
        protected string URI = "";

        public RESTClient(string strURI)
        {
            URI = strURI ;
        }

        protected string username { get; set; }
        protected string password { get; set; }
        protected bool isUseBasicAuth { get; set; }

        public void UseBasicAuth(string username, string password)
        {
            this.isUseBasicAuth = true;
            this.username = username;
            this.password = password;
        }

        public  RESTClientResult Execute(object req)
        {

            return postData(URI, req.ToJSON());
        }
        public  RESTClientResult Execute(string req)
        {

            return postData(URI, req);
        }

        public void Post(string url, string jsonContent)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";

            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                // Log exception and throw as for GET example above
            }
        }


        public RESTClientResult postData(string baseAddress, string jsonString)
        {


            var dataResult = new RESTClientResult();
            try
            {
                HttpWebResponse httpResponse = null;
                string responseText = null;


                var httpWebRequest = (HttpWebRequest) WebRequest.Create(baseAddress);
                httpWebRequest.ContentType = @"application/json;charset=UTF-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json";

                if (isUseBasicAuth == true)
                {
                    // Create a token for basic authentication and add a header for it
                    String authorization =
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                    httpWebRequest.Headers.Add("Authorization", "Basic " + authorization);
                }


                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    Console.WriteLine(jsonString);
                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                }

                catch (WebException e)
                {
                    httpResponse = (HttpWebResponse) e.Response;
                    if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var r = new ServiceFailResult();
                        r.code = "404";
                        r.message = "Bad Request";
                        r.description = "remote service return 404 BadRequest";
                        throw new RemoteServiceBadRequestErrorException(r);

                    }
                    else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var r = new ServiceFailResult();
                        r.code = "500";
                        r.message = "Remote Error";
                        r.description = "remote service return 500 Internal ServerError";
                        throw new RemoteServiceInternalServerErrorException(r);

                    }
                    else
                    {
                        throw new CannotConnetServiceException();
                    }

                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("cannot connet to remote serve");
                    var msg = "cannot connet to remote serve";
                    throw new CannotConnetServiceException();
                }


                // expect the unexpected
                // WebException may be thrown already for some of this already
                // like timeout or 404
                // Console.WriteLine("expect the unexpected");
                // Console.WriteLine((int)httpResponse.StatusCode);
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    dataResult.statusCode = httpResponse.StatusCode;
                    dataResult.response = httpResponse;
                    dataResult.message = "response with status: " + httpResponse.StatusCode + " " + httpResponse.StatusDescription;
                    return dataResult;
                }

               // httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();


                dataResult.statusCode = httpResponse.StatusCode;
                dataResult.response = httpResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    dataResult.content = streamReader.ReadToEnd();

                    return dataResult;
                }
            }
            catch (WebException e)
            {
                var httpResponse = (HttpWebResponse) e.Response;
                try
                {
                    dataResult.statusCode = httpResponse.StatusCode;
                    dataResult.response = httpResponse;


                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dataResult.content = streamReader.ReadToEnd();

                        return dataResult;
                    }

                }
                catch (NullReferenceException ex)
                {
                    dataResult.statusCode = HttpStatusCode.InternalServerError;
                    dataResult.response = httpResponse;

                    throw new CannotConnetServiceException();
                }



            }


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
    }

    public class RESTClientTests:ControllerIntegrationTests
    {
        public RESTClientTests(ITestOutputHelper output) : base(output)
        {
        }



        /// <summary>
        /// ทดสอบกรณีปกติ service return responce http status code = 200
        /// </summary>
        [Fact]
        public async void Should_Return_Success_Response()
        {
            RESTClient client = new RESTClient("http://localhost:5001/api/test/Format");
            var responce  = client.Execute(@"{'datetime': '2017-03-19 00:13:00'}");

            Assert.NotNull(responce);
            Assert.NotEmpty(responce.content);
            output.WriteLine(responce.ToJSON());

        }

        /// <summary>
        /// ทดสอบ กรณีที่ติดต่อ server ไม่ได้ จะต้อง Throw CannotConnetServiceException
        /// </summary>
        [Fact]
        public async void Should_Throw_CannotConnetServiceException()
        {
            Assert.Throws<CannotConnetServiceException>(()=>
            {
                RESTClient client = new RESTClient("http://ddddds44423.com");
                var responce  = client.Execute("{'x':'x'}");

                ServiceResultHeaderOnly result = new ServiceResultHeaderOnly();
                result.SetResponse(responce);

            });

        }

        /// <summary>
        /// ทดสอบ กรณีที่ remote server return 404 Bad Request
        /// </summary>
        [Fact]
        public async void Should_Throw_RemoteServiceInternalServerErrorException()
        {
            Assert.Throws<RemoteServiceInternalServerErrorException>(()=>
            {
                RESTClient client = new RESTClient("http://localhost:5001/api/test/TestInternalServerError");
                var responce  = client.Execute(@"{'datetime': 'XXXXX'}");


            });

        }

        /// <summary>
        /// ทดสอบ กรณีที่ remote server return 404 Bad Request
        /// </summary>
        [Fact]
        public async void Should_Throw_RemoteServiceBadRequestErrorException()
        {
            Assert.Throws<RemoteServiceBadRequestErrorException>(()=>
            {
                RESTClient client = new RESTClient("http://localhost:5001/api/test/Format");
                var responce  = client.Execute(@"{'datetime': 'XXXXX'}");


            });

        }
    }
}