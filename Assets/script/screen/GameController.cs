using UnityEngine;

public class GameController : MonoBehaviour
{
    public const string sceneName = "game";

    #region stuff

    private void ShowStats()
    {
        if (CoreGame.Instance == null) return;
    }
    #endregion
}
