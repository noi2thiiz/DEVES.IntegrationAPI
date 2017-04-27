using DEVES.IntegrationAPI.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.DataGateWay
{
    public class BaseDataGateWay: IDataGateWay
    {
        private DEVES.IntegrationAPI.Core.DataAdepter.IDataAdapter dataAdepter;

        public DEVES.IntegrationAPI.Core.DataAdepter.IDataAdapter GetDataAdepter()
        {
            return dataAdepter;
        }

        public void SetDataAdepter(DEVES.IntegrationAPI.Core.DataAdepter.IDataAdapter value)
        {
            dataAdepter = value;
        }

        public DbResult FetchAll(DbRequest req)
        {

            return GetDataAdepter().FetchRow(req);
        }

        public DbResult FetchRow(DbRequest req)
        {

            return GetDataAdepter().FetchRow(req);
        }

        public DbResult Find(string id)
        {

            return GetDataAdepter().Find(id);
        }

    }
}
