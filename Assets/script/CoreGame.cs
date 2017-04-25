using System.Collections.Generic;
using UnityEngine;

public class CoreGame : MonoBehaviour
{
    #region const
    /// <summary>Ключ куда мы сохраним игру</summary>
    public const string GameSaveKey = "gameSave";

    public const int sizeX = 13;
    public const int sizeY = 13;

    public const int trimX = 3;
    public const int trimY = 9;
    #endregion

    #region variables
    public static CoreGame Instance;
    /// <summary>Деньги на счету игрока</summary>
    public int money;
    private int currentRate;
    public int buildRate;
    public int tradeRate;
    public int level;
    public readonly List<int> CellDataList = new List<int>(sizeX * sizeY);
    #endregion

    #region constructor
    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    #endregion

    #region game

    public void CreateLevelMap()
    {
        CellDataList.Clear();

        for (var y = 0; y < sizeY; y++)
            for (var x = 0; x < sizeX; x++)
            {
                if (x + y < trimX)
                {
                    CellDataList.Add(0);
                }
                else if (sizeX - 1 - x + sizeY - 1 - y < trimX)
                {
                    CellDataList.Add(0);
                }
                else if (sizeX - 1 - x + y < trimY)
                {
                    CellDataList.Add(0);
                }
                else if (x + sizeY - 1 - y < trimY)
                {
                    CellDataList.Add(0);
                }
                else
                {
                    var rnd = Random.Range(1, 10);
                    if (rnd==1 ||  rnd <= 5 - level / 4)
                        CellDataList.Add(1); //луга
                    else if (rnd == 2 || rnd <= 8 - level / 8)
                        CellDataList.Add(2); //лес
                    else if (rnd == 3 || rnd <= 9 - level / 16)
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


    public void StartGame()
    {
        level = 0;
        money = 15;
        tradeRate = 0;
        buildRate = 0;
        currentRate = 0;
    }

    public void WinLevel()
    {
        level++;
        tradeRate = 0;
        buildRate = 0;
        currentRate = 0;
    }

    public void BuildBridge(int cellId)
    {
        var rate = 0;
        switch (cellId)
        {
            case 1:
                rate = 1; //поля
                break;
            case 2:
                rate = 2; // лес
                break;
            case 3:
                rate = 3; // горы
                break;
            case 4:
                rate = 4; // лес
                break;
        }

        currentRate += rate;
    }

    public void SetTrade(int profit)
    {
        tradeRate = profit/50;
        buildRate = currentRate;
        currentRate = 0;

        money += tradeRate - buildRate;
    }
    #endregion
}
