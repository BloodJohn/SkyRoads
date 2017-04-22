using UnityEngine;

public class CellView : MonoBehaviour
{
    public int id;
    public int x;
    public int y;
    public GridLayer grid;
    public bool hasBridge;

    private void Awake()
    {
        grid = transform.parent.GetComponent<GridLayer>();
    }
}
