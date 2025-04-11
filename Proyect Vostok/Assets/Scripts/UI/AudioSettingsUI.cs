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
        AudioManager.instance.MusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SFXVolume(volume);
    }
 
}
