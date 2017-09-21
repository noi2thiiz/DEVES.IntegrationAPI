using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    public  class BaseControllersTests
    {
        protected Task<string> ExcecuteControllers<TController>(string jsonString, string method="Post")
        {
            // Arrange
            var type = typeof(TController);
            var instance = Activator.CreateInstance(type);

            PropertyInfo prop = type.GetProperty("Request", BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(instance, new HttpRequestMessage(), null);
            }

            PropertyInfo prop2 = type.GetProperty("Configuration", BindingFlags.Public | BindingFlags.Instance);
            if (null != prop2 && prop2.CanWrite)
            {
                prop2.SetValue(instance, new HttpConfiguration(), null);
            }

            MethodInfo theMethod = type.GetMethod(method);

    
            // Act
            object input = JObject.Parse(jsonString);
            HttpResponseMessage response = (HttpResponseMessage)theMethod.Invoke(instance, new []{ input });
           // HttpResponseMessage response = (HttpResponseMessage)instance.Post(input);
            return response?.Content?.ReadAsStringAsync();
            
        }


        protected void AssertRequiredField(string profileinfoPersonalname, JObject outputJson)
        {
            Assert.IsNotNull(outputJson["data"]["fieldErrors"], "data.fieldErrors should not null");

            var fieldErrors = outputJson["data"]["fieldErrors"].ToArray();
            var filedErrorFound = false;
            foreach (var field  in fieldErrors)
            {
                if (field["name"]?.ToString()== profileinfoPersonalname)
                {
                    filedErrorFound = true;
                    var message = field["message"]?.ToString() ?? "";
                    if (!(message == "Required field must not be null" || message.Contains("is not defined in enum")  || message== "Required properties are missing from object" || message== "Required field must not be null or empty"))
                    {
                        Assert.Fail($"Assert Required field {profileinfoPersonalname} Expect<Required field must not be null> but <{field["message"]?.ToString()}>");
                    }
                }
             
            }
            if (!filedErrorFound)
            {
                Assert.Fail($"Assert Required field {profileinfoPersonalname} but not found in fieldErrors list");
            }
           

        }
    }
}
