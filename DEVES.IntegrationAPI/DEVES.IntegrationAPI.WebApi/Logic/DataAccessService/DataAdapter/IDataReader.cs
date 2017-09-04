using System.Collections.Generic;
using System.Data.SqlClient;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public interface IDataReader
    {
     

        DbResult Execute(DbRequest req);
        DbResult Execute<T>(DbRequest req);
    }
}