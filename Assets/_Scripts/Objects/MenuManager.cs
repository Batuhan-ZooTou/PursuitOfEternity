using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

#region Quality&Resolution
public enum QualityLevel
{
    Fantastic,
    Beautiful,
    Good
}

public enum Resolution
{
    x1920x1080,
    x1600x900
}
#endregion

public class MenuManager : MonoBehaviour
{
    #region Headers
    //[Header("Volume Settings")]
    //[SerializeField] private TMP_Text volumeTextValue = null;
    //[SerializeField] private Slider volumeSlider = null;
    //[SerializeField] private Toggle MusicToggle = null;
    //
    //[Header("Loading Confirm Prompt")]
    //[SerializeField] private GameObject confirmationPrompt = null;
    //
    //[Header("Graphics Settings")]
    //[SerializeField] private QualityLevel qualityLevel = QualityLevel.Good;
    //[SerializeField] private Resolution resolution = Resolution.x1920x1080;
    //[SerializeField] private Toggle FullScreenToggle;
    //[SerializeField] private TMP_Dropdown ResolutionDropdown;
    //[SerializeField] private TMP_Dropdown QualityDropdown;
    #endregion

    #region Start&Quit
    public void StartGameButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(5);
    }

    public void IntroExitButton()
    {
        SceneManager.LoadScene(3);
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);

    }
    
    #endregion
    
    //#region Options
    //public void SetVolume(float volume)
    //{
    //    AudioListener.volume = volume;
    //    int intvolume = (int)volume;
    //    volumeTextValue.text = intvolume.ToString();
    //}
    //
    //public void VolumeApply()
    //{
    //    PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    //    StartCoroutine(ConfirmationBox());
    //}
    //
    //public IEnumerator ConfirmationBox()
    //{
    //    confirmationPrompt.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    confirmationPrompt.SetActive(false);
    //}
    //
    //public void SetQualityLevel()
    //{
    //    switch (qualityLevel)
    //    {
    //        case QualityLevel.Good:
    //            QualitySettings.SetQualityLevel(1, true);
    //            break;
    //        case QualityLevel.Beautiful:
    //            QualitySettings.SetQualityLevel(3, true);
    //            break;
    //        case QualityLevel.Fantastic:
    //            QualitySettings.SetQualityLevel(5, true);
    //            break;
    //    }
    //}
    //
    //public void OnValueChanged(TMP_Dropdown change)
    //{
    //    if (ResolutionDropdown = change)
    //    {
    //        resolution = (Resolution)change.value;
    //        SetResolution();
    //    }
    //
    //    if (QualityDropdown = change)
    //    {
    //        qualityLevel = (QualityLevel)change.value;
    //        SetQualityLevel();
    //    }
    //}
    //
    //public void SetResolution()
    //{
    //    switch (resolution)
    //    {
    //        case Resolution.x1920x1080:
    //            Screen.SetResolution(1920, 1080, true);
    //            break;
    //        case Resolution.x1600x900:
    //            Screen.SetResolution(1600, 900, true);
    //            break;
    //    }
    //}
    //
    //public void SetFulscreen()
    //{
    //    Screen.fullScreen = !Screen.fullScreen;
    //}
    //
    //public void SetVolumeMute()
    //{
    //    AudioListener.pause = !AudioListener.pause;
    //}
    //#endregion
}
