var CategoryName;
var SubcategoryName;
var CustomerName;
var CaseTypeName;
var Cuschk = false;
var Catechk = false;
var Subcatechk = false;
var optionchk = true;
var CurrentDate;
function onLoadevt() {
    try {
        var fieldDis = "casetypecode,pfc_categoryId,pfc_sub_categoryId,pfc_source_data";
        fieldDis += ",pfc_claim_noti_number,pfc_claim_noti_numberOn";
        fieldDis += ",pfc_customer_client_number,pfc_customer_vip,pfc_customer_sensitive,pfc_customer_privilege";
        fieldDis += ",pfc_accident_latitude,pfc_accident_longitude";
        fieldDis += ",pfc_informer_client_number,pfc_driver_client_number";
        fieldDis += ",pfc_isurvey_status,pfc_isurvey_status_on,pfc_send_request_survey_by,pfc_isurvey_params_event_code";
        fieldDis += ",pfc_survey_meeting_latitude,pfc_survey_meeting_longitude,pfc_survey_meeting_district,pfc_survey_meeting_province"
        fieldDis += ",pfc_accident_prilim_surveyor_report_by";
        fieldDis += ",pfc_policy_additional_number,pfc_policy_number,pfc_policy_client_number,pfc_policy_vip,pfc_policy_mc_nmc";
        fieldDis += ",pfc_claim_number,pfc_policyid";
        var fieldDisLower = fieldDis.toLowerCase();
        SetDisabledField(fieldDisLower, true);
        ShowHidenTab("AssociatedKnowledgeBaseRecords", false);
        ShowHidenTab("Enhanced_SLA_Details_Tab", false);
        ShowHidenTab("ADDITIONALDETAILS_TAB", false);
        ShowHidenTab("SOCIALDETAILS_TAB", false);
        ShowHidenTab("KBARTICLE_TAB", false);
        ShowHidenTab("CASERELATIONSHIP_TAB", false);
        ShowHidenTab("tab_10", false);
        ShowHidenTab("tab_18", false);
        ShowHidenTab("tab_14", false);
        ShowHidenTab("tab_19", false);
        ShowHidenTab("tab_17", false);
        ShowHidenSection("tab_10", "tab_18_section_1", false);
        var CategoryObject = Xrm.Page.getAttribute("pfc_categoryid");
        var SubcategoryObject = Xrm.Page.getAttribute("pfc_sub_categoryid");
        var CustomerObject = Xrm.Page.getAttribute("customerid");
        var caseTypeObject = Xrm.Page.getAttribute("casetypecode");
        var sendoutSurveyorObject = Xrm.Page.getAttribute("pfc_send_out_surveyor");
        var claimtypeObject = Xrm.Page.getAttribute("pfc_claim_type");
        if (CategoryObject != null) {
            var CategoryObjectValue = CategoryObject.getValue();
            if (CategoryObjectValue != null) {
                CategoryName = CategoryObjectValue[0].name;
                Catechk = true;
            } else {
                CategoryName = "";
            }
        }
        if (SubcategoryObject != null) {
            var SubcategoryObjectValue = SubcategoryObject.getValue();
            if (SubcategoryObjectValue != null) {
                SubcategoryName = SubcategoryObjectValue[0].name;
                var SubcategoryId = SubcategoryObjectValue[0].id;
                Subcatechk = true;
                if (SubcategoryId.toLowerCase() == "{2f966072-0aab-e611-80ca-0050568d1874}"
                    || SubcategoryId.toLowerCase() == "{951ae07a-0aab-e611-80ca-0050568d1874}"
                    || SubcategoryId.toLowerCase() == "{92d632d8-29ab-e611-80ca-0050568d1874}") {
                    ShowHidenTab("tab_10", true);
                    ShowHidenTab("tab_18", true);
                    ShowHidenTab("tab_14", true);
                    ShowHidenTab("tab_19", true);
                } else {
                    SubcategoryName = "";
                }
            }
        }
        if (CustomerObject != null) {
            var CustomerObjectValue = CustomerObject.getValue();
            if (CustomerObjectValue != null) {
                CustomerName = " คุณ " + CustomerObjectValue[0].name;
                Cuschk = true;
            } else {
                CustomerName = "";
            }
        }
        if (caseTypeObject != null) {
            var caseTypeObjectValue = caseTypeObject.getText();
            if (caseTypeObjectValue != null) {
                CaseTypeName = caseTypeObjectValue;
            } else {
                CaseTypeName = "";
            }
            // stampCaseTitle();
        }
        if (claimtypeObject != null) {
            var claimtypeObjectValue = claimtypeObject.getValue();
            if (claimtypeObjectValue == "200000000") {
                Xrm.Page.getControl("pfc_send_out_surveyor").removeOption('100000005');
                optionchk = false;
            }
        }
    } catch (e) {
        alert(e);
    }
}
function onChangeSendOutSurveyor() {
    var sendoutObject = Xrm.Page.getAttribute("pfc_send_out_surveyor");
    if (sendoutObject != null) {
        var sendoutObjectValue = sendoutObject.getValue();
        if (sendoutObjectValue == "100000002") {
            ShowHidenSection("tab_10", "tab_18_section_1", true);
            CurrentDate = new Date(GetserverTime());
            Xrm.Page.getAttribute("pfc_survey_meeting_date").setValue(CurrentDate);
        } else {
            ShowHidenSection("tab_10", "tab_18_section_1", false);
            Xrm.Page.getAttribute("pfc_survey_meeting_date").setValue(null);
        }
    }
}
function onChangeClaimType() {
    var claimtypeObject = Xrm.Page.getAttribute("pfc_claim_type");
    if (claimtypeObject != null) {
        var claimtypeObjectValue = claimtypeObject.getValue();
        if (claimtypeObjectValue == "200000000") {
            Xrm.Page.getControl("pfc_send_out_surveyor").removeOption('100000005');
            optionchk = false;
        } else {
            if (optionchk == false) {
                var prb = { value: 100000005, text: "พรบ." }
                Xrm.Page.getControl("pfc_send_out_surveyor").addOption(prb);
                optionchk = true;
            }
        }
    }
}
function onChangeCusName() {
    debugger;
    var CustomerObject = Xrm.Page.getAttribute("customerid");
    if (CustomerObject != null) {
        var CustomerObjectValue = CustomerObject.getValue();
        if (CustomerObjectValue != null) {
            CustomerName = " คุณ " + CustomerObjectValue[0].name;
            Cuschk = true;
        } else {
            CustomerName = "";
        }
    }
    stampCaseTitle();
}
function onChangeSubcat() {
    debugger;
    var SubcategoryObject = Xrm.Page.getAttribute("pfc_sub_categoryid");
    if (SubcategoryObject != null) {
        var SubcategoryObjectValue = SubcategoryObject.getValue();
        if (SubcategoryObjectValue != null) {
            SubcategoryName = SubcategoryObjectValue[0].name;
            Subcatechk = true;
            var SubcategoryId = SubcategoryObjectValue[0].id;
            if (SubcategoryId.toLowerCase() == "2f966072-0aab-e611-80ca-0050568d1874"
                || SubcategoryId.toLowerCase() == "951ae07a-0aab-e611-80ca-0050568d1874"
                || SubcategoryId.toLowerCase() == "92d632d8-29ab-e611-80ca-0050568d1874") {
                ShowHidenTab("tab_10", true);
                ShowHidenTab("tab_18", true);
                ShowHidenTab("tab_14", true);
                ShowHidenTab("tab_19", true);
                CurrentDate = new Date(GetserverTime());
                Xrm.Page.getAttribute("pfc_relation_cutomer_accident_party").setValue("100000000");
                Xrm.Page.getAttribute("pfc_notification_date").setValue(CurrentDate);
                Xrm.Page.getAttribute("pfc_accident_on").setValue(CurrentDate);
                Xrm.Page.getAttribute("pfc_num_of_expect_injuries").setValue("100000000");
                var customeridvalue = Xrm.Page.getAttribute("customerid").getValue();
                if (customeridvalue != null) {
                    SetLookupValue("pfc_informer_name", customeridvalue[0].id, customeridvalue[0].name, customeridvalue[0].typename);
                } else {
                    Xrm.Page.getAttribute("pfc_informer_name").setValue(null);
                }
                //debugger;
                try {
                    var policyChk = Xrm.Page.getAttribute("pfc_policyid").getValue();
                    if (policyChk != null) {
                        var policyChkid = policyChk[0].id;
                        var policyChkType = policyChk[0].entityType;
                        var serverUrl = Xrm.Page.context.getClientUrl();
                        var ODATA_ENDPOINT = "/XRMServices/2011/OrganizationData.svc";
                        var odataSetName = "pfc_policySet";
                        var odataSelect = serverUrl + ODATA_ENDPOINT + "/" + odataSetName + "(guid'" + policyChkid + "')";
                        $.ajax({
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            datatype: "json",
                            url: odataSelect,
                            beforeSend: function (XMLHttpRequest) {
                                XMLHttpRequest.setRequestHeader("Accept", "application/json");
                            },
                            success: function (data, textStatus, XmlHttpRequest) {
                                //if (policyChkType == "contact") {
                                //    var policyCusId = data.d.pfc_customerId;
                                //    SetLookupValue("pfc_driver_name", policyCusId[0].id, policyCusId[0].Name, policyCusId[0].LogicalName);
                                //} else {
                                //    var driverFirst = data.d.pfc_driver_firstId;
                                //    SetLookupValue("pfc_driver_name", driverFirst[0].id, driverFirst[0].Name, driverFirst[0].LogicalName);
                                //}
                                //debugger;
                                var policyCusId = data.d.pfc_customerId;
                                var driverFirst = data.d.pfc_driver_firstId;
                                // if (policyCusId != null) {
                                //     if (policyCusId[0].LogicalName == 'contact') {
                                //         SetLookupValue("pfc_driver_name", policyCusId[0].id, policyCusId[0].name, policyCusId[0].typename);
                                //     }
                                //     else if (driverFirst != null) {
                                //         if (driverFirst[0].LogicalName == 'contact') {
                                //             SetLookupValue("pfc_driver_name", driverFirst[0].id, driverFirst[0].name, driverFirst[0].typename);
                                //         }
                                //         else {
                                //             if (customeridvalue != null)
                                //                 SetLookupValue("pfc_driver_name", customeridvalue[0].id, customeridvalue[0].name, customeridvalue[0].typename)
                                //             else
                                //                 Xrm.Page.getAttribute("pfc_driver_name").setValue(null);
                                //         }
                                //     }//-- else if (driverFirst != null) --
                                //     else {
                                //         if (customeridvalue != null)
                                //             SetLookupValue("pfc_driver_name", customeridvalue[0].id, customeridvalue[0].name, customeridvalue[0].typename)
                                //         else
                                //             Xrm.Page.getAttribute("pfc_driver_name").setValue(null);
                                //     }
                                // }//--  if (policyCusId != null) --
                                // else if (driverFirst != null) {
                                //     if (driverFirst[0].LogicalName == 'contact') {
                                //         SetLookupValue("pfc_driver_name", driverFirst[0].id, driverFirst[0].name, driverFirst[0].typename);
                                //     }
                                //     else {
                                //         if (customeridvalue != null)
                                //             SetLookupValue("pfc_driver_name", customeridvalue[0].id, customeridvalue[0].name, customeridvalue[0].typename)
                                //         else
                                //             Xrm.Page.getAttribute("pfc_driver_name").setValue(null);
                                //     }
                                // }//-- else if (driverFirst != null) ---
                                // else {
                                if (customeridvalue != null)
                                    SetLookupValue("pfc_driver_name", customeridvalue[0].id, customeridvalue[0].name, customeridvalue[0].typename)
                                else
                                    Xrm.Page.getAttribute("pfc_driver_name").setValue(null);
                                // }

                            }, error: function (XmlHttpRequest, textStatus, errorObject) { }
                        });
                    } else {
                        if (customeridvalue != null) {
                            SetLookupValue("pfc_driver_name", customeridvalue[0].id, customeridvalue[0].name, customeridvalue[0].typename)
                        } else {
                            Xrm.Page.getAttribute("pfc_driver_name").setValue(null);
                        }
                    }
                } catch (e) { debugger; }
            } else {
                ShowHidenTab("tab_10", false);
                ShowHidenTab("tab_18", false);
                ShowHidenTab("tab_14", false);
                ShowHidenTab("tab_19", false);
            }
            Catechk = true;
        }
    }
    stampCaseTitle();
}
function onChangeCusVip() {
    var casevipObject = Xrm.Page.getAttribute("pfc_case_vip");
    var casevipObjectValue;
    var cusPrivilege = Xrm.Page.getAttribute("pfc_customer_privilege");
    var cusPrivilegeValue;
    var cusSensitive = Xrm.Page.getAttribute("pfc_customer_sensitive");
    var cusSensitiveValue;
    if (casevipObject != null) {
        casevipObjectValue = casevipObject.getValue();
    }
    if (cusSensitive != null) {
        cusSensitiveValue = cusSensitive.getValue();
    }
    if (cusPrivilege != null) {
        cusPrivilegeValue = cusPrivilege.getValue();
    }
    if (casevipObjectValue == "1") {
        Xrm.Page.getAttribute("prioritycode").setValue(1);
    } else if (cusPrivilegeValue == "100000004") {
        Xrm.Page.getAttribute("prioritycode").setValue(1);
    } else if (cusSensitiveValue == "100000002") {
        Xrm.Page.getAttribute("prioritycode").setValue(1);
    } else if (cusPrivilegeValue == "100000002" || cusPrivilegeValue == "100000003" || cusSensitive == "100000001") {
        Xrm.Page.getAttribute("prioritycode").setValue(2);
    } else {
        Xrm.Page.getAttribute("prioritycode").setValue(3);
    }
}
function onChangePoliCusVip() {
    var policyVip = Xrm.Page.getAttribute("pfc_policy_vip");
    var customerVip = Xrm.Page.getAttribute("pfc_customer_vip");
    if (policyVip != null) {
        var policyVipValue = policyVip.getValue();

    }
    if (customerVip != null) {
        var customerVipValue = customerVip.getValue();
    }
    if (policyVipValue == true || customerVipValue == true) {

        Xrm.Page.getAttribute("pfc_case_vip").setValue(true);
    } else {
        Xrm.Page.getAttribute("pfc_case_vip").setValue(false);
    }
}

