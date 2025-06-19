// MONO가 아닌 Manager에 대해서만 inherit 할 것
public interface IManager
{
    public void Init();

    // 일단 default
    public void InitializeScriptableObject(IData data)
    {
    }
}

// IManager Script에 넣음으로 cohesion을 높인다.
public class SingletonBase<T> where T : IManager, new() 
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null) _instance = new T();

            return _instance;
        }
    }
}