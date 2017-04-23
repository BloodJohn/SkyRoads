using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public bool IsSound;

    [SerializeField]
    private GameObject Music;
    [SerializeField]
    private AudioSource Sound;

    [SerializeField] private AudioClip LevelComplete;
    [SerializeField] private AudioClip GameOver;

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

    public void PlayLevelComplete()
    {
        if (!IsSound) return;
        Sound.clip = LevelComplete;
        Sound.pitch = Random.Range(0.6f, 1.2f);
        Sound.Play();
    }

    public void PlayGameOver()
    {
        if (!IsSound) return;
        Sound.clip = GameOver;
        Sound.pitch = Random.Range(0.6f, 1.2f);
        Sound.Play();
    }
}