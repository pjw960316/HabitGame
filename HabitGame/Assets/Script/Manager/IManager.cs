using System.Collections.Generic;
using UnityEngine;

public interface IManager : IFactory
{
    //fix
    //현재 문제점이 이게 로드 단계에서 호출되기 때문에
    //MainScene에는 아직 없는 친구를 세팅한다면 문제가 생긴다.
    public void Initialize();
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