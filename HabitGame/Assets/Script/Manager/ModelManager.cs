using System;
using System.Collections.Generic;

public class ModelManager : ManagerBase<ModelManager>, IManager, IDisposable
{
    private List<IModel> _modelList = new();

    public void PreInitialize()
    {
        //
    }

    public void Initialize()
    {
    }

    public void SetModel(IEnumerable<IModel> models)
    {
        //
    }

    public void SetAllModels(List<IModel> modelList)
    {
        _modelList = modelList;
    }

    public TModel GetModel<TModel>() where TModel : IModel
    {
        foreach (var model in _modelList)
        {
            if (model is TModel genericModel)
            {
                return genericModel;
            }
        }

        throw new NullReferenceException("GetModel Fail");
    }

    public void Dispose()
    {
        //
    }
}