using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegComplaint;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzRegComplaint : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            /**
             * 1. รับ Input มาจาก P'Tose
             * 2. เอา Input พี่โต๊ส มา Query stored
             * 3. ปั้น data จาก stored มาเป็น json
             * 4. ยิงไปที่ http://192.168.3.92/ServiceProxy/Complaint_SVC/jsonservice/RegisComplaint
             * 5. ได้รับ Output แล้วส่งกลับไปให้ P'Tose
             **/

            // Deserialize Input
            RegComplaintInputModel contentModel = (RegComplaintInputModel)input;

            // Preparation Variable
            Request_RegComplaintModel reqModel = new Request_RegComplaintModel();
            BaseDataModel inputData = reqModel;

            // SQL for getting IncidentId
            // Connect SDK 
            ServiceContext svcContext;
            var _serviceProxy = GetOrganizationServiceProxy(out svcContext);

 

            var query = from c in svcContext.IncidentSet
                        where c.TicketNumber == contentModel.Ticketnumber
                        select c;


            // Query stored
            List<CommandParameter> listParam = new List<CommandParameter>();
            listParam.Add(new CommandParameter("IncidentId", contentModel.IncidentId));
            listParam.Add(new CommandParameter("currentUserId", contentModel.CurrentUserId));
            FillModelUsingSQL(ref inputData, CommonConstant.sqlcmd_Get_RegComplaintInfo, listParam);

            // Call Service through EWI
            Model.EWI.EWIResponseContent ret = (Model.EWI.EWIResponseContent)CallDevesJsonProxy<Model.EWI.EWIResponse>(CommonConstant.EWI_ENDPOINT_RequestRegComplaint, inputData, new Guid().ToString());
            // Get Response and check it!
            if (ret.case_no == null && ret.comp_id == null)
            {
                ReqComplaintOutputModel contentOutput = new ReqComplaintOutputModel();
                contentOutput.comp_id = null;
                contentOutput.case_no = null;
                contentOutput.errorMessage = ret.message;

                return contentOutput;
            }
            else
            {
                // ReqComplaintOutputModel output = new ReqComplaintOutputModel(ret.data);
                ReqComplaintOutputModel contentOutput = new ReqComplaintOutputModel();
                contentOutput.comp_id = ret.comp_id;
                contentOutput.case_no = ret.case_no;
                contentOutput.errorMessage = ret.message;

                return contentOutput;
            }

        }
    }
}