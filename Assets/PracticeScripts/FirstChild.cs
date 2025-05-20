using System;
using UnityEngine;

public class FirstChild : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("child awake");
    }

    private void Start()
    {
        Debug.Log("child start");
    }
}