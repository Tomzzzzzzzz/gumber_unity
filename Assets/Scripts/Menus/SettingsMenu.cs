using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (i == 0)
            {
                options.Add(option);
                continue;
            }
            if (option != resolutions[i-1].width + " x " + resolutions[i-1].height)
            {
                options.Add(option);
            }
        }

        resolutionDropdown.AddOptions(options);
    } 


    //Partie pour le son/l'audio

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }


    //Partie pour la qualite

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }


    //Partie pour le fullscreen

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    //Partie pour la resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



}
