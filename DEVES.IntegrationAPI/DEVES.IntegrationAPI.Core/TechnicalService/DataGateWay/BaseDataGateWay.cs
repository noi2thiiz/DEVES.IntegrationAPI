using DEVES.IntegrationAPI.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.DataGateWay
{
    class BaseDataGateWay: IDataGateWay
    {
        IDataAdapter DataAdepter;
        public BaseDataGateWay(IDataAdapter dataAdepter)
        {
            DataAdepter = dataAdepter;
        }

        public DbResult FetchAll(DbRequest req)
        {

            return DataAdepter.FetchRow(req);
        }

        public DbResult FetchRow(DbRequest req)
        {

            return DataAdepter.FetchRow(req);
        }

        public DbResult Find(string id)
        {

            return DataAdepter.Find(id);
        }

    }
}
