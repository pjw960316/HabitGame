using UnityEngine;

[CreateAssetMenu(fileName = "MyCharacterData", menuName = "ScriptableObjects/MyCharacterData")]
public class MyCharacterData : ScriptableObject, IModel
{
    public int Budget { get; private set; }

    public MyCharacterData()
    {
        
    }
}