using System;
using System.Collections.Generic;
using UnityEngine;

public class CoreGame : MonoBehaviour
{
    #region const
    private const string SaveKey = "saveKey";

    public const int TradePart = 5; //10
    #endregion

    #region variables
    public static CoreGame Instance;
    /// <summary>Деньги на счету игрока</summary>
    public int money;
    public int buildRate;
    public int tradeRate;
    public int level;
    public readonly List<int> moneyHistory = new List<int>(50);
    #endregion

    #region constructor
    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    #endregion

    #region game
    public void StartGame()
    {
        level = 0;
        money = 100;
        tradeRate = 0;
        buildRate = 0;
        moneyHistory.Clear();
        SaveGame();
    }

    public void WinLevel()
    {
        level++;
        tradeRate = 0;
        buildRate = 0;
        moneyHistory.Add(money);
        SaveGame();
    }

    public int BuildBridge(int cellId)
    {
        var rate = 0;
        switch (cellId)
        {
            case 1:
                rate = 5; //поля
                break;
            case 2:
                rate = 10; // лес
                break;
            case 3:
                rate = 20; // горы
                break;
            case 4:
                rate = 40; // лес
                break;
        }
        return rate;
    }

    public void CompleteBuild(int currentRate)
    {
        buildRate = currentRate;
        money -= buildRate;

    }

    public void CompleteRoad(int profit)
    {
        tradeRate = profit/ TradePart;
        money += (int)tradeRate;
    }
    #endregion

    private void SaveGame()
    {
        var save = new GameProgress()
        {
            level = level,
            money = money,
            history = moneyHistory.ToArray()
        };

        var saveStr = JsonUtility.ToJson(save);
        Debug.LogFormat("save: {0}",saveStr);
        PlayerPrefs.SetString(SaveKey, saveStr);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        var saveStr = PlayerPrefs.GetString(SaveKey, string.Empty);
        if (string.IsNullOrEmpty(saveStr)) return;
        var save = JsonUtility.FromJson<GameProgress>(saveStr);

        level = save.level;
        money = save.money;
        moneyHistory.Clear();
        moneyHistory.AddRange(save.history);

        if (level <= 0) money = 100;
        tradeRate = 0;
        buildRate = 0;
    }


    private struct GameProgress
    {
        public int level;
        public int money;
        public int[] history;
    }
}
