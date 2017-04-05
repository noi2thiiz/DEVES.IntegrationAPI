using System;
using System.Globalization;
using DEVES.IntegrationAPI.WebApi.Core.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

using DEVES.IntegrationAPI.WebApi.Core;

namespace DEVES.IntegrationAPI.WebApi.Services.DataGateWay
{
    public abstract class CrmSdkTableGateWayAbstract : IDataGateWay
    {
        public Entity BindedEntity;
        public string EntityName = "";

        public void AddAttribute<T>(string key, T value)
        {
            // throw new Exception("Error on AddAttribute :" + key + " to " + (typeof(T)).ToString());
            try
            {

                switch (typeof(T).ToString())
                {
                    case "System.Int32":
                        int v = Int32.Parse(value.ToString());
                        BindedEntity[key] = v;
                        break;
                    case "System.Enum":
                        BindedEntity[key] = value.ToString();
                        break;
                    case "System.DateTime":
                        var myDate = Convert.ToDateTime(value) ;
                        BindedEntity[key] = myDate;
                        break;

                    case "Microsoft.Xrm.Sdk.OptionSetValue":
                        BindedEntity[key] = value;
                        break;
                    case "Object":
                        BindedEntity[key] = value.ToString();
                        break;
                    default:
                        //  throw new Exception("Error on AddAttribute :" + key + " to " + Type.GetTypeCode(typeof(T)).ToString());
                        BindedEntity[key] = "" + value.ToString();
                        break;
                }
            }
           
            catch (NullReferenceException  e)
            {
                // Do not thing
            }
            catch (Exception e)
            {
                throw new Exception("Error on AddAttribute :" + key + " to " + Type.GetTypeCode(typeof(T)).ToString());
            }

        }

        public void AddAttribute(string key, string value)
        {
            try
            {
                Console.WriteLine("AddAttribute"+ key +" Type String");
                BindedEntity[key] = "" + value.ToString();
            }
           
            catch (NullReferenceException  e)
            {
                // Do not thing
            }

            catch (Exception e)
            {
                throw new Exception("Error on AddAttribute :" + key+": Error:"+e.Message+"ErrorType:"+e.GetType());
            }
        }

        public  DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }
        public  DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public  DateTime StringToDateTime(string dateString)
        {
            DateTime dt;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt))
            {
                return  dt;
                //;//dt;
            }
            else
            {
               var r =new ServiceFailResult();
                r.message = "cannot convert {dateString} to dateTime";
                throw new RemoteServiceBadRequestErrorException(r);
                //  return  DateTime.Now;
            }


        }


        public OrganizationServiceProxy getOrganizationServiceProxy()
        {
            CrmServiceClient client = new CrmServiceClient(CrmConfigurationSettings.AppConfig.Get("settings.CRMSDK").ToString());
            return client.OrganizationServiceProxy;
        }
    }
}