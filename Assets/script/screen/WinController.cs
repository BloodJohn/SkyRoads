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

    void Start()
    {
        SoundManager.Instance.PlayLevelComplete();
    }

    public void ClickBackground()
    {
        SceneManager.LoadScene(GameController.sceneName);
    }
}
