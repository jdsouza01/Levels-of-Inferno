using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip automaticRifleShot;
    public AudioClip shotGunShot;
    public AudioClip walk;
    public AudioClip reload;
    public static bool walking;

    private AudioSource gunSource;
    private AudioSource audioSource;
    private AudioSource walkSource;
    private AudioSource reloadSource;
    public static AudioManager instance;

    void Awake()
    {
        // Singleton setup to persist across scenes
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); Persist this object between scene loads
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set up audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = menuMusic;
        audioSource.volume = 0.5f;
        audioSource.Play();

        gunSource = gameObject.AddComponent<AudioSource>();
        gunSource.loop = false;

        walkSource = gameObject.AddComponent<AudioSource>();
        walkSource.loop = true;
        walkSource.clip = walk;

        reloadSource = gameObject.AddComponent<AudioSource>();
        reloadSource.loop = true;
        reloadSource.clip = reload;
    }

    public void shot()
    {
        gunSource.Play();
    }

    public void assignGunNoise(string weapon)
    {
        switch (weapon)
        {
            case "automaticRifle":
                gunSource.clip = automaticRifleShot;
                break;

            case "shotGun":
                gunSource.clip = shotGunShot;
                break;
        }

    }

    public void startWalk()
    {
        walkSource.Play();
    }

    public void stopWalk()
    {
        walkSource.Stop();
    }

    public void reloadSound()
    {
        reloadSource.Play();
    }

    public void reloadStop()
    {
        reloadSource.Stop();
    }
}

