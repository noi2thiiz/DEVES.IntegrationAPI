using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
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

            return PostData(URI, req.ToJson());
        }
        public  RESTClientResult Execute(string req)
        {

            return PostData(URI, req);
        }

       
       

       

        public void Post(string url, string jsonContent)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";

            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            //request.ContentType = @"application/json";
            request.ContentType = @"application/json;charset=UTF-8";

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
            catch (WebException)
            {
                // Log exception and throw as for GET example above
            }
        }

        public async Task<WebResponse> StartWebRequest(object input)
        {
           
            string jsonString;
      
             jsonString = input.ToJson();

            string baseAddress = URI;
  
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseAddress);
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
                   // Console.WriteLine(jsonString);
                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
       
               return  await httpWebRequest.GetResponseAsync();
                

        }

        public Task<HttpResponseMessage> PostAsync(object input)
        {
       
            var client = new HttpClient();
            return client.PostAsync(URI, new StringContent(input.ToJson(), Encoding.UTF8, "application/json"));
        }


        public RESTClientResult PostData(string baseAddress, string jsonString)
        {


            var dataResult = new RESTClientResult();
            dataResult.Params = jsonString;
            try
            {
                HttpWebResponse httpResponse = null;
               // string responseText = null;


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
                   // Console.WriteLine(jsonString);
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
                        //var r = new ServiceFailResult();
                        //r.code = "404";
                        //r.message = "Bad Request";
                        //r.description = "remote service return 404 BadRequest";
                        //throw new RemoteServiceBadRequestErrorException(r);

                    }
                    else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                      //  var r = new ServiceFailResult();
                      //  r.code = "500";
                       // r.message = "Remote Error";
                       // r.description = "remote service return 500 Internal ServerError";
                       // throw new RemoteServiceInternalServerErrorException(r);

                    }
                    else
                    {
                       // throw new CannotConnetServiceException();
                    }
                    throw;
                }

                catch (Exception e)
                {
                   // Console.WriteLine(e);
                    //Console.WriteLine("cannot connet to remote serve");
                   // var msg = "cannot connet to remote serve";
                    //throw new CannotConnetServiceException();
                    throw;
                }


                // expect the unexpected
                // WebException may be thrown already for some of this already
                // like timeout or 404
                // Console.WriteLine("expect the unexpected");
                // Console.WriteLine((int)httpResponse.StatusCode);
                if (httpResponse != null && httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    dataResult.StatusCode = httpResponse.StatusCode;
                    dataResult.Response = httpResponse;
                    dataResult.Request =  httpWebRequest;


                    dataResult.Message = "response with status: " + httpResponse.StatusCode + " " + httpResponse.StatusDescription;
                    return dataResult;
                }

               // httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();


                if (httpResponse != null)
                {
                    dataResult.StatusCode = httpResponse.StatusCode;
                    dataResult.Response = httpResponse;
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dataResult.Content = streamReader.ReadToEnd();

                        return dataResult;
                    }
                }
            }
            catch (WebException e)
            {
                var httpResponse = (HttpWebResponse) e.Response;
                try
                {
                    dataResult.StatusCode = httpResponse.StatusCode;
                    dataResult.Response = httpResponse;


                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        dataResult.Content = streamReader.ReadToEnd();

                        return dataResult;
                    }

                }
                catch (NullReferenceException)
                {
                    dataResult.StatusCode = HttpStatusCode.InternalServerError;
                    dataResult.Response = httpResponse;

                   // throw new CannotConnetServiceException();
                   
                }

                throw;

            }
            return dataResult;

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
                    //Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    //Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

       
    }
    
}