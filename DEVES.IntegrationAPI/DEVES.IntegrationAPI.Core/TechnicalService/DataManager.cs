using System;
using DEVES.IntegrationAPI.Core.DataAdepter;
using DEVES.IntegrationAPI.Core.DataGateWay;
using DEVES.IntegrationAPI.Core.TechnicalService.DataManager;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.Core.TechnicalService
{
    public class CrmDataManager<TModelClass>
        where TModelClass : class

    {
    private readonly CrmDataAdepter _dataAdepter;

    public CrmDataManager(CrmDataAdepter dataAdepter)
    {
        _dataAdepter = dataAdepter;
    }

    public Guid Create(TModelClass entity)
    {
        return _dataAdepter.Create(entity);
    }

    public TModelClass Find(Guid id)
    {
        var entity = _dataAdepter.Find<TModelClass>(id);
            Console.WriteLine(entity.ToJSON());
        var tranform = new XrmTranformer<TModelClass>();
        return tranform.TranformEntityWithAttribute(entity);

    }
        public TModelClass Find(string id)
        {
            return Find(new Guid(id));

        }
    }
}