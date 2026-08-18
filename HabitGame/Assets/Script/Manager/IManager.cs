public interface IManager
{
    // NOTE
    // 외부 데이터 없이 스스로 온전히 초기화 가능한 것 구현
    public void PreInitialize();

    // NOTE
    // 외부 데이터가 필요하거나, 시점이 빠르지 않아도 되는 것 구현
    public void Initialize();

    // NOTE
    // XML 또는 ScriptableObject를 통해 데이터를 읽어온다.
    public void SetData();
    public void BindEvent();
    public void ConnectInstanceByActivator(IManager instance);
}
