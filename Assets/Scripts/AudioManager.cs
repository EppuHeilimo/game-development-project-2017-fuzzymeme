using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, Sfx, Music};

    public float masterVolumePrecent { get; private set; }
    public float sfxVolumePrecent { get; private set; }
    public float musicVolumePrecent { get; private set; }

    public static AudioManager instance;

    AudioSource sfx2DSource;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    Transform audioListener;
    Transform playerT;

    SoundLibrary library;

    void Awake()
    {

       
        if (instance != null)
        {
        
            Debug.Log("destroy obj");
            Destroy(gameObject);
        }

        else
        {

            instance = this;
            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];//create our music sources
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source" + (i));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }
            GameObject newSfx2Dsource = new GameObject("2D sfx source");
            sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();


            audioListener = FindObjectOfType<AudioListener>().transform;
            masterVolumePrecent = PlayerPrefs.GetFloat("master vol", 1);
            sfxVolumePrecent = PlayerPrefs.GetFloat("sfx vol", 1);
            musicVolumePrecent = PlayerPrefs.GetFloat("music vol", 1);
        }
    }
    void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void SetVolume(float volumePrecent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePrecent = volumePrecent;
                break;

            case AudioChannel.Sfx:
                sfxVolumePrecent = volumePrecent;
                break;

            case AudioChannel.Music:
                musicVolumePrecent = volumePrecent;
                break;
        }
        musicSources[0].volume = musicVolumePrecent * masterVolumePrecent;
        musicSources[1].volume = musicVolumePrecent * masterVolumePrecent;
        PlayerPrefs.SetFloat("master vol", masterVolumePrecent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePrecent);
        PlayerPrefs.SetFloat("music vol", musicVolumePrecent);
    }



    //for short sounds like effects
    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
           // Debug.Log(clip);
          //  Debug.Log("sfx"+ sfxVolumePrecent);
           // Debug.Log("master" + masterVolumePrecent);

            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePrecent * masterVolumePrecent);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        Debug.Log(soundName);
        PlaySound(library.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePrecent * masterVolumePrecent);
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePrecent * masterVolumePrecent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePrecent * masterVolumePrecent, 0, percent);
            yield return null;
        }
    }

    public static void PlayShootSound(string InventaryItemName, Transform _bulletSpawnPosition, string Shooter)
    {
        // Boss Minion
        // Debug.Log(Shooter);
        if (Shooter == "Boss" || Shooter == "Minion")
        {
            AudioManager.instance.PlaySound("Boss", _bulletSpawnPosition.transform.position);
            if (InventaryItemName == "")
            {
                
                //AudioManager.instance.PlaySound("BossRifle", _bulletSpawnPosition.transform.position);
            }

        }
        //normal sounds
        else
        {
                    if (InventaryItemName == "Stick")
                    {
                        //Debug.Log("make stick sound");
                    }
                    else if (InventaryItemName == "Rifle" || InventaryItemName == "Rusty Rifle")
                    {
                        AudioManager.instance.PlaySound("Rifle", _bulletSpawnPosition.transform.position);
                    }
                    else if (InventaryItemName == "Pistol" || InventaryItemName == "Rusty Pistol")
                    {
                        AudioManager.instance.PlaySound("Pistol", _bulletSpawnPosition.transform.position);
                    }

                    

                    else if (InventaryItemName == "Test")
                    {
                        AudioManager.instance.PlaySound("Rifle", _bulletSpawnPosition.transform.position);
                    }

            }

    }
    

    public static void PlayMultiBulletSound(string InventaryItemName,Transform BulletSpawnPosition)
    {
        Debug.Log(InventaryItemName);
        if (InventaryItemName == "Auto shotgun" || InventaryItemName == "Shotgun" || InventaryItemName == "Rusty Shotgun")
        {
            AudioManager.instance.PlaySound("Shotgun", BulletSpawnPosition.position);
        }

        
    }




}

