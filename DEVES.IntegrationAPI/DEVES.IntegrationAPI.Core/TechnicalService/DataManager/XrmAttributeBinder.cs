using System;
using System.Globalization;
using DEVES.IntegrationAPI.WebApi.Core;
using DEVES.IntegrationAPI.WebApi.Core.Exceptions;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.Core.TechnicalService.DataManager
{
    public class XrmAttributeBinder
    {
        private Entity BindedEntity;

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public DateTime StringToDateTime(string dateString)
        {
            DateTime dt;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt))
            {
                return dt;
                //;//dt;
            }
            else
            {
                var r = new ServiceFailResult();
                r.message = "cannot convert {dateString} to dateTime";
                throw new RemoteServiceBadRequestErrorException(r);
                //  return  DateTime.Now;
            }
        }

        protected void BindEntity(Entity entity)
        {
            foreach (var pi in entity.GetType().GetProperties())
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
                    Console.WriteLine("Type:= " + propertyType.ToString());
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
                                        var myDate = (DateTime) value; // Convert.ToDateTime(value);
                                        PersianCalendar persianCalendar = new PersianCalendar();
                                        if (persianCalendar.GetYear(myDate) >= 1753)
                                        {
                                            // BindedEntity[propertyName] = myDate;
                                            Console.WriteLine(":");
                                            Console.WriteLine(":");
                                            Console.WriteLine(
                                                propertyName + " IS DateTiime :" + myDate + ":" +
                                                myDate.ToLongDateString());
                                            Console.WriteLine(":");
                                            Console.WriteLine(":");
                                        }
                                        else
                                        {
                                            // BindedEntity[propertyName] = myDate;
                                            Console.WriteLine(":");
                                            Console.WriteLine(":");
                                            Console.WriteLine(
                                                propertyName + " IS DateTiime < 1753 :" + myDate + ":" +
                                                myDate.ToLongDateString());
                                            Console.WriteLine(":");
                                            Console.WriteLine(":");
                                        }
                                        BindedEntity[propertyName] = DateTime.Now;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Can not Convert System.DateTime " + propertyName);
                                    }
                                    break;

                                case "Microsoft.Xrm.Sdk.OptionSetValue":
                                    BindedEntity[propertyName] = value;
                                    break;
                                case "Microsoft.Xrm.Sdk.EntityReference":
                                    EntityReference Ref = (EntityReference) value;
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

                    catch (NullReferenceException)
                    {
                        // Do not thing
                    }
                    catch (Exception)
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

        public Entity Bind<T>(Entity xrmEntity)
        {
            BindEntity(xrmEntity);
            return BindedEntity;
        }
    }
}