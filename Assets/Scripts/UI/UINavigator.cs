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

    [SerializeField] private Text scoreText;
    [SerializeField] private Text addedText;
    [SerializeField] private Text finalScore;
    [SerializeField] private Text highScore;
    [SerializeField] private Text clickText;
    private Animator addedTextAnim;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider coolDownBar;
    
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Text volumePercentage;
    [SerializeField] private Text effectPercentage;

    public AudioManager audioManager;

    public string[] leaderBoardItems = new string[10];
    [SerializeField] private Text[] leaderboardSlots = new Text[10];
    [SerializeField] private Text connectionErrorText;

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
        addedTextAnim = addedText.gameObject.GetComponent<Animator>();
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

    public void PlayButtonClick()
    {
        audioManager.Play("button");
    }

    public void InitHealthBar(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void SetHealthBar(int healthPoints)
    {
        healthBar.value = healthPoints;
    }

    public void InitCoolDownBar(float maxTimer)
    {
        coolDownBar.maxValue = maxTimer;
        coolDownBar.value = -1;
    }

    public void SetCoolDownBar(float timer)
    {
        coolDownBar.value = timer;
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
        audioManager.Play("volume knob");

    }

    public void SetScore(float score)
    {
        scoreText.text = ((int) score).ToString();
    }

    public void AddToScore(float score)
    {
        addedText.text = "+" + ((int) score).ToString();
        addedTextAnim.SetTrigger("play");
    }

    public void SetFinalScore(float score)
    {
        finalScore.text = ((int) score).ToString();
    }
    public void SetHighScore(float score)
    {
        int iScore = (int) score;
        int hs = PlayerPrefs.GetInt("highscore", 0);
        if (score > hs)
        {
            hs = iScore;
            PlayerPrefs.SetInt("highscore", iScore);
            GetComponent<PlayFabManager>().SendLeaderBoard(iScore);
        }
        highScore.text = hs.ToString();
    }

    public void SetTutorialText(string text, bool isOn)
    {
        clickText.text = text;
        clickText.gameObject.SetActive(isOn);
    }

    public void SetEffectVolume(float volume)
    {
        Mixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", volume);
        int percentage = Mathf.RoundToInt(effectsVolumeSlider.value * 100);
        effectPercentage.text = percentage.ToString() + "%";
        audioManager.Play("volume knob");
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
        GetLeaderBoard();
        uiShipHandler.OpenLeaderBoard(leaderboardScreen);

    }

    private void GetLeaderBoard()
    {
        PlayFabManager playFabManager = GetComponent<PlayFabManager>();

        foreach (Text slot in leaderboardSlots)
        {
            slot.gameObject.SetActive(playFabManager.connected);
        }
        connectionErrorText.gameObject.SetActive(!playFabManager.connected);
        
        playFabManager.GetLeaderBoard();
        for (int i = 0; i < leaderBoardItems.Length; i++)
        {
            if (leaderBoardItems[i] != null)
            {
                leaderboardSlots[i].text = leaderBoardItems[i];
            }
        }
    }

    public void CloseLeaderBoard()
    {
        uiShipHandler.CloseLeaderBoard(titleMenuUI);
    }
}
