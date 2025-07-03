using System.Collections.Generic;
using UnityEngine;

public interface IManager : IFactory
{
    public void Init();
    public void SetModel(IEnumerable<ScriptableObject> _list);

    //REFACTOR
    //이 녀석의 위치가 이게 맞는가?
    //Reflection을 써서 Instance를 연결해야 하니까 넣었는데...
    public void ConnectInstanceByActivator(IManager instance);
}

/* Refactor
 SingletonBase의 기능이 포함되어 있지만, ManagerBase의 기능을 helper class로 빼는 게 파편화 같아서 둘이 합침
 합치는 게 SRP는 위반했다.
*/
public class ManagerBase<T> where T : IManager, new()
{
    protected static T _instance;

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
}