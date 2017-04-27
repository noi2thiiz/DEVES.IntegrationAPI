using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;

namespace DEVES.IntegrationAPI.Core.DataAdepter
{
    public interface IDataAdapter
    {
        DbResult FetchRow(DbRequest req);
        DbResult FetchAll(DbRequest req);
        DbResult Find(string id);
    }
}
