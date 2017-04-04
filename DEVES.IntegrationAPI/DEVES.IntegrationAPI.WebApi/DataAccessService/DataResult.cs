using System.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class DataResult<T>
    {
        public int Count = 0;
        public List<T> Data { get; }

        public DataResult(List<T> data)
        {
            Count = data.Count;
            Data = data;
        }
    }
}