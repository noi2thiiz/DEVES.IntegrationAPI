using System;
using System.Reflection;
using DEVES.IntegrationAPI.WebApi.Core.Attributes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEVES.IntegrationAPI.Core.TechnicalService.DataManager
{
    public class XrmTranformer<TModelClass>
        where TModelClass : class
    {
     protected XrmAttributeReflector AttrReflector = new XrmAttributeReflector();

        public TModelClass Tranform(Entity entity)
        {


            var obj = (TModelClass)Activator.CreateInstance(typeof(TModelClass));
            // PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);


            var attributes = new ColumnSet(new string[] { });
            var properties = AttrReflector.GetProperties<TModelClass>();
            var IgnoreAttributes = AttrReflector.IgnoreAttributes;
            foreach (PropertyInfo pi in properties)
            {
                
                if (null != pi && pi.CanWrite )
                {
                    string piName = pi.Name.ToLower();
                    if (IgnoreAttributes.Contains(piName)) continue;
                   
                    try
                    {
                        var attr = pi.GetCustomAttribute<XrmAttributeMappingAttribute>();
                        var xrmFieldName = attr.FieldName;
                        pi.SetValue(obj,
                            attr.FieldKey == EntityFieldKey.PK
                                ? new Guid(entity.Id.ToString())
                                : entity[xrmFieldName], null);
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

        public TModelClass TranformEntity(Entity entity)
        {

            var obj =  (TModelClass)Activator.CreateInstance(typeof(TModelClass));
            // PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);

            var IgnoreAttributes = AttrReflector.IgnoreAttributes;
            var attributes = new ColumnSet(new string[] { });
            foreach (PropertyInfo pi in typeof(TModelClass).GetProperties())
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
        public TModelClass TranformEntityWithAttribute(Entity entity)
        {

            var obj = (TModelClass)Activator.CreateInstance(typeof(TModelClass));
            // PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);

            var ignoreAttributes = AttrReflector.IgnoreAttributes;
         
            foreach (var pi in typeof(TModelClass).GetProperties())
            {
                var piName = pi.Name.ToLower();
                if (pi.CanWrite && !ignoreAttributes.Contains(piName))
                {
                    try
                    {
                        var attr = pi.GetCustomAttribute<XrmAttributeMappingAttribute>();
                        var xrmFieldName = attr.FieldName;
                        Console.WriteLine(entity[xrmFieldName] + "===>" + xrmFieldName);
                        pi.SetValue(obj,
                            attr.FieldKey == EntityFieldKey.PK
                                ? new Guid(entity.Id.ToString())
                                : entity[xrmFieldName], null);
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
    }
}