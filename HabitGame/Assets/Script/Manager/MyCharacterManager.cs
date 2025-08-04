using System.Collections.Generic;
using UnityEngine;

// TODO
// Instance = 1개 = 박지원
public class MyCharacterManager : ManagerBase<MyCharacterManager>, IManager
{
    // TODO 
    // Job Instance 2개 만들고
    // previous Job = high coupling
    // future job = loose coupling 
    private MyCharacterData _myCharacterData;

    public void Initialize()
    {
    }

    public void SetModel(IEnumerable<IModel> model)
    {
        
    }
}