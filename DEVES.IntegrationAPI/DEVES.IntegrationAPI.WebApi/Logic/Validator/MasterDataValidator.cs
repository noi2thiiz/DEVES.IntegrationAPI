using System;
using System.Linq;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator
{
    public class MasterDataValidator
    {
        public OutputModelFailData fieldErrorData;
        public MasterDataValidator()
        {
             fieldErrorData = new OutputModelFailData();
        }

        public OutputModelFailData GetFieldErrorData()
        {
            return fieldErrorData;
        }
        public bool Invalid()
        {
            return fieldErrorData.fieldErrors.Any();
        }


        public bool ValidateContryCode()
        {
            return true;
        }

        public bool ValidateSubDistrictCode()
        {
            return true;
        }

        public bool ValidateDistrictCode()
        {
            return true;
        }

        public bool ValidateProvinceCode()
        {
            return true;
        }

        public string TryConvertSalutationCode(string fieldName,string masterCode,string defaultCode = "0001")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            try
            {
                var masterSalutation =
                    PersonalTitleMasterData.Instance.FindByCode(masterCode);
                if (masterSalutation != null)
                {
                    return masterSalutation.PolisyCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Salutation", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+":"+e.StackTrace);
                //do nothing
            }

            return defaultCode;


        }

        public string TryConvertNationalityCode(string fieldName, string masterCode, string defaultCode = "00203")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            try
            {
                var masterSalutation =
                    NationalityMasterData.Instance.FindByCode(masterCode, defaultCode);
                if (masterSalutation != null)
                {
                    return masterSalutation.PolisyCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Nationality", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;


        }

        

        public string TryConvertOccupationCode(string fieldName, string masterCode, string defaultCode = "00023")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            try
            {
                var master =
                    OccupationMasterData.Instance.FindByCode(masterCode, defaultCode);
                if (master != null)
                {
                    return master.PolisyCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Occupation", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;


        }

        public string TryConvertCountryPolisyCode(string fieldName, string polisyCode, string defaultCode = "764")
        {
            if (string.IsNullOrEmpty(polisyCode))
            {
                polisyCode = defaultCode;
            }
            try
            {
                var master =
                    CountryMasterData.Instance.FindByCode(polisyCode, defaultCode);
                if (master != null)
                {
                    return master.CountryCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Country", polisyCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;

        }

        public string TryConvertCountryCode(string fieldName, string masterCode, string defaultCode = "00220")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            try
            {
                var master =
                    CountryMasterData.Instance.FindByCode(masterCode, defaultCode);
                if (master != null)
                {
                    return master.PolisyCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Country", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;
            
        }
        
        public string TryConvertProvinceCode(string fieldName, string masterCode, string countryCode= "00220", string defaultCode = "00")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            try
            {
                var master =
                    ProvinceMasterData.Instance.FindByCode(masterCode, defaultCode);
                if (master != null)
                {
                    return master.ProvinceCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Province", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;

        }

        public string TryConvertDistrictCode(string fieldName, string masterCode, string provinceCode = "0000", string defaultCode = "0000")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            try
            {
                var master =
                    DistricMasterData.Instance.FindByCode(masterCode, defaultCode);
                if (master != null)
                {
                    return master.DistrictCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("District", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;

        }

        public string TryConvertSubDistrictCode(string fieldName, string masterCode, string provinceCode = "00", string districtCode = "0000", string defaultCode = "000000")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }

            try
            {
                var master =
                    SubDistrictMasterData.Instance.FindByCode(masterCode, defaultCode);
                if (master != null)
                {
                    return master.SubDistrictCode;
                }
                else
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Country", masterCode);
                    fieldErrorData.AddFieldError(fieldName, errorMessage);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;


        }

       protected string[] AddressTypeArray = {"01","02","03","04","05","06","07","08" };

        public string TryConvertAddressTypeCode(string fieldName, string masterCode, string defaultCode = "01")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                masterCode = defaultCode;
            }
            if (masterCode == "00")
            {
                masterCode = "01";
            }

            if (AddressTypeArray.Contains(masterCode) == false)
            {
                var errorMessage =
                    MessageBuilder.Instance.GetInvalidMasterMessage("AddressType", masterCode);
                fieldErrorData.AddFieldError(fieldName, errorMessage);
            }

            return masterCode;

        }

        protected string[] EconActivityArray = { "001", "002", "003", "004", "005", "006", "007", "008" };

        public string TryConvertEconActivityCode(string fieldName, string masterCode, string defaultCode = "")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
              return defaultCode;
                
            }
            //allow null
            //ไม่ต้อง return error ถ้าใส่ค่า emtpy มา
            if (EconActivityArray.Contains(masterCode) == false)
            {
                var errorMessage =
                    MessageBuilder.Instance.GetInvalidMasterMessage("EconActivity", masterCode);
                fieldErrorData.AddFieldError(fieldName, errorMessage);
            }

            return masterCode;

        }

       
    }
}