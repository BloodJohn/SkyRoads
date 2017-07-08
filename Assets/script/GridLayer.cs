using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridLayer : MonoBehaviour
{
    private const int MapSizeX = 11;
    private const int MapSizeY = 11;

    private const int MapTrimX = 2;
    private const int MapTrimY = 7;
    public readonly List<int> CellDataList = new List<int>(MapSizeX * MapSizeY);

    public List<GameObject> cellPrefab;
    public GameObject bridgePrefab;
    public float dx = 0.737f;
    public float dy = 0.426f;

    private readonly List<CellView> cellViewList = new List<CellView>(MapSizeX * MapSizeY);

    private readonly List<BridgeView> bridgeViewList = new List<BridgeView>();

    void Awake()
    {
        CreateLevelMap();
        for (var i = 0; i < CellDataList.Count; i++)
            cellViewList.Add(null);
    }

    private void CreateLevelMap()
    {
        var level = CoreGame.Instance.level;
        CellDataList.Clear();

        for (var y = 0; y < MapSizeY; y++)
            for (var x = 0; x < MapSizeX; x++)
            {
                if (x + y < MapTrimX)
                {
                    CellDataList.Add(0);
                }
                else if (MapSizeX - 1 - x + MapSizeY - 1 - y < MapTrimX)
                {
                    CellDataList.Add(0);
                }
                else if (MapSizeX - 1 - x + y < MapTrimY)
                {
                    CellDataList.Add(0);
                }
                else if (x + MapSizeY - 1 - y < MapTrimY)
                {
                    CellDataList.Add(0);
                }
                else
                {
                    var rnd = Random.Range(1, 10);
                    if (rnd == 1 || rnd <= 5 - level / 2)
                        CellDataList.Add(1); //луга
                    else if (rnd == 2 || rnd <= 8 - level / 4)
                        CellDataList.Add(2); //лес
                    else if (rnd == 3 || rnd <= 9 - level / 8)
                        CellDataList.Add(3); //горы
                    else
                        CellDataList.Add(4); //вода
                }
            }

        //добавляем 4 поселков
        var i = 4;
        while (i > 0)
        {
            var index = Random.Range(0, CellDataList.Count);
            if (CellDataList[index] > 0)
            {
                CellDataList[index] = 5;
                i--;
            }
        }

        //добавляем 3 городов
        i = 3;
        while (i > 0)
        {
            var index = Random.Range(0, CellDataList.Count);
            if (CellDataList[index] > 0)
            {
                CellDataList[index] = 6;
                i--;
            }
        }

        //добавляем 2 столицы
        i = 2;
        while (i > 0)
        {
            var index = Random.Range(0, CellDataList.Count);
            if (CellDataList[index] > 0)
            {
                CellDataList[index] = 7;
                i--;
            }
        }
    }

    public void BuildGrid()
    {
        var shiftX = new Vector3(dx, -dy);
        var shiftY = new Vector3(-dx, -dy);
        var topShift = new Vector3(0f, dy * (MapSizeX + MapSizeY) / 2);

        for (var x = 0; x < MapSizeX; x++)
        {
            for (var y = 0; y < MapSizeY; y++)
            {
                var id = CellDataList[y * MapSizeX + x];
                if (id == 0) continue;

                var newCell = Instantiate(cellPrefab[id - 1], transform);
                var pos = new Vector3();
                pos = topShift;
                pos += shiftX * x + shiftY * y;
                pos.z = -(x + y) * 0.01f;
                newCell.transform.localPosition = pos;
                newCell.name = string.Format("cell_{0}_{1}", x, y);

                var cell = newCell.GetComponent<CellView>();
                cellViewList[y * MapSizeX + x] = cell;
                cell.id = id;
                cell.x = x;
                cell.y = y;

                cell.speed = Random.Range(-5f, 5f);

                if (x != y) cell.speed += (x - y);
            }
        }
    }

    public bool BuildBridge(CellView clickCell)
    {
        if (clickCell.hasBridge) return false;
        var buildRate = 0;
        foreach (var nearCell in GetNear(clickCell))
            if (nearCell.hasBridge)
            {
                clickCell.hasBridge = true;
                var bridge = CreateBridge(clickCell, nearCell);
                var cost = CoreGame.Instance.BuildBridge(clickCell.id);
                bridge.ShowScore(-cost);
                buildRate += cost;
            }

        CoreGame.Instance.CompleteBuild(buildRate);
        return clickCell.hasBridge;
    }

    public void BuildCityBridge()
    {
        foreach (var cell in cellViewList)
        {
            if (cell==null) continue;
            if (!cell.IsTown) continue;

            foreach (var nearCell in GetNear(cell))
            {
                if (!nearCell.IsTown) continue;
                if (nearCell.y < cell.y) continue;
                if (nearCell.x < cell.x) continue;

                var newBridge = CreateBridge(cell,nearCell);
                newBridge.SetAlpha(0.6f);
            }
        }
    }

    private BridgeView CreateBridge(CellView fromCell, CellView toCell)
    {
        var newObject = Instantiate(bridgePrefab, transform);
        var newBridge = newObject.GetComponent<BridgeView>();
        bridgeViewList.Add(newBridge);

        var pos = (fromCell.transform.position + toCell.transform.position) / 2;
        pos.z = Mathf.Min(fromCell.transform.position.z, toCell.transform.position.z) - 0.01f;
        newObject.transform.position = pos;

        var difference = fromCell.transform.position - toCell.transform.position;
        if (difference.x < 0) difference *= -1f;
        var angle = Vector2.Angle(Vector2.down, difference);
        newObject.transform.Rotate(0, 0, angle);

        return newBridge;
    }

    private CellView this[int x, int y]
    {
        get
        {
            var index = y * MapSizeX + x;
            if (index < 0) return null;
            if (index >= cellViewList.Count) return null;
            return cellViewList[index];
        }
    }

    private IEnumerable<CellView> GetNear(CellView cell)
    {
        if (this[cell.x - 1, cell.y - 1] != null) yield return this[cell.x - 1, cell.y - 1];
        if (this[cell.x, cell.y - 1] != null) yield return this[cell.x, cell.y - 1];
        if (this[cell.x + 1, cell.y] != null) yield return this[cell.x + 1, cell.y];
        if (this[cell.x + 1, cell.y + 1] != null) yield return this[cell.x + 1, cell.y + 1];
        if (this[cell.x, cell.y + 1] != null) yield return this[cell.x, cell.y + 1];
        if (this[cell.x - 1, cell.y] != null) yield return this[cell.x - 1, cell.y];
    }

    /// <summary>считаем объединения в одну дорогу</summary>
    public int OneRoadTest()
    {
        var roadNum = 0;

        ClearRoadTest();

        for (var y = 0; y < MapSizeY; y++)
            for (var x = 0; x < MapSizeX; x++)
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
        return roadNum;
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
        //var result = -bridgeViewList.Count * 3;
        var result = 0;

        for (var i = 0; i < cellViewList.Count; i++)
        {
            var cell = cellViewList[i];
            if (null == cell) continue;
            if (cell.IsTown)
                result += GetTradeProfit(cell);
        }

        //Debug.LogFormat("total {0}", (float)result/CoreGame.TradePart);
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
                if (cell == null) continue;
                if (cell.roadTest != currentWave) continue;

                foreach (var nearCell in GetNear(cell))
                    if (nearCell.roadTest == Int32.MaxValue)
                    {
                        nextRoad = true;
                        nearCell.roadTest = currentWave - 1;
                        if (nearCell.IsTown)
                            result += currentWave - 1;
                    }
            }

            if (nextRoad)
                currentWave--;
            else
                currentWave = 0;
        }

        townCell.ShowScore((float)result/CoreGame.TradePart);

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
