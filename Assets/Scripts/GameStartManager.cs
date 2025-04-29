using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    private void Awake()
    {
        //모든 싱글턴의 Awake를 돌릴 것
        MyCharacterManager.Instance.OnAwake();
    }
}