using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WinController : MonoBehaviour
{

    public const string sceneName = "win";
    public Text scoreText;

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }

    void Start()
    {
        SoundManager.Instance.PlayLevelComplete();
        scoreText.text = CoreGame.Instance.money.ToString();
    }

    public void ClickBackground()
    {
        SceneManager.LoadScene(GameController.sceneName);
    }
}
