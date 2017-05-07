using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DAL
/// </summary>
namespace SubjectSelection
{
    public static class DAL
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["DBCRM"].ToString();

        public static DataSet executeDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Data");
            conn.Close();

            return ds;
        }
    }
}