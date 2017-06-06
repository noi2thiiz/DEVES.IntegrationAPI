function initvariable(){

    var hostname = window.location.hostname;
    window.top.systemUserRoles = getCurrentUser();
    window.top.qaurl = "crmqa.deves.co.th";
    window.top.devurl = "crmdev.deves.co.th";
    window.top.produrl = "crm.deves.co.th";
    window.top.xrmApiRootPart = "https://crmappqa.deves.co.th/XrmApi/Api";
    window.top.internalApiRootPart = "https://crmappqa.deves.co.th/internal-service/api";
    window.top.casetypeENUM = {
        name:'Service Request',
        value:'100000001'
    };

    window.top.environmentName = "dev";
    window.top.callCenterLinkHostname = "";
    window.top.callCenterLinkProxyPart = "";
    switch(hostname){



        case "crmqa.deves.co.th":
        case "crmappqa.deves.co.th":
            window.top.environmentName="qa";
            window.top.xrmApiRootPart = "https://crmappqa.deves.co.th/XrmApi/Api";
            window.top.internalApiRootPart = "https://crmappqa.deves.co.th/internal-service/api";
            window.top.callCenterLinkHostname= "http://192.168.10.33";
            window.top.callCenterLinkProxyPart = "service/link-proxy.ashx";

            break;

        case "crm.deves.co.th":
        case "crmapp.deves.co.th":
            window.top.environmentName="pro";
            window.top.xrmApiRootPart = "https://crmapp.deves.co.th/XrmApi/Api";
            window.top.internalApiRootPart = "https://crmapp.deves.co.th/internal-service/api";
            window.top.callCenterLinkHostname = "http://192.168.10.3";
            window.top.callCenterLinkProxyPart = "service/link-proxy.ashx";

            break;

        default:
            window.top.environmentName="dev";
            window.top.xrmApiRootPart = "https://crmappqa.deves.co.th/XrmApi/Api";
            window.top.internalApiRootPart = "https://crmappqa.deves.co.th/internal-service/api";
            window.top.callCenterLinkHostname = "http://192.168.10.33";
            window.top.callCenterLinkProxyPart = "service/link-proxy.ashx";
            break;
    }

    window.top.callCenterLinkToken = "INi9SdhXRvpZO7mYI0ViUlflmqc9I9GO";
    window.top.callCenterLinkNewProductPart = window.top.callCenterLinkHostname+"/NewProduct.asp?token="+window.top.callCenterLinkToken;
    window.top.callCenterLinkInformationPart = window.top.callCenterLinkHostname+"/Information.asp?token="+window.top.callCenterLinkToken;


    window.top.createclaimAPI = window.top.xrmApiRootPart + "/ClaimRegistration";
    window.top.requestsurveyorAPI  = window.top.xrmApiRootPart + "/RequestSurveyor";

    window.top.CRMInquiryClientMasterAPI = window.top.xrmApiRootPart+"/CRMInquiryClientMaster";
    window.top.RegClientPersonalAPI = window.top.xrmApiRootPart+"/RegClientPersonal";
    window.top.RegClientCorporateAPI = window.top.xrmApiRootPart+"/RegClientCorporate";
    window.top.InquiryCRMPayeeListAPI = window.top.xrmApiRootPart+"/InquiryCRMPayeeList";
    window.top.CRMStoreServiceAPI = window.top.internalApiRootPart+"/StoreService/crm";
    window.top.EXTStoreServiceAPI = window.top.internalApiRootPart+"/StoreService/ext";

    if(hostname == "crmqa.deves.co.th"){
        window.top.isurvey = "http://192.168.3.137:82/isurveyTest/";
        window.top.webvehicle = "https://crmqa.deves.co.th//WebResources/pfc_web_vehicle_parts";
        window.top.googlemapCusRegis = "https://crmqa.deves.co.th//WebResources/pfc_googlemap_customer_regis";
        window.top.googlemapMeeting = "https://crmqa.deves.co.th//WebResources/pfc_map_meeting_place";
        window.top.googleMapMotor = "https://crmqa.deves.co.th//WebResources/pfc_googlemap_for_motor";
        window.top.googlemap = "https://crmqa.deves.co.th//WebResources/pfc_geocoder_googlemap";
        window.top.addcalltype = "https://crmappqa.deves.co.th/SubjectSelection/SubjectSelection.aspx";
        window.top.searchClaim = "https://crmappqa.deves.co.th/dvssearch/SearchClaim.aspx";
        window.top.searchCase = "https://crmappqa.deves.co.th/dvssearch/SearchCase.aspx";
        window.top.searchPolicy = "https://crmappqa.deves.co.th/dvssearch/SearchPolicy.aspx";
        window.top.searchCustomer = "https://crmappqa.deves.co.th/dvssearch/SearchCustomer.aspx";
        window.top.searchDriver = "https://crmappqa.deves.co.th/dvssearch/SearchDriver.aspx";
        window.top.searchInformer = "https://crmappqa.deves.co.th/dvssearch/SearchInformer.aspx";
        window.top.createclaimnotiAPI = "https://crmappqa.deves.co.th/GenerateCallerId/GenerateCaller.aspx";
        window.top.categoryENUM = {
            id:'1DCB2B21-0AAB-E611-80CA-0050568D1874', //0A0BBAAC-621A-E711-80D0-0050568D615F
            name:'สินไหม (Motor)',
            logicalname:'pfc_category'
        };
        window.top.subcategoryENUM = {
            id:'2F966072-0AAB-E611-80CA-0050568D1874', //C10061E2-621A-E711-80D0-0050568D615F
            name:'แจ้งอุบัติเหตุรถยนต์',
            logicalname:'pfc_sub_category'
        };
        window.top.appConfig = {

            makeCallApiEnpoint: "https://crmappqa.deves.co.th/cti-integration-test/api/cti-web-dialer/makecall",
        }
        // window.top.claimCar = "c10061e2-621a-e711-80d0-0050568d615f";
        // window.top.claimCarPLB = "c30061e2-621a-e711-80d0-0050568d615f";
        // window.top.claimCarCB = "d90061e2-621a-e711-80d0-0050568d615f";
        window.top.claimCar = "2f966072-0aab-e611-80ca-0050568d1874";
        window.top.claimCarPLB = "951ae07a-0aab-e611-80ca-0050568d1874";
        window.top.claimCarCB = "92d632d8-29ab-e611-80ca-0050568d1874";
    } else if (hostname == "crmdev.deves.co.th"){
        window.top.webvehicle = "https://crmdev.deves.co.th//WebResources/pfc_web_vehicle_parts";
        window.top.googlemapCusRegis = "https://crmdev.deves.co.th//WebResources/pfc_googlemap_customer_regis";
        window.top.googlemapMeeting = "https://crmdev.deves.co.th//WebResources/pfc_map_meeting_place";
        window.top.googleMapMotor = "https://crmdev.deves.co.th//WebResources/pfc_googlemap_for_motor";
        window.top.googlemap = "https://crmdev.deves.co.th//WebResources/pfc_geocoder_googlemap";
        window.top.addcalltype = "https://crmappdev.deves.co.th/dvsSubjectSelection/SubjectSelection.aspx";
        window.top.searchClaim = "https://crmappdev.deves.co.th/dvssearch/SearchClaim.aspx";
        window.top.searchCase = "https://crmappdev.deves.co.th/dvssearch/SearchCase.aspx";
        window.top.searchPolicy = "https://crmappdev.deves.co.th/dvssearch/SearchPolicy.aspx";
        window.top.searchCustomer = "https://crmappdev.deves.co.th/dvssearch/SearchCustomer.aspx";
        window.top.searchDriver = "https://crmappdev.deves.co.th/dvssearch/SearchDriver.aspx";
        window.top.searchInformer = "https://crmappdev.deves.co.th/dvssearch/SearchInformer.aspx";
        window.top.createclaimnotiAPI = "https://crmappdev.deves.co.th/GenerateClaimnoti/GenerateCaller.aspx";
        window.top.isurvey = "http://192.168.3.137:82/isurveyTest/";

        window.top.categoryENUM = {
            id:'1DCB2B21-0AAB-E611-80CA-0050568D1874',
            name:'สินไหม (Motor)',
            logicalname:'pfc_category'
        };
        window.top.subcategoryENUM = {
            id:'2F966072-0AAB-E611-80CA-0050568D1874',
            name:'แจ้งอุบัติเหตุรถยนต์',
            logicalname:'pfc_sub_category'
        };
        window.top.claimCar = "2f966072-0aab-e611-80ca-0050568d1874";
        window.top.claimCarPLB = "951ae07a-0aab-e611-80ca-0050568d1874";
        window.top.claimCarCB = "92d632d8-29ab-e611-80ca-0050568d1874";
    } else if (hostname == "crm.deves.co.th"){
        window.top.webvehicle = "https://crm.deves.co.th//WebResources/pfc_web_vehicle_parts";
        window.top.googlemapCusRegis = "https://crm.deves.co.th//WebResources/pfc_googlemap_customer_regis";
        window.top.googlemapMeeting = "https://crm.deves.co.th//WebResources/pfc_map_meeting_place";
        window.top.googleMapMotor = "https://crm.deves.co.th//WebResources/pfc_googlemap_for_motor";
        window.top.googlemap = "https://crm.deves.co.th//WebResources/pfc_geocoder_googlemap";
        window.top.addcalltype = "https://crmapp.deves.co.th/SubjectSelection/SubjectSelection.aspx";
        window.top.searchClaim = "https://crmapp.deves.co.th/dvssearch/SearchClaim.aspx";
        window.top.searchCase = "https://crmapp.deves.co.th/dvssearch/SearchCase.aspx";
        window.top.searchPolicy = "https://crmapp.deves.co.th/dvssearch/SearchPolicy.aspx";
        window.top.searchCustomer = "https://crmapp.deves.co.th/dvssearch/SearchCustomer.aspx";
        window.top.searchDriver = "https://crmapp.deves.co.th/dvssearch/SearchDriver.aspx";
        window.top.searchInformer = "https://crmapp.deves.co.th/dvssearch/SearchInformer.aspx";
        window.top.createclaimnotiAPI = "https://crmapp.deves.co.th/GenerateCallerId/GenerateCaller.aspx";
        window.top.isurvey = "http://192.168.3.37:82/isurveyV2/";


        window.top.createclaimAPI = "https://crmapp.deves.co.th/XrmApi/api/ClaimRegistration";
        window.top.requestsurveyorAPI  = "https://crmapp.deves.co.th/XrmApi/api/RequestSurveyor";

        window.top.categoryENUM = {
            id:'EB5D6D39-9935-E711-80CB-0050568D18DF',
            name:'สินไหม (Motor)',
            logicalname:'pfc_category'
        };
        window.top.subcategoryENUM = {
            id:'63D833AA-A535-E711-80CB-0050568D18DF',
            name:'แจ้งอุบัติเหตุรถยนต์',
            logicalname:'pfc_sub_category'
        };
        window.top.claimCar = "63d833aa-a535-e711-80cb-0050568d18df";
        window.top.claimCarPLB = "65d833aa-a535-e711-80cb-0050568d18df";
        window.top.claimCarCB = "7bd833aa-a535-e711-80cb-0050568d18df";
        window.top.appConfig = {

            makeCallApiEnpoint: "https://crmapp.deves.co.th/internal-service/api/cti-web-dialer/makecall",
        }
    }

}

