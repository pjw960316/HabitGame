using System.Collections.Generic;
using UnityEngine;

public interface IManager : IFactory
{
    public void PreInitialize();
    public void Initialize();
    public void SetModel(IEnumerable<IModel> models);
    public void ConnectInstanceByActivator(IManager instance);
}

// Note
// 공통로직을 담는 메서드가 굳이 IManager를 상속 받을 필요 없다.
public abstract class ManagerBase<T> where T : class, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }

    // Note
    // Activator를 통해 만든 Instance를 Singleton Instance에 초기화 시켜준다.
    public void ConnectInstanceByActivator(IManager instance)
    {
        _instance = instance as T;
    }
}