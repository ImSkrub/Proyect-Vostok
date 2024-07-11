using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("----Audio Source-----")]
    [SerializeField] AudioSource MusicSource, SFXSource;
    [Header("-----Audio CLip-----")]
    //Sound maneja audioclip y nombre. Y lo guarda en un array.
    public Sound[] MusicSounds, SFXSounds;

    
    
    /*
     * Si queremos hacer sonidos que tengan en cuenta la distancia con el player el audio tiene que ser en 3D.
     * Si es en 2D se reproduce constantemente, pero se puede activar o desactivar con los checkpoints.
    */
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
        //PlayMusic("Theme");
        //playerCharacter.ChestSound.AddListener(ChestSound);
        //playerCharacter.DoorSound.AddListener(DoorSound);

    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
        }

        else
        {

            MusicSource.clip = s.Clip;
            MusicSource.Play();
        }
    }
    public void PlaySFX(string name) 
    {
        Sound s = Array.Find(SFXSounds, x => x.Name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
        }

        else
        {
            SFXSource.PlayOneShot(s.Clip);
        }

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
    public void DoorSound()
    {
        PlaySFX("Door");
    }
    public void ChestSound()
    {
        PlaySFX("Chest");
    }

}
