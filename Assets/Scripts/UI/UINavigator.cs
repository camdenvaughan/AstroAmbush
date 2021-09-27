using System;
using System.Collections;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Serialization;


public class UINavigator : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject volumeScreen;
    [SerializeField] private GameObject leaderboardScreen;
    [SerializeField] private GameObject changeNameScreen;
    
    [Header("Fader and Fader Properties")]
    [SerializeField] CanvasGroup fader;
    [SerializeField] float fadeTime;

    [Header("Game UI Text Objects")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text addedText;
    [SerializeField] private Text finalScore;
    [SerializeField] private Text highScore;
    [SerializeField] private Text clickText;
    private Animator addedTextAnim;

    [Header("Game UI Progress Bars")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image coolDownBar;
    
    [Header("Volume Sliders and Text Objects")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Text volumePercentage;
    [SerializeField] private Text effectPercentage;

    [Header("Buttons (Control Layouts)")]
    [SerializeField] private Button keyboardControls;
    [SerializeField] private Button mouseControls;

    [Header("Audio Manager Prefab")]
    public GameObject audioManagerPrefab;
    [HideInInspector] public AudioManager audioManager;
    [SerializeField] private AudioMixer mixer;


    [Header("LeaderBoard Objects")]
    public string[] leaderBoardItems = new string[10];
    [SerializeField] private Text[] leaderboardSlots = new Text[10];
    [SerializeField] private Text connectionErrorText;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowsParent;

    

    private string cachedDisplayName;
    

    private UIShipHandler uiShipHandler;

    private bool isFullScreen;

    public delegate void PauseStateEventHandler(object source, EventArgs args);

    public event PauseStateEventHandler PauseStateChanged;

    private void Awake()
    {
        SetDependencies();
    }
    
    private void SetDependencies()
    {
        uiShipHandler = GameObject.FindObjectOfType<UIShipHandler>();
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", .75f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume", .75f);
        addedTextAnim = addedText.gameObject.GetComponent<Animator>();
        audioManager = Instantiate(audioManagerPrefab).GetComponent<AudioManager>();
    }
    private void Start()
    {
        StartCoroutine(FadeIn());
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
            SetTitleUI();
        else if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            SetGameTimeUI();
        mixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume")) * 20);
        mixer.SetFloat("effectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("effectsVolume")) * 20);
        isFullScreen = PlayerPrefs.GetInt("isFullScreen", 0) == 0;
    }

    private void SetGameTimeUI()
    {
        menuScreen.SetActive(false);
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        volumeScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        changeNameScreen.SetActive(false);
        gameScreen.SetActive(true);
    }
    private void SetTitleUI()
    {
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameScreen.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        volumeScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        changeNameScreen.SetActive(false);
        menuScreen.SetActive(true);
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

    public void InitCoolDownBar()
    {
        coolDownBar.fillAmount = 0f;
    }

    public void SetCoolDownBar(float timer, float waitTime)
    {
        coolDownBar.fillAmount = timer / waitTime;
    }

    public void LoadScene(int index)
    {
        StartCoroutine(FadeOut(index));
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
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
        mixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
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
            mouseControls.GetComponent<Animator>().SetBool("isSelected", true);
            keyboardControls.GetComponent<Animator>().SetBool("isSelected", false);
        }
        else if (state == 1)
        {
            PlayerPrefs.SetInt("controlLayout", 1);
            mouseControls.GetComponent<Animator>().SetBool("isSelected", false);
            keyboardControls.GetComponent<Animator>().SetBool("isSelected", true);
        }
        else
        {
            Debug.Log("int outside of range placed into SetControlsMethod");
        }
    }

    public void ChangePauseState()
    {
        if (pauseScreen.gameObject.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            ClosePauseScreen();
        }
        else
        {
            gameScreen.SetActive(false);
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
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }
    
    // Settings

    public void ShowPauseScreen()
    {
        uiShipHandler.OpenPauseScreen();
        MoveToPauseScreen();
    }

    public void MoveToPauseScreen()
    {
        uiShipHandler.MoveCameraToPauseLoc(pauseScreen);
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
        uiShipHandler.MoveCameraToControlsLoc(controlsScreen, mouseControls, keyboardControls);
    }

    public void MoveToVolumeScreen()
    {

        uiShipHandler.MoveCameraToVolumeLoc(volumeScreen);

    }

    public void MoveToChangeNameScreen()
    {
        uiShipHandler.MoveCameraToChangeNameLoc(changeNameScreen);
    }

    public void ClosePauseScreen()
    {
        uiShipHandler.ClosePauseScreen(gameScreen);
    }
    

    public void CloseSettings()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            uiShipHandler.CloseSettings(menuScreen);
        else
            MoveToPauseScreen();
        
    }

    public void OpenLeaderBoard()
    {
        uiShipHandler.OpenLeaderBoard(leaderboardScreen);
        GetComponent<PlayFabManager>().GetLeaderBoard();
    }

    public void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        foreach (Transform child in rowsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (PlayerLeaderboardEntry item in result.Leaderboard)
        {
            GameObject newRow = Instantiate(rowPrefab, rowsParent);
            Text[] rowTexts = newRow.GetComponentsInChildren<Text>();

            rowTexts[0].text = (item.Position + 1).ToString();
            rowTexts[1].text = item.DisplayName;
            rowTexts[2].text = item.StatValue.ToString();
        }
    }

    public void CloseLeaderBoard()
    {
        uiShipHandler.CloseLeaderBoard(menuScreen);
    }
}
