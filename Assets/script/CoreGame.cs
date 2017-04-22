using UnityEngine;

public class CoreGame : MonoBehaviour
{
    #region const
    /// <summary>Ключ куда мы сохраним игру</summary>
    public const string GameSaveKey = "gameSave";
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
        money = 20;
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
        tradeRate = profit / (50 + level*5);
        buildRate = currentRate;
        currentRate = 0;

        money += tradeRate - buildRate;
    }
    #endregion
}
