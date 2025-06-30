using System.Collections.Generic;
using UnityEngine;

public interface IManager : IFactory
{
    public void Init();
    public void SetModel(IEnumerable<ScriptableObject> _list);
}

/* REFACTOR : SingletonBase의 기능이 포함되어 있지만, ManagerBase의 기능을 helper class로 빼는 게 파편화 같아서 둘이 합침
 합치는 게 SRP는 위반 되니, 좋은 방법 리팩터링 할 것.
*/
public class ManagerBase<T> where T : IManager, new()
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