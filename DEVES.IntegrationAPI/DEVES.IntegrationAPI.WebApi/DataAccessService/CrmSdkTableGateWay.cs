using System;
using System.Globalization;
using DEVES.IntegrationAPI.WebApi.Core.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

using DEVES.IntegrationAPI.WebApi.Core;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using System.Reflection;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;
using Microsoft.Xrm.Sdk.Query;
using System.Reflection;
using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public  class BaseCrmSdkTableGateWay<ENTITY_TYPE> : IDataGateWay
        where ENTITY_TYPE : class
    {
        public Entity BindedEntity;
        public string EntityName = "";

        public void AddAttribute<T>(string key, T value)
        {
            // throw new Exception("Error on AddAttribute :" + key + " to " + (typeof(T)).ToString());
            try
            {
                if (null != value)
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
                            var myDate = Convert.ToDateTime(value);
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
               
            }
           
            
            catch (Exception )
            {
                throw new Exception("Error on AddAttribute :" + key + " to " + Type.GetTypeCode(typeof(T)).ToString());
            }

        }

        protected void BindEntity(Entity entity)
        {
            foreach (PropertyInfo pi in entity.GetType().GetProperties())
            {
                try
                {
                    var propertyName = pi.Name.ToLower();
                    Console.WriteLine("Name:= " + propertyName);
                    var propertyType = pi.PropertyType.ToString();
                    //  dynamic value = pi.GetValue(incidentXrmEntity, null).ToString();
                    Console.WriteLine("PropertyType:= " + pi.PropertyType.ToString());
                    var value = entity.GetType().GetProperty(propertyName).GetValue(entity);
                    Console.WriteLine("Value:= " + value.ToString());
                    try
                    {
                        if (null != value)
                        {
                            switch (propertyType)
                            {
                                case "System.Int32":
                                    int v = Int32.Parse(value.ToString());
                                    BindedEntity[propertyName] = v;
                                    break;
                                case "System.Decimal":
                                    decimal d = Decimal.Parse(value.ToString());
                                    BindedEntity[propertyName] = d;
                                    break;
                                case "System.Enum":
                                    BindedEntity[propertyName] = value.ToString();
                                    break;
                                case "System.Boolean":
                                    BindedEntity[propertyName] = value;
                                    break;
                                case "System.DateTime":
                                    try
                                    {
                                        var myDate = (DateTime)value; // Convert.ToDateTime(value);
                                        PersianCalendar persianCalendar = new PersianCalendar();
                                        if (persianCalendar.GetYear(myDate) >= 1753)
                                        {
                                            BindedEntity[propertyName] = myDate;
                                        }
                                       
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Can not Convert "+ propertyName);
                                    }
                                   
                                    break;

                                case "Microsoft.Xrm.Sdk.OptionSetValue":
                                    BindedEntity[propertyName] = value;
                                    break;
                                case "Microsoft.Xrm.Sdk.EntityReference":
                                    EntityReference Ref = (EntityReference)value;
                                    if (Ref.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                                    {
                                        BindedEntity[propertyName] = value;
                                    }
                                   
                                    break;
                                case "Object":
                                    BindedEntity[propertyName] = value.ToString();
                                    break;
                                default:
                                    //  throw new Exception("Error on AddAttribute :" + key + " to " + Type.GetTypeCode(typeof(T)).ToString());
                                    BindedEntity[propertyName] = "" + value.ToString();
                                    break;
                            }
                        }
                            
                    }

                    catch (NullReferenceException )
                    {
                        // Do not thing
                    }
                    catch (Exception )
                    {
                        throw new Exception("Error on AddAttribute");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>PropertyInfo Error:" + e.Message);
                }

                Console.WriteLine(">>>>>>>>>N E X T>>>>>>>>");
            }
        }

     

        public void AddAttribute(string key, string value)
        {
            try
            {
                Console.WriteLine("AddAttribute"+ key +" Type String");
                BindedEntity[key] = "" + value.ToString();
            }
           
            catch (NullReferenceException)
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
            CrmServiceClient client = new CrmServiceClient(CrmConfigurationSettings.AppConfig.Get("CRMSDK").ToString());
            return client.OrganizationServiceProxy;
        }

        public Guid Create(Entity entity)
        {
            try
            {
                var _p = getOrganizationServiceProxy();
                BindedEntity = new Entity(EntityName);
                BindEntity(entity);

                Console.WriteLine(BindedEntity.ToJSON());
               
                var guid = _p.Create(BindedEntity);

                return guid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                throw new Exception(BindedEntity.ToJSON()+">>>>>>>>>>>>>>>>>>>>>>>>>>>>>Error on Create:" + e.Message + e.StackTrace );
            }
            finally
            {
               
            }
          
        }
        public List<string> IgnoreAttributes = new List<string>{
                      "logicalname",
                      "id",
                      "attributes",
                      "entitystate",
                      "formattedvalues",
                      "relatedentities",
                      "rowversion",
                      "keyattributes",
                      "item",
                      "extensiondata"};
        public ColumnSet GetColumnSet()
        {
            var attributes = new ColumnSet(new string[] {});
            foreach (PropertyInfo pi in typeof(ENTITY_TYPE).GetProperties())
            {
                var piName = pi.Name.ToLower();
                if (!IgnoreAttributes.Contains(piName))
                {
                    attributes.AddColumn(piName);
                }
               
            }

            return attributes;
               
        }
      
    public ENTITY_TYPE TranformEntity(Entity entity)
        {

            var obj =  (ENTITY_TYPE)Activator.CreateInstance(typeof(ENTITY_TYPE));
           // PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
            

            var attributes = new ColumnSet(new string[] { });
            foreach (PropertyInfo pi in typeof(ENTITY_TYPE).GetProperties())
            {
                string piName = pi.Name.ToLower();
                if (null != pi && pi.CanWrite && !IgnoreAttributes.Contains(piName))
                {
                    try
                    {
                        pi.SetValue(obj, entity[piName], null);
                    }
                    catch (Exception e)
                    {
                       // throw new Exception(e.Message + "===>" + piName);
                        Console.WriteLine(e.Message + "===>" + piName);
                    }
                  
                }
  
            }

            return obj;

        }
        public ENTITY_TYPE Find(Guid guid)
        {
      
            var jsonString = "";
            try
            {
                ColumnSet attributes = GetColumnSet();
                jsonString = attributes.ToJSON();
                var _p = getOrganizationServiceProxy();
                var entity = _p.Retrieve(EntityName, guid, attributes);
                var returnEntity = TranformEntity(entity);
                return returnEntity;
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message +""+ jsonString +"/n"+e.StackTrace);
            }
           
           
          
        }
        public Entity Retrieve(Guid guid, ColumnSet attributes)
        {
          
            var _p = getOrganizationServiceProxy();
            var entity = _p.Retrieve(EntityName, guid, attributes);
            return entity;
        }
    }
}