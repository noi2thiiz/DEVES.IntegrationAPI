using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.QuerySQL
{
    public class QuerySQLOutputModel
    {
        public string databaseName { get; set; }
        public string sqlCommand { get; set; }
        public string message { get; set; }
        public System.Data.DataTable dt { get; set; }
    }
}
