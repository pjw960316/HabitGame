public interface IView
{
    // Note
    // View는 MonoBehaviour를 상속 받기 때문에, Awake()를 사용한다.
    // 당연히, 최상단 View는 Awake를 1회 호출해야 한다.
    // 그러므로 virtual OnAwake()를 구현해서 이용한다.
    
    //refactor
    //이것도 내부니까 필요없음
    //public void OnAwake();
    
    //protected void Initialize();
    
    // todo
    // SetView는 진짜 책임 졌으면 좋겠는데 모든 params가 다르고, 괜히 복잡해지고,,
}