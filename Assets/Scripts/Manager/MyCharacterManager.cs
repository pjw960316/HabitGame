using UnityEngine;

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

public class MyCharacterManager : SingletonBase<MyCharacterManager>, IManager
{
    public MyCharacterManager()
    {
        Budget = 0;
    }
    public int Budget { get; private set; }

}

