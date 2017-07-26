using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.personSearchModel
{
    public class personSearchOutputModel : BaseContentJsonProxyOutputModel
    {
  
        public List<personSearchDataOutput> Persondata { get; set; }
        public List<corpSearchDataOutput>  Corpdata { get; set; }
    }

    public class personSearchDataOutput : BaseDataModel
    {
        public string fullName { get; set; }
        public string idCard { get; set; }
        public string emailAddress { get; set; }
        public string cleansingId { get; set; }
        public string crmClientId { get; set; }
        public string mobilePhone { get; set; }
        public string salutationText { get; set; }
        public string sex { get; set; }
        public string idPassport { get; set; }
        public string idAlien { get; set; }
        public string idDriving { get; set; }
        public string idTax { get; set; }
        public string corporateBranch { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime dateOfDeath { get; set; }
        public string nationalityText { get; set; }
        public string marriedText { get; set; }
        public string occupationText { get; set; }
        public string econActivityText { get; set; }
        public string countryOriginText { get; set; }
        public string riskLevelText { get; set; }
        public string language { get; set; }
        public string vipStatus { get; set; }
        public string clientStatus { get; set; }
        public string remark { get; set; }
        public string fax { get; set; }
        public string contactNumber { get; set; }
        public string lineID { get; set; }
        public string facebook { get; set; }
        public string polisyClientId { get; set; }

    }

    public class corpSearchDataOutput : BaseDataModel
    {
        public string fullName { get; set; }
        public string  idRegCorp { get; set; }
        public string idTax { get; set; }
        public string corporateBranch { get; set; }
        public DateTime corporateDate { get; set; }
        public string riskLevelText { get; set; }
        public string language { get; set; }
        public string clientStatus { get; set; }
        public string vipStatus { get; set; }
    }

}
