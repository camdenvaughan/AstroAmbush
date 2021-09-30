using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private UINavigator uiNav;
    
    
    private float timer;

    private Transform shipTrans;
    
    public enum GameState
    { WaitingForInput, Active, Paused, Ended, Tutorial }

    private GameState state = GameState.WaitingForInput;

    private bool gameHasStarted = false;

    private bool inputIsDisabled = false;

    private bool onTutorial = false;

    private int tutorialStage = 0;

    void Start()
    {
        current = this;
        SetDependencies();
    }

    void SetDependencies()
    {
        uiNav = FindObjectOfType<UINavigator>();

        uiNav.SetScore(0f);
        
        shipTrans = GameObject.FindGameObjectWithTag("Ship").transform;

        if (SceneManager.GetActiveScene().buildIndex == 2)
            onTutorial = true;
        
        SetTutorialText(true);
    }

    private void Update()
    {
        if (inputIsDisabled) return;
        
        if (state == GameState.Active || state == GameState.Tutorial)
        {
            timer += Time.deltaTime;
            uiNav.SetScore(timer);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            uiNav.ChangePauseState();

    }

    private void SetTutorialText(bool isOn)
    {
        string text = "";
        current.uiNav.SetBottomTutorialText("", false);
        if (current.state == GameState.WaitingForInput)
        {
            text = PlayerPrefs.GetInt("controlLayout", 0) == 0 ? "Click Mouse Button " : "Press Space Bar ";
            if (!gameHasStarted)
            {
                text += "to Start";
            }
            else
            {
                text += "to Resume";
            }
        }
        else if (current.state == GameState.Tutorial)
        {
            switch (current.tutorialStage)
            {
                case 0:
                    text = PlayerPrefs.GetInt("controlLayout", 0) == 0 ? "Move Mouse " : "Press 'W' 'A' 'S' 'D' Keys ";
                    text += "to Fly the Ship.";
                    current.uiNav.SetBottomTutorialText("As you Fly, the score will go up!");
                    break;
                case 1:
                    text = PlayerPrefs.GetInt("controlLayout", 0) == 0 ? "Click Mouse Button " : "Press Space Bar ";
                    text += "to Fire Blasters.";
                    current.uiNav.SetBottomTutorialText("If you fire too fast, you may overheat your weapons and will have to wait for it to cool down.");
                    break;
                case 2:
                    text = "Avoid Alien Ships and their Blaster Fire!";
                    current.uiNav.SetBottomTutorialText("Blasters will damage your Health, but colliding with an enemy will kill you on contact.");
                    break;
                case 3:
                    text = "Shoot at Alien Ships to destroy them and get a point bonus";
                    current.uiNav.SetBottomTutorialText("Destroy the 2 ships to finish the tutorial.");
                    break;
            }
        }

        uiNav.SetTopTutorialText(text, isOn);
    }

    public static void SetUniqueTutorialText(string text)
    {
        current.uiNav.SetTopTutorialText(text, true);
    }
    public static GameState GetState()
    {
        return current.state;
    }

    public static void SetState(GameState state)
    {
        current.state = state;
    }

    public static int GetTutorialStage()
    {
        return current.tutorialStage;
    }

    public static void IncrementTutorialStage()
    {
        current.tutorialStage++;
        current.SetTutorialText(true);
    }

    public static void EndTutorial()
    {
        current.state = GameState.Ended;
        current.uiNav.EndTutorial();
    }
    
    public static void SetGameToActive()
    {
        if (current.inputIsDisabled) return;

        current.gameHasStarted = true;
        
        if (!current.onTutorial)
        {
            current.state = GameState.Active;
            current.SetTutorialText(false);
            return;
        }

        current.state = GameState.Tutorial;
        current.SetTutorialText(true);
    }

    public static void PauseGame()
    {
        if (current.state == GameState.Ended)
            return;
        if (current.state == GameState.Paused)
        {
            current.state = GameState.WaitingForInput;
            current.SetTutorialText(true);
        }
        else
        {
            current.state = GameState.Paused;
            current.uiNav.ShowPauseScreen();
        }
    }
    public static Vector3 GetShipPos()
    {
        return current.shipTrans.position;
    }

    public static void AddToScore(float scoreToAdd)
    {
        current.uiNav.AddToScore(scoreToAdd);
    }

    public static void EndGame()
    {
        current.state = GameState.Ended;
        
        current.uiNav.SetFinalScore(current.timer);
        current.uiNav.SetHighScore(current.timer);
        current.uiNav.EndGame();
    }

    public static void ToggleInput()
    {
        current.inputIsDisabled = !current.inputIsDisabled;
    }
}
