using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LoadController : MonoBehaviour
{
    public const string sceneName = "load";

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
        Application.targetFrameRate = 10;
    }

    public void ClickBackground()
    {
        CoreGame.Instance.LoadGame();
        SceneManager.LoadScene(GameController.sceneName);
    }
}