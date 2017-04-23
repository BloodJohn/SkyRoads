using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DefeatController : MonoBehaviour {

    public const string sceneName = "defeat";

    public Text scoreText;

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);

        scoreText.text = string.Format("Completed {0} levels", CoreGame.Instance.level);
    }

    public void ClickBackground()
    {
        CoreGame.Instance.StartGame();
        SceneManager.LoadScene(GameController.sceneName);
    }
}
