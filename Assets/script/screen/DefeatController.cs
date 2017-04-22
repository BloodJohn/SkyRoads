using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DefeatController : MonoBehaviour {

    public const string sceneName = "defeat";

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }

    public void ClickBackground()
    {
        SceneManager.LoadScene(GameController.sceneName);
    }
}
