using System.Collections.Generic;

public interface IManager
{
    // note : 외부 데이터 없이 스스로 온전히 초기화 가능한 것 구현
    public void PreInitialize();
    
    // note : 외부 데이터가 필요하거나, 시점이 빠르지 않아도 되는 것 구현
    public void Initialize();
    public void SetModel(); 
    public void BindEvent();
    public void ConnectInstanceByActivator(IManager instance);
}