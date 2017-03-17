using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzClaimRegistrationCommand: BaseCommand
    {
        const string sqlcmd_Get_RegClaimInfo = "sp_CustomApp_RegClaimInfo_Incident";

        public BuzClaimRegistrationCommand() : base()
        { }

        public override BaseDataModel Execute( object value)
        {
            //+ Deserialize Input
            ClaimRegistrationInputModel contentModel = DeserializeJson<ClaimRegistrationInputModel>(value.ToString());

            //+ Prepare input data model
            LocusClaimRegistrationInputModel data = new LocusClaimRegistrationInputModel();
            data.claimHeader = new LocusClaimheaderModel();
            data.claimAssignSurv = new LocusClaimassignsurvModel();
            data.claimInform = new LocusClaiminformModel();
            data.claimSurvInform = new LocusClaimsurvinformModel();
            BaseDataModel input = data;

            //+ Call SQL to get data
            List<CommandParameter> listParam = new List<CommandParameter>();
            listParam.Add(new CommandParameter("incidentId", contentModel.IncidentId ));
            listParam.Add(new CommandParameter("CurrentUserId", contentModel.CurrentUserId ));
            FillModelUsingSQL(ref input , sqlcmd_Get_RegClaimInfo, listParam);



            //output = HandleMessage(contentText, contentModel);
            return new ClaimRegistrationOutputModel();
        }
    }
}