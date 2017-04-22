using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WinController : MonoBehaviour
{

    public const string sceneName = "win";

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }

    public void ClickBackground()
    {
        SceneManager.LoadScene(GameController.sceneName);
    }
}
