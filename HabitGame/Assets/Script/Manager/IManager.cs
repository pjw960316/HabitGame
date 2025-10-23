using System.Collections.Generic;

public interface IManager
{
    public void PreInitialize();
    public void Initialize();
    public void LateInitialize();
    public void SetModel(IEnumerable<IModel> models);
    public void ConnectInstanceByActivator(IManager instance);
}