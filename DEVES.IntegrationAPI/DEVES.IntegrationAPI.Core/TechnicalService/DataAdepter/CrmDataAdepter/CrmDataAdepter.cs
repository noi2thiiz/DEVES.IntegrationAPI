using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;

namespace DEVES.IntegrationAPI.Core.DataAdepter
{
    public class CrmDataAdepter : IDataAdapter
    {
        protected string ConnectionString;
        protected string EntityName;

  

        public DbResult FetchAll(DbRequest req)
        {
            throw new NotImplementedException();
        }

        public DbResult FetchRow(DbRequest req)
        {
            throw new NotImplementedException();
        }

        public DbResult Find(string id)
        {
            throw new NotImplementedException();
        }
    }
}
