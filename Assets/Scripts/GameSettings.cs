using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public static GameSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        List<string> resStrings = new List<string>();

        int currentRes = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resStrings.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (CompareResolution(resolutions[i])) currentRes = i;
        }

        resolutionDropdown.AddOptions(resStrings);
        resolutionDropdown.value = currentRes;

        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float amount)
    {
        mixer.SetFloat("master_volume", amount);
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    public void SetFullscreen(bool state)
    {
        Screen.fullScreen = state;
    }

    bool CompareResolution(Resolution resolution)
    {
        if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
        {
            return true;
        }
        return false;
    }
}