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

    #region unity

    private void Start()
    {
        grid.BuildGrid();
        ShowStats();
    }

    private void Update()
    {
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
            if (hit.transform != null)
            {
                var cell = hit.transform.parent.gameObject.GetComponent<CellView>();
                if (cell==null) continue;
                //Debug.LogFormat("click {0}", hit.transform.parent.gameObject.name);

                if (grid.BuildBridge(cell))
                {
                    var trade = grid.AllTradeTest();
                    CoreGame.Instance.SetTrade(trade);
                    Debug.LogFormat("trade: {0} money: {1}", trade, CoreGame.Instance.money);


                    if (CoreGame.Instance.money>0)
                        ShowStats();
                    else
                    {
                        Debug.LogFormat("GAME OVER");
                        SceneManager.LoadScene(DefeatController.sceneName);
                    }

                    if (grid.OneRoadTest())
                    {
                        Debug.LogFormat("WIN");
                        CoreGame.Instance.WinLevel();
                        SceneManager.LoadScene(WinController.sceneName);
                    }
                }
            }
        }

        
    }
    #endregion
}
