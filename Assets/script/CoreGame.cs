using System.Collections.Generic;
using UnityEngine;

public class CoreGame : MonoBehaviour
{
    #region const
    /// <summary>Ключ куда мы сохраним игру</summary>
    public const string MoneySaveKey = "moneySave";
    /// <summary>Ключ куда мы сохраним игру</summary>
    public const string LevelSaveKey = "levelSave";
    #endregion

    #region variables
    public static CoreGame Instance;
    /// <summary>Деньги на счету игрока</summary>
    public int money;
    private int currentRate;
    public int buildRate;
    public int tradeRate;
    public int level;
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
        currentRate = 0;
        PlayerPrefs.SetInt(LevelSaveKey,level);
        PlayerPrefs.SetInt(MoneySaveKey, money);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        level = PlayerPrefs.GetInt(LevelSaveKey, 0);
        money = PlayerPrefs.GetInt(MoneySaveKey, 100);
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

        PlayerPrefs.SetInt(LevelSaveKey,level);
        PlayerPrefs.SetInt(MoneySaveKey, money);
        PlayerPrefs.Save();
    }

    public void BuildBridge(int cellId)
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

        currentRate += rate;
    }

    public void SetTrade(int profit)
    {
        tradeRate = profit/10;
        buildRate = currentRate;
        currentRate = 0;
         
        money += (int)tradeRate - buildRate;
    }
    #endregion
}
