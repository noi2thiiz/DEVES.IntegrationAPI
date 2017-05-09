using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator
{
    public class MasterDataValidator
    {
        protected OutputModelFailData fieldErrorData;
        public MasterDataValidator()
        {
             fieldErrorData = new OutputModelFailData();
        }

        public OutputModelFailData GetFieldErrorData()
        {
            return fieldErrorData;
        }
        internal bool Invalid()
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

        public string TryConvertSalutationCode(string fieldName,string masterCode,string defaultCode ="")
        {
            try
            {
                var masterSalutation =
                    PersonalTitleMasterData.Instance.FindByCode(masterCode, defaultCode);
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
            catch (Exception)
            {
                //do nothing
            }

            return defaultCode;


        }

        public string TryConvertNationalityCode(string fieldName, string masterCode, string defaultCode = "00203")
        {
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

        

        public string TryConvertOccupationCode(string fieldName, string masterCode, string defaultCode = "")
        {
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

        public string TryConvertCountryCode(string fieldName, string masterCode, string defaultCode = "00220")
        {
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

        
        public string TryConvertSubDistrictCode(string fieldName, string masterCode, string defaultCode = "000000")
        {
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
    }
}