using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RegComplaint
{
    public class RegComplaintInputModel
    {
        public string Ticketnumber { get; set; }
        public Guid IncidentId { get; set; }
        public Guid CurrentUserId { get; set; }
    }

    public class Request_RegComplaintModel : BaseDataModel
    {
        [CrmMapping(FieldName = "compResolve", Source = ENUMDataSource.srcSQL)]
        public string compResolve { get; set; } = "";

        [CrmMapping(FieldName = "compIdcard", Source = ENUMDataSource.srcSQL)]
        public string compIdcard { get; set; } = "";

        [CrmMapping(FieldName = "compRegno", Source = ENUMDataSource.srcSQL)]
        public string compRegno { get; set; } = "";

        [CrmMapping(FieldName = "compEmail", Source = ENUMDataSource.srcSQL)]
        public string compEmail { get; set; } = "";

        [CrmMapping(FieldName = "compClaim", Source = ENUMDataSource.srcSQL)]
        public string compClaim { get; set; } = "";

        [CrmMapping(FieldName = "compPolicy", Source = ENUMDataSource.srcSQL)]
        public string compPolicy { get; set; } = "";

        [CrmMapping(FieldName = "chanInform", Source = ENUMDataSource.srcSQL)]
        public string chanInform { get; set; } = "";

        [CrmMapping(FieldName = "compCustcompany", Source = ENUMDataSource.srcSQL)]
        public string compCustcompany { get; set; } = "";

        [CrmMapping(FieldName = "empNo", Source = ENUMDataSource.srcSQL)]
        public string empNo { get; set; } = "";

        [CrmMapping(FieldName = "compCustname", Source = ENUMDataSource.srcSQL)]
        public string compCustname { get; set; } = "";

        [CrmMapping(FieldName = "compDetail", Source = ENUMDataSource.srcSQL)]
        public string compDetail { get; set; } = "";

        [CrmMapping(FieldName = "contrChanel", Source = ENUMDataSource.srcSQL)]
        public string contrChanel { get; set; } = "";

        [CrmMapping(FieldName = "compMobile", Source = ENUMDataSource.srcSQL)]
        public string compMobile { get; set; } = "";

        [CrmMapping(FieldName = "compAddr", Source = ENUMDataSource.srcSQL)]
        public string compAddr { get; set; } = "";

        public string compDate
        {
            get
            {
                string s = "";
                if (dtCompDate != null)
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    s = dtCompDate.Value.ToString(DateTimeCustomFormat, enUS);

                }
                return s;
            }
        }
        [JsonIgnore]
        [CrmMapping(FieldName = "compDate", Source = ENUMDataSource.srcSQL)]
        public DateTime? dtCompDate { get; set; }

        [CrmMapping(FieldName = "compType", Source = ENUMDataSource.srcSQL)]
        public string compType { get; set; } = "";

        [CrmMapping(FieldName = "compCusttype", Source = ENUMDataSource.srcSQL)]
        public string compCusttype { get; set; } = "";

        [CrmMapping(FieldName = "compFax", Source = ENUMDataSource.srcSQL)]
        public string compFax { get; set; } = "";

        [CrmMapping(FieldName = "cntType", Source = ENUMDataSource.srcSQL)]
        public string cntType { get; set; } = "";

        [CrmMapping(FieldName = "caseNo", Source = ENUMDataSource.srcSQL)]
        public string caseNo { get; set; } = "";

        public string kpvDate
        {
            get
            {
                string s = "";
                if (dtkpvDate != null)
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    s = dtkpvDate.Value.ToString(DateTimeCustomFormat, enUS);

                }
                return s;
            }
        }
        [JsonIgnore]
        [CrmMapping(FieldName = "kpvDate", Source = ENUMDataSource.srcSQL)]
        public DateTime? dtkpvDate { get; set; }

        [CrmMapping(FieldName = "compPhone", Source = ENUMDataSource.srcSQL)]
        public string compPhone { get; set; } = "";


    }
}
