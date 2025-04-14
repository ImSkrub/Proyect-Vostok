using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Opcional: Cargar valores guardados
        musicSlider.value = AudioManager.instance.MusicSource.volume;
        sfxSlider.value = AudioManager.instance.SFXSource.volume;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
     
    }

    public void SetMusicVolume(float volume)
    {
        float newVolume = volume * 0.05f;
        AudioManager.instance.MusicVolume(newVolume);
    }

    public void SetSFXVolume(float volume)
    {
        float newVolume = volume * 1f;
        AudioManager.instance.SFXVolume(newVolume);
    }
 
}
