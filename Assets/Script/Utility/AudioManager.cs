using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BackgroundMusic")]
    public AudioClip BGM;

    [Header("SFX Clips")]
    public AudioClip windClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayBGM();
    }

    public void PlayBGM()
    {
        if (BGM == null) return;

        bgmSource.clip = BGM;
        bgmSource.Play();
    }


    public void PlayWindSFX()
    {
        sfxSource.PlayOneShot(windClip);
    }
}
