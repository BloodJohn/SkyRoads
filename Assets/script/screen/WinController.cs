using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WinController : MonoBehaviour
{

    public const string sceneName = "win";
    public Text scoreText;
    public Text levelText;

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }

    void Start()
    {
        SoundManager.Instance.PlayLevelComplete();
        scoreText.text = CoreGame.Instance.money.ToString();
        levelText.text = string.Format("{0} level", CoreGame.Instance.level);
    }

    public void ClickBackground()
    {
        SceneManager.LoadScene(GameController.sceneName);
    }
}
