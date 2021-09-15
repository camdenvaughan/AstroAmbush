using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class UINavigator : MonoBehaviour
{
    [SerializeField] GameObject titleMenuUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameTimeUI;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject volumeScreen;
    [SerializeField] private GameObject leaderboardScreen;
    [SerializeField] CanvasGroup fader;
    [SerializeField] float fadeTime;

    public Text timerText;

    public Text finalTime;
    
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Text volumePercentage;
    [SerializeField] private Text effectPercentage;

    [SerializeField] private Button mouseControlsButton;
    [SerializeField] private Button keyControlsButton;


    [SerializeField] private AudioMixer Mixer;
    

    private UIShipHandler uiShipHandler;

    private bool isFullScreen;

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
        isFullScreen = PlayerPrefs.GetInt("isFullScreen", 0) == 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ChangePauseState();
    }

    private void SetGameTimeUI()
    {
        titleMenuUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        volumeScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        gameTimeUI.SetActive(true);
    }
    private void SetTitleUI()
    {
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameTimeUI.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        volumeScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        titleMenuUI.SetActive(true);
    }

    private void SetDependencies()
    {
        uiShipHandler = GameObject.FindObjectOfType<UIShipHandler>();
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", .75f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume", .75f);
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

    public void LoadScene(int index)
    {
        StartCoroutine(FadeOut(index));
    }
    
    public void OnFullScreenToggle()
    {
        PlayerPrefs.SetInt("isFullScreen", isFullScreen ? 0 : 1);
        Screen.fullScreen = isFullScreen;
    }

    public void SetMusicVolume(float volume)
    {
        Mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        int percentage = Mathf.RoundToInt(musicVolumeSlider.value * 100);
        volumePercentage.text = percentage.ToString() + "%";
    }

    public void SetEffectVolume(float volume)
    {
        Mixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", volume);
        int percentage = Mathf.RoundToInt(effectsVolumeSlider.value * 100);
        effectPercentage.text = percentage.ToString() + "%";

    }

    public void SetControls(int state)
    {
        if (state == 0)
        {
            PlayerPrefs.SetInt("controlLayout", 0);
        }
        else if (state == 1)
        {
            PlayerPrefs.SetInt("controlLayout", 1);
        }
        else
        {
            Debug.Log("int outside of range placed into SetControlsMethod");
        }
    }

    public void ChangePauseState()
    {
        if (pauseUI.gameObject.activeInHierarchy)
        {
            pauseUI.SetActive(false);
            gameTimeUI.SetActive(true);
            ClosePauseScreen();
        }
        else
        {
            gameTimeUI.SetActive(false);
        }
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

    public void EndGame()
    {
        gameTimeUI.SetActive(false);
        gameOverUI.SetActive(true);
    }
    
    // Settings

    public void ShowPauseScreen()
    {
        uiShipHandler.OpenPauseScreen();
        MoveToPauseScreen();
    }

    public void MoveToPauseScreen()
    {
        uiShipHandler.MoveCameraToPauseLoc(pauseUI);
    }
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

    public void ClosePauseScreen()
    {
        uiShipHandler.ClosePauseScreen();
    }
    

    public void CloseSettings()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            uiShipHandler.CloseSettings(titleMenuUI);
        else
            MoveToPauseScreen();
        
    }

    public void OpenLeaderBoard()
    {
        uiShipHandler.OpenLeaderBoard(leaderboardScreen);
    }

    public void CloseLeaderBoard()
    {
        uiShipHandler.CloseLeaderBoard(titleMenuUI);
    }
}
