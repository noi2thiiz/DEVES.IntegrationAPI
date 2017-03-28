using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public interface ITransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCLSPersonalClientInput
    {
        void TransformModel(BaseDataModel input, ref BaseDataModel output);
    }
}