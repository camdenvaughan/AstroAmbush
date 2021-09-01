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
    [SerializeField] CanvasGroup fader;
    [SerializeField] float fadeTime;

    public Text timerText;

    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Dropdown controlOptions;
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private AudioMixer mixer;

    Resolution[] resolutions;


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
        mixer.SetFloat("volume", Mathf.Log10(PlayerPrefs.GetFloat("volume")) * 20);
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
        fullScreenToggle.isOn = PlayerPrefs.GetInt("isFullScreen") == 0;
        Screen.fullScreen = fullScreenToggle.isOn;
        resolutions = Screen.resolutions;
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    public void OnGameFinished(string winner)
    {
        gameOverUI.SetActive(true);
        timerText.text = string.Format("{0} won", winner);
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

    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
}
