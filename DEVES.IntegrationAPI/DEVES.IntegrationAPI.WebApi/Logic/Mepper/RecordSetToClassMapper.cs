using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic.Mepper
{
    public class DataModelMapper
    {
        public static ToutputModel MapToModel<ToutputModel>(dynamic item)
        {
            var obj = (ToutputModel)Activator.CreateInstance(typeof(ToutputModel));
            foreach (PropertyInfo pi in typeof(ToutputModel).GetProperties())
            {
                var piName = pi.Name;
                if (!pi.CanWrite) continue;


                try
                {
                    //AdHoc  Convert เฉพาะที่ใช้เท่านั้น
                    Console.WriteLine(pi.PropertyType.Name + " to " + item[piName].GetType());
                    if (pi.PropertyType.Name == "System.String" && item[piName].GetType() == "System.Guid")
                    {
                        pi.SetValue(obj, ((Guid)item[piName]).ToString("D"), null);
                    }
                    else if (pi.PropertyType.Name == "System.Guid" && item[piName].GetType() == "System.String")
                    {
                        pi.SetValue(obj, new Guid(item[piName]?.ToString()), null);
                    }
                    else if (pi.PropertyType.Name == "System.String")
                    {
                        pi.SetValue(obj, new Guid(item[piName]?.ToString()), null);
                    }
                    else
                    {
                        pi.SetValue(obj, item[piName], null);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Fail Tranform Entity in BaseDataBaseContracts:" + e.Message + "===>" + piName);
                }
            }

            return obj;
        }
    }
}