function getOdataObject(object,callback){
    var serverUrl = window.parent.Xrm.Page.context.getClientUrl();
    var ODATA_ENDPOINT = "/XRMServices/2011/OrganizationData.svc";
    var odataSelect = serverUrl + ODATA_ENDPOINT + "/" + object.odataSetName + "(guid'" + object.guid + "')";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        url: odataSelect,
        beforeSend: function (XMLHttpRequest) {
            XMLHttpRequest.setRequestHeader("Accept", "application/json");

        },
        success: function (data, textStatus, XmlHttpRequest) {
            var dataD = data.d;
            callback(dataD);
        },
        error: function (XmlHttpRequest, textStatus, errorObject) {
            callback("Error : " + textStatus + ": " + JSON.parse(XMLHttpRequest.responseText).error.message.value);
        }
    });
}

function getCurrentUser() {
 try{
    // ** Shows all the roles for current user

    var userRoles = [];
    var Roles = Xrm.Page.context.getUserRoles();
    for (var i = 0; i < Roles.length; i++) {
        var RoleId = Roles[i];
        var selectQuery = "/RoleSet?$top=1&$filter=RoleId eq guid'" + RoleId + "'&$select=Name";
        var role = null;
        role = makeRequest(selectQuery);
        var RoleName = role[0].Name;
        userRoles.push(RoleName);
    }
    return userRoles;
  }catch (e){

 }
    return null;
}

function makeRequest(query) {
    try{
    var serverUrl = Xrm.Page.context.getClientUrl();

    var oDataEndpointUrl = serverUrl + "/XRMServices/2011/OrganizationData.svc/";
    oDataEndpointUrl += query;

    var service = GetRequestObject();

    if (service != null) {
        service.open("GET", oDataEndpointUrl, false);
        service.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        service.setRequestHeader("Accept", "application/json, text/javascript, */*");
        service.send(null);

        var retrieved = JSON.parse(service.responseText).d;

        var results = new Array();
        for (var i = 0; i < retrieved.results.length; i++) {
            results.push(retrieved.results[i]);
        }

        return results;
    }
    }catch (e){

    }
    return null;
}

function GetRequestObject() {
    if (window.XMLHttpRequest) {
        return new window.XMLHttpRequest;
    } else {
        try {
            return new ActiveXObject("MSXML2.XMLHTTP.3.0");
        } catch (ex) {
            return null;
        }
    }
}
