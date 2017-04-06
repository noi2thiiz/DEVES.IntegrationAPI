using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class CRMPolicyMotorDataGateWay: IDataGateWay
    {

         public DbResult FetchAll(DataRequest req)
         {
          
          
            var conectionString =  CrmConfigurationSettings.AppConfig.Get("CRMDB");
            StoreDataReader reader = new StoreDataReader(conectionString);
           
            req.StoreName = "sp_API_InquiryPolicyMotorList";
           
            DbResult result = reader.Execute(req);

     
            return result;
         }


    }
}