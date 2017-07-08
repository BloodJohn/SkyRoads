using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadController : MonoBehaviour
{
    public const string sceneName = "load";
    public Sprite sountOn;
    public Sprite sountOff;
    public Image soundImage;

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
        Application.targetFrameRate = 10;
    }

    void Start()
    {
        soundImage.sprite = SoundManager.Instance.IsSound ? sountOn : sountOff;
    }

    public void ClickBackground()
    {
        CoreGame.Instance.LoadGame();
        SceneManager.LoadScene(GameController.sceneName);
    }

    public void ClickSound()
    {
        SoundManager.Instance.MuteSound();
        soundImage.sprite = SoundManager.Instance.IsSound ? sountOn : sountOff;
    }
}