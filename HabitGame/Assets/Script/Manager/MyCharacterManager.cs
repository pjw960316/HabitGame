using System.Collections.Generic;
using UnityEngine;

public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    private MyCharacterData _myCharacterData;

    public void Init()
    {
    }

    public void SetModel(IEnumerable<ScriptableObject> models)
    {
    }

    public void ConnectInstanceByActivator(IManager instance)
    {
    }
}