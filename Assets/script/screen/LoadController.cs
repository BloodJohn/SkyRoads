using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadController : MonoBehaviour
{
    public const string sceneName = "load";

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }
}