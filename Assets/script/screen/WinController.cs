using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WinController : MonoBehaviour
{
    private const float scale = 200f / 1500f;

    public const string sceneName = "win";
    public Text scoreText;
    public Text levelText;
    public GridLayoutGroup grid;
    public GameObject prefab;
    public Color red = Color.red;
    public Color green = Color.green;

    void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
    }

    void Start()
    {
        SoundManager.Instance.PlayLevelComplete();
        scoreText.text = CoreGame.Instance.money.ToString();
        levelText.text = string.Format("{0} level", CoreGame.Instance.level);

        var history = CoreGame.Instance.moneyHistory;

        var i = Mathf.Max(0, history.Count - 10);
        var prev = 100f;

        while (i<history.Count)
        {
            var item = Instantiate(prefab, grid.transform);
            var bar = item.GetComponentInChildren<Image>();
            bar.rectTransform.offsetMax = new Vector2(0,history[i]*scale);

            if (history[i] > prev) bar.color = green;
            if (history[i] < prev) bar.color = red;


            prev = history[i];
            i++;
        }
    }

    public void ClickBackground()
    {
        SceneManager.LoadScene(GameController.sceneName);
    }
}
