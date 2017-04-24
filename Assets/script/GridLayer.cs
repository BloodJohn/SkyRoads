using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GridLayer : MonoBehaviour
{
    public List<GameObject> cellPrefab;
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
                    var rnd = Random.Range(1, 10);
                    if (rnd <= 4)
                        cellDataList.Add(1); //луга
                    else if (rnd <= 7)
                        cellDataList.Add(2); //лес
                    else if (rnd <= 8)
                        cellDataList.Add(3); //горы
                    else
                        cellDataList.Add(4); //вода
                }
            }

        //добавляем 4 поселков
        var i = 4;
        while (i > 0)
        {
            var x = Random.Range(0, sizeX - 1);
            var y = Random.Range(0, sizeY - 1);

            if (cellDataList[y * sizeX + x] > 0)
            {
                cellDataList[y * sizeX + x] = 5;
                i--;
            }
        }

        //добавляем 3 городов
        i = 3;
        while (i > 0)
        {
            var x = Random.Range(0, sizeX - 1);
            var y = Random.Range(0, sizeY - 1);

            if (cellDataList[y * sizeX + x] > 0)
            {
                cellDataList[y * sizeX + x] = 6;
                i--;
            }
        }

        //добавляем 2 столицы
        i = 2;
        while (i > 0)
        {
            var x = Random.Range(0, sizeX - 1);
            var y = Random.Range(0, sizeY - 1);

            if (cellDataList[y * sizeX + x] > 0)
            {
                cellDataList[y * sizeX + x] = 7;
                i--;
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
                var id = cellDataList[y * sizeX + x];
                if (id == 0) continue;

                var newCell = Instantiate(cellPrefab[id - 1], transform);
                var pos = new Vector3();
                pos = topShift;
                pos += shiftX * x + shiftY * y;
                pos.z = -(x + y) * 0.01f;
                newCell.transform.localPosition = pos;
                newCell.name = string.Format("cell_{0}_{1}", x, y);

                var cell = newCell.GetComponent<CellView>();
                cellViewList[y * sizeX + x] = cell;
                cell.id = id;
                cell.x = x;
                cell.y = y;

                cell.speed = Random.Range(-5f, 5f);

                if (x != y) cell.speed += (x-y);
            }
        }
    }

    public bool BuildBridge(CellView clickCell)
    {
        if (clickCell.hasBridge) return false;

        foreach (var nearCell in GetNear(clickCell))
            if (nearCell.hasBridge)
            {
                clickCell.hasBridge = true;
                var newBridge = Instantiate(bridgePrefab, transform);
                bridgeViewList.Add(newBridge.GetComponent<BridgeView>());

                var pos = (clickCell.transform.position + nearCell.transform.position) / 2;
                pos.z = Mathf.Min(clickCell.transform.position.z, nearCell.transform.position.z) - 0.01f;
                newBridge.transform.position = pos;

                var difference = clickCell.transform.position - nearCell.transform.position;
                if (difference.x < 0) difference *= -1f;
                var angle = Vector2.Angle(Vector2.down, difference);
                newBridge.transform.Rotate(0, 0, angle);

                CoreGame.Instance.BuildBridge(clickCell.id);
            }

        return clickCell.hasBridge;
    }

    private CellView this[int x, int y]
    {
        get
        {
            var index = y * sizeX + x;
            if (index < 0) return null;
            if (index >= cellViewList.Count) return null;
            return cellViewList[index];
        }
    }

    private IEnumerable<CellView> GetNear(CellView cell)
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

    /// <summary>считаем объединения в одну дорогу</summary>
    public bool OneRoadTest()
    {
        int roadNum = 1;

        ClearRoadTest();

        for (var y = 0; y < sizeY; y++)
            for (var x = 0; x < sizeX; x++)
            {
                var cell = this[x, y];
                if (null == cell) continue;
                if (!cell.IsTown) continue;
                if (cell.roadTest < int.MaxValue) continue;

                cell.roadTest = roadNum;
                MarkAllRoad(cell);
                roadNum++;
            }

        //Debug.LogFormat("RoadTest {0}", roadNum - 1);
        return roadNum == 2;
    }

    /// <summary>помечаем все клетки эти номером дороги</summary>
    private void MarkAllRoad(CellView townCell)
    {
        foreach (var nearCell in GetNear(townCell))
            if (nearCell.hasBridge && nearCell.roadTest > townCell.roadTest)
            {
                nearCell.roadTest = townCell.roadTest;
                MarkAllRoad(nearCell);
            }
    }

    public int AllTradeTest()
    {
        var result = -bridgeViewList.Count*3;

        for (var i = 0; i < cellViewList.Count; i++)
        {
            var cell = cellViewList[i];
            if (null == cell) continue;
            if (cell.IsTown)
                result += GetTradeProfit(cell);
        }
         
        return result;
    }

    /// <summary>все города с которыми торгует этот город</summary>
    private int GetTradeProfit(CellView townCell)
    {
        ClearRoadTest();
        var currentWave = 0;
        switch (townCell.id)
        {
            case 5:
                currentWave = 5;
                break;
            case 6:
                currentWave = 10;
                break;
            case 7:
                currentWave = 15;
                break;
        }
        var result = 0;
        townCell.roadTest = currentWave;
        
        while (currentWave > 0)
        {
            var nextRoad = false;
            for (var i = 0; i < cellViewList.Count; i++)
            {
                var cell = cellViewList[i];
                if (cell==null) continue;
                if (cell.roadTest!=currentWave) continue;

                foreach (var nearCell in GetNear(cell))
                    if (nearCell.roadTest == Int32.MaxValue)
                    {
                        nextRoad = true;
                        nearCell.roadTest = currentWave - 1;
                        if (nearCell.IsTown)
                            result += currentWave -1;
                    }
            }

            if (nextRoad)
                currentWave--;
            else
                currentWave = 0;
        }

        return result;
    }

    private void ClearRoadTest()
    {
        for (var i = 0; i < cellViewList.Count; i++)
        {
            var cell = cellViewList[i];
            if (null == cell) continue;
            cell.roadTest = cell.hasBridge ? int.MaxValue : cell.roadTest;
        }
    }

    public void Fly(float time)
    {
        for (var i = 0; i < cellViewList.Count; i++)
        {
            var cell = cellViewList[i];
            if (null != cell) cell.Fly(time);
        }
    }

    public void FlyDown(float time)
    {
        for (var i = 0; i < cellViewList.Count; i++)
        {
            var cell = cellViewList[i];
            if (null != cell) cell.FlyDown(time);
        }
    }

    public void HideBridge()
    {
        for (var i = 0; i < bridgeViewList.Count; i++)
        {
            var bridge = bridgeViewList[i];
            bridge.gameObject.SetActive(false);
        }
    }
}
