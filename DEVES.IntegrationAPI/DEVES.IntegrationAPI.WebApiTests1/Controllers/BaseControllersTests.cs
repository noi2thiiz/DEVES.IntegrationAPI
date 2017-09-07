using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Crm.Sdk.Messages;
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
            if (null != prop && prop.CanWrite)
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
    }
}
