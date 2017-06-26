var hostname = window.location.hostname;
switch(hostname){

    case "csat-qa.deves.co.th":
        window.appConfig = {
            assessmentQuestionnaire : [
                "",
                "D181A7B8-FA4C-E711-80DA-0050568D615F", /*1= surveyGuid*/
                "BDA64EC4-FA4C-E711-80DA-0050568D615F" /*2= garageGuid*/
            ]
        };
        break;

    default:
        window.appConfig = {
            assessmentQuestionnaire : [
                "",
                "FB59FF14-3E57-E711-80CB-0050568D6908", /*1= surveyGuid*/
                "FD59FF14-3E57-E711-80CB-0050568D6908" /*2= garageGuid*/
            ]
        };
        break;
}



