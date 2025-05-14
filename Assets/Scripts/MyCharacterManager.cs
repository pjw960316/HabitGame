using UnityEngine;

public class MyCharacterManager
{
    // field
    private static MyCharacterManager _myCharacterManager;

    // Property
    public static MyCharacterManager Instance
    {
        get
        {
            if (_myCharacterManager == null) _myCharacterManager = new MyCharacterManager();

            return _myCharacterManager;
        }
    }

    public int Budget { get; private set; }

    // Constructor
    public MyCharacterManager()
    {
        Debug.Log("MyCharacterManager Constructor");
        
        Budget = 0;
    }
}