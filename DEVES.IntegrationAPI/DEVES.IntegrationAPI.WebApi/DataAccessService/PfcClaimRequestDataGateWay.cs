using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.WebApi.CrmApi.DataAccessService.Xrm.Attributes;
using DEVES.IntegrationAPI.WebApi.Services.DataGateWay;
using DEVES.IntegrationAPI.WebApi.Services.Enumerations;
using DEVES.IntegrationAPI.WebApi.Services.Models.RegClaimRequestFromClaimDi;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace DEVES.IntegrationAPI.WebApi.CrmApi.DataAccessService.DataGateWay
{
    public class PfcClaimRequestDataGateWay : CrmSdkTableGateWayAbstract
    {
        public string EntityName = "pfc_eclaim_request";


        public void BindParams(RegClaimRequestFromClaimDiRequestModel model)
        {
            //  PfcVoiceRecode["pfc_activityid"] = new EntityReference("pfc_activityid",  aircraftGuid);;
            // AddAttribute(entity,"pfc_eclaim_requestid",model.);


            //default
            AddAttribute<OptionSetValue>("pfc_accident_legal_result", new OptionSetValue(100000000));



            try
            {
                AddAttribute<int>("pfc_event_lert_id", value: model.eventLertId);
                AddAttribute("pfc_name",(model.eventLertId.ToString()).Trim());
            }
            catch (Exception e)
            {

            }


            try
            {
                if (model.lertInfo != null)
                {
                    AddAttribute<int>("pfc_lert_id", model.lertInfo.lertId);


                    try
                    {
                        if (model.lertInfo.lertDateTime != "")
                        {
                            AddAttribute<DateTime>("pfc_lert_datetime", StringToDateTime(model.lertInfo.lertDateTime));
                        }
                    }
                    catch (Exception e)
                    {

                    }



                    AddAttribute("pfc_mobile_no", model.lertInfo.telNo);

                }

            }
            catch (Exception e)
            {Console.WriteLine(e.Message+e.StackTrace);

            }

            try
            {
                if (model.lertInfo != null)
                {
                    AddAttribute("pfc_lert_by", model.lertInfo.lertBy);

                }

            }
            catch (Exception e)
            {Console.WriteLine(e.Message+e.StackTrace);

            }



            if (model.policyOwnerInfo != null)
            {

                AddAttribute("pfc_policy_no", model.policyOwnerInfo.policyNo);
                AddAttribute("pfc_current_reg_num", model.policyOwnerInfo.carLicenseNo);
                AddAttribute("pfc_current_reg_num_prov", model.policyOwnerInfo.carLicense_province);
                AddAttribute("pfc_owner_first_name", model.policyOwnerInfo.ownerFirstName);
                AddAttribute("pfc_owner_last_name", model.policyOwnerInfo.ownerLastName);
                AddAttribute("pfc_owner_mobile", model.policyOwnerInfo.ownerMobileNo);
                AddAttribute("pfc_driver_first_name", model.policyOwnerInfo.driverFirstName);
                AddAttribute("pfc_driver_last_name", model.policyOwnerInfo.driverLastName);
                AddAttribute("pfc_driver_mobile", model.policyOwnerInfo.driverMobileNo);
                AddAttribute<Enum>("pfc_case_result", model.policyOwnerInfo.caseResult);

                try
                {
                    if (model.policyOwnerInfo.caseDatetime != "")
                    {
                        AddAttribute<DateTime>("pfc_lert_datetime", StringToDateTime(model.policyOwnerInfo.caseDatetime));
                    }
                }
                catch (Exception e)
                {

                }



                    //Extra Field
                    AddAttribute("pfc_eclaim_request_name",
                        (model.policyOwnerInfo.ownerFirstName + " " + model.policyOwnerInfo.ownerLastName).Trim());

                  //  AddAttribute("pfc_name",
                  //      (model.policyOwnerInfo.ownerFirstName + " " + model.policyOwnerInfo.ownerLastName).Trim());


                if (model.policyOwnerInfo.caseResult != null)
                {
                    switch (model.policyOwnerInfo.caseResult)
                    {
                        case CaseResultOptions.NONE:
                            AddAttribute<OptionSetValue>("pfc_accident_legal_result", new OptionSetValue(100000000));
                            break;
                        case CaseResultOptions.RIGHT:
                            AddAttribute<OptionSetValue>("pfc_accident_legal_result", new OptionSetValue(100000001));
                            break;
                        case CaseResultOptions.WRONG:
                            AddAttribute<OptionSetValue>("pfc_accident_legal_result", new OptionSetValue(100000002));
                            break;
                        case CaseResultOptions.NEGLIGENT:
                            AddAttribute<OptionSetValue>("pfc_accident_legal_result", new OptionSetValue(100000003));
                            break;
                        default:
                            AddAttribute<OptionSetValue>("pfc_accident_legal_result", new OptionSetValue(100000000));
                            break;
                    }
                    ;
                }

            }




            if (model.location != null)
            {
                AddAttribute("pfc_accident_latitude", model.location.latitude);
                AddAttribute("pfc_accident_longitude", model.location.longitude);
                AddAttribute("pfc_accident_place", model.location.place);
                AddAttribute("pfc_accident_province", "");
                AddAttribute("pfc_accident_district", "");
            }

            if (model.requestChanel != null)
            {
                switch (model.requestChanel)
                {
                    case RequestChanelOption.ilertu:
                        AddAttribute<OptionSetValue>("pfc_data_source", new OptionSetValue(100000000));
                        break;
                    case RequestChanelOption.KFK:
                        AddAttribute<OptionSetValue>("pfc_data_source", new OptionSetValue(100000001));
                        break;
                }
            }
        }


        public Guid Create(RegClaimRequestFromClaimDiRequestModel model)
        {
            try
            {
                var _p = getOrganizationServiceProxy();
                BindedEntity = new Entity(EntityName);
                this.BindParams(model);

                var guid = _p.Create(BindedEntity);

                foreach (ThirdPartyInfo info in model.thirdPartyInfo)
                {
                    PfcEclaimPartiesRequestDataGateWay g2 = new PfcEclaimPartiesRequestDataGateWay();
                    g2.Create(info, guid);
                }


                return guid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                throw new Exception("Error on Create:" + e.Message + e.StackTrace);
            }
        }

        public static EntityMetadata[] GetEntities(IOrganizationService organizationService)
        {
            Dictionary<string, string> attributesData = new Dictionary<string, string>();
            RetrieveAllEntitiesRequest metaDataRequest = new RetrieveAllEntitiesRequest();
            RetrieveAllEntitiesResponse metaDataResponse = new RetrieveAllEntitiesResponse();
            metaDataRequest.EntityFilters = EntityFilters.Entity;

            // Execute the request.

            metaDataResponse = (RetrieveAllEntitiesResponse) organizationService.Execute(metaDataRequest);

            var entities = metaDataResponse.EntityMetadata;

            return entities;
        }
    }

    public class PfcClaimRequestEntity
    {
        //pfc_eclaim_requestid	เลขที่รับแจ้ง Eclaim	single line of text(20)
        [XrmField("pfc_eclaim_requestid")]
        public string pfc_eclaim_requestid { get; set; }


        //pfc_eclaim_request_name	รายละเอียด Eclaim	single line of text(100)

        [XrmField(@"pfc_eclaim_request_name")]
        public string pfc_eclaim_request_name { get; set; }


        //pfc_event_lert_id	event lert id	whole number
        //pfc_lert_id	เลขที่ของข้อมูลการแจ้งเหตุ	whole number
        //pfc_lert_datetime	วันที่แจ้งเหตุ	datetime
        //pfc_lert_by	ชื่อผู้แจ้ง	single line of text(255)
        //pfc_mobile_no	เบอร์ติดต่อ	single line of text(20)
        //pfc_policy_no	เลขที่กรมธรรม์	single line of text(20)
        //pfc_current_reg_num	ทะเบียนรถยนต์	single line of text(20)
        //pfc_current_reg_num_prov	จังหวัดทะเบียนรถ	single line of text(20)
        //pfc_owner_first_name	ชื่อ	single line of text(60)
        //pfc_owner_last_name	นามสกุล	single line of text(60)
        //pfc_owner_mobile	เบอร์ติดต่อ	single line of text(20)
        //pfc_driver_first_name	ชื่อผู้ขับขี่	single line of text(60)
        //pfc_driver_last_name	นามสกุลคู่ขับขี่	single line of text(60)
        //pfc_driver_mobile	เบอร์ติดต่อผู้ขับขี่	single line of text(20)
        //pfc_case_result	ผลคดี ประกอบด้วย 	single line of text(10)
        //pfc_case_datetime	วันที่เกิดเหตุ	datetime
        //pfc_accident_latitude	Latitude ที่เกิดเหตุ	single line of text(20)
        //pfc_accident_longitude	Longitude ที่เกิดเหตุ	single line of text(20)
        //pfc_accident_place	สถานที่เกิดเหตุ	Multiple Line of text (2000)
        //pfc_accident_province	จังหวัดที่เกิดเหตุ	single line of text(50)
        //pfc_accident_district	อำเภอที่เกิดเหตุ	single line of text(50)
        //pfc_data_source	Data Source	option set
    }
}