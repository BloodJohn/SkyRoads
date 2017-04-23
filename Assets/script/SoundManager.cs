using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public bool IsSound;

    [SerializeField]
    private GameObject Music;

    private static readonly string SoundKey = "muteSound";

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 10;

        IsSound = PlayerPrefs.GetInt(SoundKey, 100) > 0;
        Music.SetActive(IsSound);
    }

    public void MuteSound()
    {
        if (!IsSound)
        {
            IsSound = true;
            PlayMusic();
        }
        else
        {
            IsSound = false;
            StopSound();
        }

        PlayerPrefs.SetInt(SoundKey, IsSound ? 100 : 0);
    }

    public void StopSound()
    {
        Music.SetActive(false);
    }

    public void PlayMusic()
    {
        if (!IsSound) return;
        Music.SetActive(true);
    }
}