using UnityEngine;

public class StringManager : SingletonBase<MyCharacterManager>, IManager
{
    public void Init()
    {
        Debug.Log("StringManager");
    }
}