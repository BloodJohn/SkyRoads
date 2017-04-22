using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class GridLayer : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject bridgePrefab;
    public float dx = 0.737f;
    public float dy = 0.426f;

    public int sizeX = 3;
    public int sizeY = 3;

    public int trimX = 1;
    public int trimY = 1;

    private List<int> cellDataList;
    private List<CellView> cellViewList;

    private readonly List<BridgeView> bridgeViewList = new List<BridgeView>();

    void Awake()
    {
        cellDataList = new List<int>(sizeX * sizeY);
        cellViewList = new List<CellView>(sizeX * sizeY);

        for (var y = 0; y < sizeY; y++)
            for (var x = 0; x < sizeX; x++)
            {
                cellViewList.Add(null);

                if (x + y < trimX)
                {
                    cellDataList.Add(0);
                }
                else if (sizeX - 1 - x + sizeY - 1 - y < trimX)
                {
                    cellDataList.Add(0);
                }
                else if (sizeX - 1 - x + y < trimY)
                {
                    cellDataList.Add(0);
                }
                else if (x + sizeY - 1 - y < trimY)
                {
                    cellDataList.Add(0);
                }
                else
                {
                    cellDataList.Add(1);
                }
            }
    }

    public void BuildGrid()
    {
        var shiftX = new Vector3(dx, -dy);
        var shiftY = new Vector3(-dx, -dy);
        var topShift = new Vector3(0f, dy * (sizeX + sizeY) / 2);

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                if (cellDataList[y * sizeX + x] == 0) continue;

                var newCell = Instantiate(cellPrefab, transform);
                var pos = new Vector3();
                pos = topShift;
                pos += shiftX * x + shiftY * y;
                pos.z = -(x + y) * 0.01f;
                newCell.transform.localPosition = pos;
                newCell.name = string.Format("cell_{0}_{1}", x, y);

                var cell = newCell.GetComponent<CellView>();
                cellViewList[y * sizeX + x] = cell;
                cell.id = cellDataList[y * sizeX + x];
                cell.x = x;
                cell.y = y;
            }
        }
    }

    public void BuildBridge(CellView clickCell)
    {
        clickCell.hasBridge = true;

        foreach (var nearCell in GetNear(clickCell))
            if (nearCell.hasBridge)
            {
                var newBridge = Instantiate(bridgePrefab, transform);

                var pos = (clickCell.transform.position + nearCell.transform.position) / 2;
                pos.z = Mathf.Min(clickCell.transform.position.z, nearCell.transform.position.z) - 0.01f;
                newBridge.transform.position = pos;

                var difference = clickCell.transform.position - nearCell.transform.position;
                if (difference.x < 0) difference *= -1f;
                var angle = Vector2.Angle(Vector2.down, difference);
                newBridge.transform.Rotate(0,0,angle);
            }
    }

    public CellView this[int x, int y]
    {
        get
        {
            if (x + y < 0) return null;
            if (x + y >= sizeX * sizeY) return null;
            return cellViewList[y * sizeX + x];
        }
    }

    public IEnumerable<CellView> GetNear(CellView cell)
    {
        if (this[cell.x - 1, cell.y - 1] != null)
            yield return this[cell.x - 1, cell.y - 1];
        if (this[cell.x, cell.y - 1] != null)
            yield return this[cell.x, cell.y - 1];
        if (this[cell.x + 1, cell.y] != null)
            yield return this[cell.x + 1, cell.y];
        if (this[cell.x + 1, cell.y + 1] != null)
            yield return this[cell.x + 1, cell.y + 1];
        if (this[cell.x, cell.y + 1] != null)
            yield return this[cell.x, cell.y + 1];
        if (this[cell.x - 1, cell.y] != null)
            yield return this[cell.x - 1, cell.y];
    }
}
