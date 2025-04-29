using UnityEngine;

[CreateAssetMenu(fileName = "ServerData", menuName = "ScriptableObjects/ServerDataScriptableObject", order = 1)]
public class ServerData : ScriptableObject
{
    public int coupon;
    public int budget;
}