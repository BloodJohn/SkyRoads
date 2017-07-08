using UnityEngine;

public class CoreGame : MonoBehaviour
{
    #region const
    /// <summary>Ключ куда мы сохраним игру</summary>
    private const string MoneySaveKey = "moneySave";
    /// <summary>Ключ куда мы сохраним игру</summary>
    private const string LevelSaveKey = "levelSave";

    public const int TradePart = 5; //10
    #endregion

    #region variables
    public static CoreGame Instance;
    /// <summary>Деньги на счету игрока</summary>
    public int money;
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
    }

    public void WinLevel()
    {
        level++;
        tradeRate = 0;
        buildRate = 0;

        PlayerPrefs.SetInt(LevelSaveKey,level);
        PlayerPrefs.SetInt(MoneySaveKey, money);
        PlayerPrefs.Save();
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
}
