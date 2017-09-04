# DEVES.IntegrationAPI

###Api List


CRM URL   (QA) | CRM URL (PRD) | Test (Endpoint) | EWI Proxy Url | System Request
-- | -- | -- | -- | --
http://192.168.8.121/XrmApi/api/CRMInquiryClientMaster | http://crmapi.deves.co.th/client-service/api/CRMInquiryClientMaster | Pass | /ServiceProxy/ClaimMotor/jsonservice/CRM_InquiryClientMaster | Locus, Ö
http://192.168.8.121/XrmApi/api/RegClientPersonal | http://crmapi.deves.co.th/client-service/api/RegClientPersonal | Pass | /ServiceProxy/ClaimMotor/jsonservice/CRM_RegClientPersonal | Locus, Ö
http://192.168.8.121/XrmApi/api/RegClientCorporate | http://crmapi.deves.co.th/client-service/api/RegClientCorporate | Pass | /ServiceProxy/ClaimMotor/jsonservice/CRM_RegClientCorporate | Locus, Ö
http://192.168.8.121/XrmApi/api/InquiryCRMPayeeList | http://crmapi.deves.co.th/client-service/api/InquiryCRMPayeeList | Pass | /ServiceProxy/ClaimMotor/jsonservice/CRM_InquiryPayeeList | Locus, Ö
http://192.168.8.121/XrmApi/api/RegPayeePersonal | http://crmapi.deves.co.th/client-service/api/RegPayeePersonal | Pass | /ServiceProxy/ClaimMotor/jsonservice/CRM_RegPayeePersonal | Locus, Ö
http://192.168.8.121/XrmApi/api/RegPayeeCorporate | http://crmapi.deves.co.th/client-service/api/RegPayeeCorporate | Pass | /ServiceProxy/ClaimMotor/jsonservice/CRM_RegPayeeCorporate | Locus, Ö
http://192.168.8.121/XrmApi/api/RequestSurveyor† (internal api) | † | † | /ServiceProxy/ClaimMotor/jsonservice/MOTOR_RequestSurveyor | CRM
http://192.168.8.121/XrmApi/api/AssignedSurveyor | http://crmapi.deves.co.th/isurvey-service/api/AssignedSurveyor | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_AssignedSurveyor | i-Survey
http://192.168.8.121/XrmApi/api/UpdateSurveyStatus | http://crmapi.deves.co.th/isurvey-service/api/UpdateSurveyStatus | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_UpdateSurveyStatus | i-Survey
http://192.168.8.121/XrmApi/api/AccidentPrilimSurveyorReport | http://crmapi.deves.co.th/isurvey-service/api/AccidentPrilimSurveyorReport | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_AccidentPrilimSurveyorReport | i-Survey
† | † | † | /ServiceProxy/ClaimMotor/jsonproxy/LOCUS_ClaimRegistration | CRM
http://192.168.8.121/XrmApi/api/UpdateClaimInfo | http://crmapi.deves.co.th/claim-service/api/UpdateClaimInfo | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_UpdateClaimInfo | Locus
http://192.168.8.121/XrmApi/api/InquiryConsultingHistory | http://crmapi.deves.co.th/claim-service/api/InquiryConsultingHistory | † | /ServiceProxy/ClaimMotor/jsonproxy/CRM_InquiryConsultingHistory | Locus
http://192.168.8.121/claimdi-integration-test/api/RegClaimRequestFromClaimDi | http://crmapi.deves.co.th/e-claim-service/api/RegClaimRequestFromClaimDi | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_RegClaimRequestFromClaimDi | i-Survey   (ClaimDi)
† | † | † | † | CRM
† | † | † | /ServiceProxy/ClaimMotor/jsonservice/regisComplaint | CRM
http://192.168.8.121/XrmApi/api/UpdateCompliantStatus | http://crmapi.deves.co.th/claim-service/api/UpdateCompliantStatus | † | /ServiceProxy/ClaimMotor/jsonproxy/CRM_UpdateCompliantStatus | Complaint   Control System
http://192.168.8.121/rvp-integration-test/api/RegClaimRequestFromRVP | http://crmapi.deves.co.th/claim-service/api/RegClaimRequestFromRVP | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_RegClaimRequestFromRVP | Locus
http://192.168.8.121/XrmApi/api/InquiryPolicyMotorListClient | http://crmapi.deves.co.th/claim-service/api/InquiryPolicyMotorListClient | † | /ServiceProxy/ClaimMotor/jsonproxy/CRM_InquiryPolicyMotorListClient | Locus
http://192.168.8.121/XrmApi/api/UpdateClaimNo | http://crmapi.deves.co.th/claim-service/api/UpdateClaimNo | Pass | /ServiceProxy/ClaimMotor/jsonproxy/CRM_UpdateClaimNo | Locus
† | † | † | † | CRM
† | http://crmapp.deves.co.th/cti-integration-service/api/cti-web-dialer/MakeCall† („™È¿“¬„π√–∫∫ CRM ‡∑Ë“π—Èπ) | † | † | CRM
http://192.168.8.121/cti-integration-test/api/CTIWebDialer/VoiceRecord | http://crmapi.deves.co.th/cti-integration-service/api/CTIWebDialer/VoiceRecord | † | † | CRM

