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
    }

    public void ClickBackground()
    {
        CoreGame.Instance.StartGame();
        SceneManager.LoadScene(GameController.sceneName);
    }
}