function onChangeCate() {
    var CategoryObject = Xrm.Page.getAttribute("pfc_categoryid");
    if (CategoryObject != null) {
        var CategoryObjectValue = CategoryObject.getValue();
        if (CategoryObjectValue != null) {
            CategoryName = CategoryObjectValue[0].name;
            Categoryid = CategoryObjectValue[0].id;

        }
    }
    ///stampCaseTitle();
}
function onChangeCasetype() {
    var caseTypeObject = Xrm.Page.getAttribute("casetypecode");
    if (caseTypeObject != null) {
        var caseTypeObjectValue = caseTypeObject.getText();
        if (caseTypeObjectValue != null) {
            CaseTypeName = caseTypeObjectValue;
        }
    }
    //stampCaseTitle();
}

function stampCaseTitle() {
    var concat;
    //debugger;
    var caseTitlefield = Xrm.Page.getAttribute("title");
    // if(Catechk == true && Subcatechk == true && Cuschk == true){
    // concat = CategoryName + " " + SubcategoryName + " คุณ " + CustomerName;
    // caseTitlefield.setValue(concat);
    // }else if(Catechk == true && Subcatechk == false && Cuschk == false){
    // concat = CategoryName;
    // caseTitlefield.setValue(concat);
    // }else if(Catechk == false && Subcatechk == true && Cuschk == false){
    // concat = SubcategoryName;
    // caseTitlefield.setValue(concat);
    // }else if(Catechk == false && Subcatechk == false && Cuschk == true){
    // concat = "คุณ " + CustomerName;
    // caseTitlefield.setValue(concat);
    // }else if(Catechk == true && Subcatechk == true && Cuschk == false){
    // concat = CategoryName + " " + SubcategoryName;
    // caseTitlefield.setValue(concat);
    // }else if(Catechk == true && Subcatechk == false && Cuschk == true){
    // concat = CategoryName + " คุณ " + CustomerName;
    // caseTitlefield.setValue(concat);
    // }else if(Catechk == false && Subcatechk == true && Cuschk == true){
    // concat = SubcategoryName + " คุณ " + CustomerName;
    // caseTitlefield.setValue(concat);
    // }
    //debugger;
    CategoryName = Xrm.Page.getAttribute("pfc_categoryid").getValue() != null ? Xrm.Page.getAttribute("pfc_categoryid").getValue()[0].name : "";
    SubcategoryName = Xrm.Page.getAttribute("pfc_sub_categoryid").getValue() != null ? Xrm.Page.getAttribute("pfc_sub_categoryid").getValue()[0].name : "";
    CaseTypeName = Xrm.Page.getAttribute("casetypecode").getValue() != null ? Xrm.Page.getAttribute("casetypecode").getText() : "";
    concat = CategoryName + " " + SubcategoryName + CustomerName;
    caseTitlefield.setValue(concat);
}
function SetLookupValue(fieldName, id, name, entityType) {
    if (fieldName != null) {
        var lookupValue = new Array();
        lookupValue[0] = new Object();
        lookupValue[0].id = id;
        lookupValue[0].name = name;
        lookupValue[0].entityType = entityType;
        Xrm.Page.getAttribute(fieldName).setValue(lookupValue);
    }
}