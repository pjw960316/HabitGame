using System.Collections.Generic;

public interface IManager
{
    public void PreInitialize();
    public void Initialize();
    public void SetModel(); 
    public void BindEvent();
    public void ConnectInstanceByActivator(IManager instance);
}