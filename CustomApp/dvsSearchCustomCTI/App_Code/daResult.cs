using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace devesCustomCTI.BusinessEntities
{
    public class daResult
    {
        public enum EnumResultCode { OK, Error }

        public EnumResultCode ResultCode;
        public string ResultCodestr;
        public string ResultDescription;

        public daResult() { ResultCode = EnumResultCode.Error; }

        #region [CreateDataSetResultBox]
        private const string colName_RESULT_CODE = "RESULT_CODE";
        private const string colName_RESULT_DESC = "RESULT_DESC";
        public DataSet CreateDataSetResultBox(string tableName, daResult ret)
        {
            DataSet retDS = new DataSet();
            DataTable retTB = new DataTable(tableName);

            retTB.Columns.Add(colName_RESULT_CODE, typeof(String));
            retTB.Columns.Add(colName_RESULT_DESC, typeof(String));
            DataRow retRow = retTB.NewRow();
            retRow[colName_RESULT_CODE] = ret.ResultCode == daResult.EnumResultCode.OK ? "0" : "1";
            retRow[colName_RESULT_DESC] = ret.ResultDescription;
            retTB.Rows.Add(retRow);

            retDS.Tables.Add(retTB);

            return retDS;
        }
        #endregion

        #region [CreateStringResultBox]
        public string[] CreateStringResultBox(daResult ret)
        {
            return new string[] { ret.ResultCode == daResult.EnumResultCode.OK ? "0" : "1", ret.ResultDescription };
        }
        #endregion
    }
}