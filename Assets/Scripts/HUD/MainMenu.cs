
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{


    public TMP_Dropdown _resolutionDropdown;

    Resolution[] m_resolutions;

    private void Start()
    {
        m_resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();

        List<String> options = new List<String>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < m_resolutions.Length; i++)
        {
            string option = m_resolutions[i].width + "x" + m_resolutions[i].height;
            options.Add(option);

            if (m_resolutions[i].width == Screen.currentResolution.width &&
                m_resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
