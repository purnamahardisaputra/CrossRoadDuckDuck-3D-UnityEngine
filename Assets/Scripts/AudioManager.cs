using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Toggle muteToggle;
    [SerializeField] Slider SFXSlider;
    private void Awake() {
        float db;
        if(audioMixer.GetFloat("VolumeAll", out db))
            masterSlider.value = (db+80)/80;

        if(audioMixer.GetFloat("BGMVol", out db))
            BGMSlider.value = (db+80)/80;

        if(audioMixer.GetFloat("SFXVol", out db))
            SFXSlider.value = (db+80)/80;

        dropdown.value = QualitySettings.GetQualityLevel();

        if(PlayerPrefs.GetInt("Mute", 0) == 1)
            muteToggle.isOn = true;
        else
            muteToggle.isOn = false;

        
    }

    public void SceneLoader(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void setQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void muteToggleChanged() {
        if(muteToggle.isOn) {
            PlayerPrefs.SetInt("Mute", 1);
            AudioListener.pause = true;
        }
        else {
            PlayerPrefs.SetInt("Mute", 0);
            AudioListener.pause = false;
        }
        PlayerPrefs.Save();
    }



    public void SetMasterVolume(float volume) {
        volume = volume * 80 - 80;
        audioMixer.SetFloat("VolumeAll", volume);
        PlayerPrefs.SetFloat("VolumeAll", volume);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float volume) {
        volume = volume * 80 - 80;
        audioMixer.SetFloat("BGMVol", volume);
        PlayerPrefs.SetFloat("BGMVol", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume) {
        volume = volume * 80 - 80;
        audioMixer.SetFloat("SFXVol", volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
        PlayerPrefs.Save();
    }

}
