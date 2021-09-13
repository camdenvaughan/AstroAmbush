using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class UINavigator : MonoBehaviour
{
    [SerializeField] GameObject titleMenuUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameTimeUI;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject volumeScreen;
    [SerializeField] CanvasGroup fader;
    [SerializeField] float fadeTime;

    public Text timerText;

    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;

    [SerializeField] private AudioMixer Mixer;
    

    private UIShipHandler uiShipHandler;

    public delegate void PauseStateEventHandler(object source, EventArgs args);

    public event PauseStateEventHandler PauseStateChanged;

    private void Awake()
    {
        SetDependencies();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
            SetTitleUI();
        else if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            SetGameTimeUI();
        Mixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume")) * 20);
        Mixer.SetFloat("effectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("effectsVolume")) * 20);
    }

    private void SetGameTimeUI()
    {
        titleMenuUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameTimeUI.SetActive(true);
    }
    private void SetTitleUI()
    {
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameTimeUI.SetActive(false);
        titleMenuUI.SetActive(true);
    }

    private void SetDependencies()
    {
        uiShipHandler = GameObject.FindObjectOfType<UIShipHandler>();
        fullScreenToggle.isOn = PlayerPrefs.GetInt("isFullScreen") == 0;
        Screen.fullScreen = fullScreenToggle.isOn;
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume");
    }
    
    
    protected virtual void OnPauseStateChanged()
    {
        PauseStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void HideUI(GameObject hideUI)
    {
        hideUI.SetActive(false);
    }
    public void ShowUI(GameObject showUI)
    {
        showUI.SetActive(true);
    }
    public void BackFromSettings()
    {
        if (gameTimeUI.activeSelf)
            pauseUI.SetActive(true);
        else
            titleMenuUI.SetActive(true);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(FadeOut(index));
    }
    
    public void OnFullScreenToggle()
    {
        PlayerPrefs.SetInt("isFullScreen", fullScreenToggle.isOn ? 0 : 1);
        Screen.fullScreen = fullScreenToggle.isOn;
    }

    public void SetMusicVolume(float volume)
    {
        Mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        Mixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }

    public void SetControls(int state)
    {
        if (state == 0)
        {
            
        }
        else if (state == 1)
        {
            
        }
        else
        {
            Debug.Log("int outside of range placed into SetControlsMethod");
        }
    }

    public void ChangePauseState()
    {
        OnPauseStateChanged();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator FadeIn()
	{
		while (fader.alpha > 0f)
		{
            fader.alpha -= Time.deltaTime / fadeTime;
            yield return null;
		}
        fader.gameObject.SetActive(false);
    }
    private IEnumerator FadeOut(int index)
    {
        fader.gameObject.SetActive(true);
        while (fader.alpha < 1f)
        {
            fader.alpha += Time.deltaTime / fadeTime;
            yield return null;
        }
        SceneManager.LoadScene(index);
    }
    
    // Settings

    public void OpenSettings()
    {
        uiShipHandler.OpenSettings();
        MoveToSettingsScreen();
    }
    public void MoveToSettingsScreen()
    {
        uiShipHandler.MoveCameraToSettingsLoc(settingsScreen);
    }

    public void MoveToControlsScreen()
    {
        uiShipHandler.MoveCameraToControlsLoc(controlsScreen);
    }

    public void MoveToVolumeScreen()
    {
        uiShipHandler.MoveCameraToVolumeLoc(volumeScreen);

    }

    public void CloseSettings()
    {
        uiShipHandler.CloseSettings(titleMenuUI);
    }
}
