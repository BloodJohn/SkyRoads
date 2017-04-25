using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public const string sceneName = "game";

    public GridLayer grid;
    public Text moneyText;
    public Text buildText;
    public Text tradeText;

    private float time = 0f;
    private float speed = 0f;
    private const float maxTime = 1f;
    private bool nextLevel = false;

    #region unity

    private void Start()
    {
        grid.BuildGrid();
        ShowStats();

        time = maxTime;
        speed = -1f;
    }

    private void Update()
    {
        if (FlyAnimation()) return;

        //баузер показывает поддержку мультитача (на самом деле нет)
        /*if (Input.touchSupported)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Moved) //|| touch.phase == TouchPhase.Began
                {
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                    CheckMouseClick(mouseWorldPos);
                }
            }
        }
        else*/
        {
            if (Input.GetMouseButton(0)) //GetMouseDown
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CheckMouseClick(mouseWorldPos);
            }
        }
    }
    #endregion

    #region stuff
    private void ShowStats()
    {
        if (CoreGame.Instance == null) return;
        moneyText.text = CoreGame.Instance.money.ToString();
        buildText.text = CoreGame.Instance.buildRate.ToString();
        tradeText.text = CoreGame.Instance.tradeRate.ToString();
    }

    private void CheckMouseClick(Vector2 mouseWorldPos)
    {
        var hitList = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero);
        foreach (var hit in hitList)
        {
            if (hit.transform == null) continue;
            var cell = hit.transform.parent.gameObject.GetComponent<CellView>();
            if (cell==null) continue;

            if (!grid.BuildBridge(cell)) continue;
            var trade = grid.AllTradeTest();
            CoreGame.Instance.SetTrade(trade);
            Debug.LogFormat("trade: {0} money: {1}", trade, CoreGame.Instance.money);

            ShowStats();

            if (CoreGame.Instance.money<=0)
            {
                Debug.LogFormat("GAME OVER");
                speed = 1f;
                grid.HideBridge();
                return;
            }

            if (grid.OneRoadTest())
            {
                Debug.LogFormat("WIN");
                CoreGame.Instance.WinLevel();
                speed = 1f;
                grid.HideBridge();
                nextLevel = true;
                return;
            }
        }
    }

    private bool FlyAnimation()
    {
        if (speed==0f) return false;

        time += Time.deltaTime * speed;
        if (time < 0)
        {
            time = 0;
            grid.Fly(0f);
            speed = 0f;

            return false; //начало игры
        }

        if (time > maxTime)
        {
            time = maxTime;
            grid.Fly(maxTime);
            speed = 0f;

            if (nextLevel)
                SceneManager.LoadScene(WinController.sceneName);
            else
                SceneManager.LoadScene(DefeatController.sceneName);

            return true; //конец игры
        }

        if (speed > 0 && !nextLevel)
        {
            grid.FlyDown(time);
        }
        else
        {
            grid.Fly(time);
        }
        return true;
    }
    #endregion
}
