using System;
using System.Collections.Generic;

// refactor
// Alarm Presenter에서 Model을 로드 할 때 필요해서 일단 만들었다.
// 일단 모든 모델을 들고 있는 게 책임이다.
public class DataManager : ManagerBase<DataManager>, IManager, IDisposable
{
    private List<IModel> _modelList = new List<IModel>();
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
    
    //test
    public IModel GetAlarmModel()
    {
        foreach (var model in _modelList)
        {
            if (model is AlarmData alarmData)
            {
                return alarmData;
            }
        }

        return null;
    }

    public void Dispose()
    {
        //
    }
}