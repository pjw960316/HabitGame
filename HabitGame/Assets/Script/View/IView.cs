public interface IView
{
    // Note
    // View는 MonoBehaviour를 상속 받기 때문에, Awake()를 사용한다.
    // 당연히, 최상단 View는 Awake를 1회 호출해야 한다.
    // 그러므로 virtual OnAwake()를 구현해서 이용한다.
    public void OnAwake();

    // Note
    // 모든 View는 Presenter를 virtual 하게 구현해야 한다.
    public void CreatePresenterByManager();
}