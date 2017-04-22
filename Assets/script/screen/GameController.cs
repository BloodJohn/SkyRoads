using UnityEngine;

public class GameController : MonoBehaviour
{
    public const string sceneName = "game";

    public GridLayer grid;

    #region unity

    private void Start()
    {
        grid.BuildGrid();
    }

    private void Update()
    {
        if (Input.touchSupported)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began) //|| touch.phase == TouchPhase.Moved
                {
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                    CheckMouseClick(mouseWorldPos);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
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
                Debug.LogFormat("click {0}", hit.transform.parent.gameObject.name);

                grid.BuildBridge(cell);
                /*var itemButton = hit.transform.gameObject.GetComponent<ItemButton>();
                if (itemButton != null) CheeseCakeClick(hit.point, itemButton);*/
            }
        }

        
    }
    #endregion
}
