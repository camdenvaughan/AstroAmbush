using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private UINavigator uiNav;
    
    
    private float timer;

    private Transform shipTrans;
    
    public enum GameState
    { WaitingForInput, Active, Paused, Ended }

    private GameState state = GameState.WaitingForInput;

    private bool gameHasStarted = false;

    private bool inputIsDisabled = false;
    
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
    }

    private void Update()
    {
        if (inputIsDisabled) return;
        
        if (state == GameState.Active)
        {
            timer += Time.deltaTime;
            uiNav.SetScore(timer);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            uiNav.ChangePauseState();

    }

    private void SetTutorialText(bool isOn)
    {
        string text = PlayerPrefs.GetInt("controlLayout", 0) == 0 ? "Click Mouse Button " : "Press Space Bar ";
        if (!gameHasStarted)
        {
                text += "to Start Game";
        }
        else
        {
                text += "to Resume Game";
        }
        uiNav.SetTutorialText(text, isOn);
    }
    public static GameState GetState()
    {
        return current.state;
    }

    public static void SetGameToActive()
    {
        if (current.inputIsDisabled) return;
        
        if (!current.gameHasStarted)
        {
            current.gameHasStarted = true;
        }

        current.state = GameState.Active;

        current.SetTutorialText(false);
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
