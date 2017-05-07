function onclick_addcalltype() {
    getVariable();
    var aspxurl = "http://" + document.domain + "/dvsSubjectSelection/SubjectSelection.aspx?etc=" + getEtc + "&" + "directioncode=" + getDirection;

    var ret = window.showModalDialog(aspxurl, "test", "dialogWidth:800px;dialogHeight:650px");
    //var ret = window.open(aspxurl, "_blank", "width=800,height=650");
    if (ret != null || ret != undefined) {
        //SetValueStandard(Xrm.Page.getAttribute("description"), ret.description.replace(/_-ln-_/g, "\n").replace(/_-lr-_/g, "\r")); //Display : Note
        SetValueOptionSet(Xrm.Page.getAttribute("pfc_calltype".toLowerCase()), ret.new_calltypeid); //Display : Call Type
        //SetValueStandard(Xrm.Page.getAttribute("new_max_work_day"), ret.new_max_work_day); //Display : max_work_day
        //SetValueStandard(Xrm.Page.getAttribute("subject"), ret.subject);
        if (ret.directioncode != null) {
            if (ret.directioncode == "Outgoing") {
                SetValueTwooption(Xrm.Page.getAttribute("directioncode"), "1");
            } else {
                SetValueTwooption(Xrm.Page.getAttribute("directioncode"), "2");

            }
        }
        if (ret.new_categoryid != null)
            SetValueLookup(Xrm.Page.getAttribute("pfc_categoryId".toLowerCase()), ret.new_categoryid[0], ret.new_categoryid[1], ret.new_categoryid[2]);

        if (ret.new_sub_categoryid != null)
            SetValueLookup(Xrm.Page.getAttribute("pfc_sub_categoryId".toLowerCase()), ret.new_sub_categoryid[0], ret.new_sub_categoryid[1], ret.new_sub_categoryid[2]);

        SetSubject(ret.new_categoryid[1] + '-' + ret.new_sub_categoryid[1]);

    }
    //-- if (ret != null || ret != undefined) --
}
//-- Call_CustomSubjectInfo() --
function SetSubject(txt) {
    try {
        var txtSubject = txt + ' คุณ ';
        var DirectionCodeEnum = { 'Incoming': false, 'Outgoing': true };
        if (Xrm.Page.getAttribute("directioncode").getValue() == DirectionCodeEnum.Incoming) {
            if (Xrm.Page.getAttribute("from").getValue() != null) {
                txtSubject += Xrm.Page.getAttribute("from").getValue()[0].name;
                //if (Xrm.Page.getAttribute("from").getValue()[0].entityType == 'contact') {
                //txtSubject += ChangeDisplayContact(Xrm.Page.getAttribute("from"));
                //} //--  if (Xrm.Page.getAttribute("from").getValue()[0].entityType == 'contact') --
            } //-- if (Xrm.Page.getAttribute("from").getValue() != null) --
        } //-- if (Xrm.Page.getAttribute("directioncode").getValue() == DirectionCodeEnum.Incoming) --
        else if (Xrm.Page.getAttribute("directioncode").getValue() == DirectionCodeEnum.Outgoing) {
            if (Xrm.Page.getAttribute("to").getValue() != null) {
                txtSubject += Xrm.Page.getAttribute("to").getValue()[0].name;
                //if (Xrm.Page.getAttribute("to").getValue()[0].entityType == 'contact') {

                //txtSubject += ChangeDisplayContact(Xrm.Page.getAttribute("to"));
                //} //--  if (Xrm.Page.getAttribute("to").getValue()[0].entityType == 'contact') --
            } //--  if (Xrm.Page.getAttribute("to").getValue() != null) --
        } //-- else if (Xrm.Page.getAttribute("directioncode").getValue() == DirectionCodeEnum.Outgoing) --
        SetValueStandard(Xrm.Page.getAttribute("subject"), txtSubject);
    } catch (e) {
        alert(e);
    } //-- Try Catch --
} //-- function SetSubject(txt) --
function getVariable() {
    window.getEtc = Xrm.Page.context.getQueryStringParameters().etc;


    window.getDirection = Xrm.Page.getAttribute("directioncode").getValue();
}

