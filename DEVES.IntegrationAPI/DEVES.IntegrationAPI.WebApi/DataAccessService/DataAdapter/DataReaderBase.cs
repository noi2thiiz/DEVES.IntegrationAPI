using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public class DataReaderBase
    {
        public TEntityClass Tranform<TEntityClass>(dynamic item)
        {
            var obj = (TEntityClass)Activator.CreateInstance(typeof(TEntityClass));
            foreach (PropertyInfo pi in typeof(TEntityClass).GetProperties())
            {
                var piName = pi.Name;
                if (!pi.CanWrite) continue;
                try
                {
                    pi.SetValue(obj, item[piName], null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "===>" + piName);
                }
            }

            return obj;
        }

        
    }
}