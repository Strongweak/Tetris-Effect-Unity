using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;
    public Slider audioSlider;

    public float volume;
    public int quality;
    public bool isFullScreen;
    public int resolutionIndexbla;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Awake()
    {
        LoadSetting();
    }

    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullScreen(bool isFullScreens)
    {
        Screen.fullScreen = isFullScreen;
        isFullScreen = isFullScreens;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionIndexbla = resolutionIndex;
    }

    [System.Serializable]
    class SettingSave
    {
        public float volume;
        public int quality;
        public bool isFullScreen;
        public int resolutionIndex;
    }
    public void SaveSetting()
    {
        SettingSave data = new SettingSave();
        data.volume = audioSlider.value;
        data.quality = quality;
        data.isFullScreen = isFullScreen;
        data.resolutionIndex = resolutionIndexbla;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Save at :" + Application.persistentDataPath + "/savefile.json");
    }

    public void LoadSetting()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SettingSave data = JsonUtility.FromJson<SettingSave>(json);

            volume = data.volume;
            quality = data.quality;
            isFullScreen = data.isFullScreen;
            resolutionIndexbla = data.resolutionIndex;

            SetVolume(volume);
            Debug.Log(path);
        }
    }
}
