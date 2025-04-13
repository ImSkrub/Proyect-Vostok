using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource MusicSource, SFXSource, LoopSFXSource;
    public Sound[] MusicSounds, SFXSounds;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("menu");
    }

    public void PlayMusic(string name)
    {
        Debug.Log($"🔍 Buscando música con nombre: '{name}'");
        Sound s = Array.Find(MusicSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.LogWarning($"❌ Música no encontrada: '{name}'. Revisá si está mal escrito o no cargado.");
        }
        else
        {
            Debug.Log($"✅ Reproduciendo música: {s.Name}");
            MusicSource.clip = s.Clip;
            MusicSource.Play();
        }
    }
    public void StopMusic()
    {
        MusicSource.Stop();
    }
    public void PlaySFX(string name, bool loop = false) 
    {
        Sound s = Array.Find(SFXSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
        }

        else
        {
            SFXSource.loop = loop;
            SFXSource.PlayOneShot(s.Clip);
        }

    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public bool isMusicPlaying(string name)
    {
        Console.WriteLine("hola que tal");
        if (MusicSource.name == name)
        {
            Console.WriteLine("hola");
            return true;
        } else
        {
            Console.WriteLine("chau");
            return false;
        }
    }

    public void ToggleMusic()
    {
        MusicSource.mute = !MusicSource.mute;
    }
    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
    public void PlayLoopSFX(string name)
    {
        if (!LoopSFXSource.isPlaying)
        {
            Sound s = Array.Find(SFXSounds, x => x.Name == name);

            if (s == null)
            {
                Debug.Log("sound not found");
            }

            else
            {
                LoopSFXSource.loop = true;
                LoopSFXSource.PlayOneShot(s.Clip);
            }
        }
    }
    public void StopLoopSFX()
    {
        LoopSFXSource.Stop();
    }
    public void StopAll()
    {
        StopLoopSFX();
        StopSFX();
        StopMusic();
    }


}
