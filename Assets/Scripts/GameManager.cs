using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private UINavigator uiNav;

    private Text timerText;
    private Text finalText;

    private float startTime;

    private Transform shipTrans;
    
    public enum GameState
    { WaitingForInput, Active, Paused, Ended }

    private GameState state = GameState.WaitingForInput;

    private bool gameHasStarted = false;

    private float cachedTime = 0f;
    void Start()
    {
        current = this;
        
        SetDependencies();
    }

    void SetDependencies()
    {
        uiNav = FindObjectOfType<UINavigator>();
        timerText = uiNav.timerText;
        finalText = uiNav.finalTime;
        
        shipTrans = GameObject.FindGameObjectWithTag("Ship").transform;
    }

    private void Update()
    {
        if (state == GameState.Active)
        {
            float t = Time.time - startTime;
            t += cachedTime;
            string points = ((int) t).ToString();
            timerText.text = points;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                uiNav.ChangePauseState();
        }
        

    }

    public static GameState GetState()
    {
        return current.state;
    }

    public static void SetGameToActive()
    {
        if (!current.gameHasStarted)
        {
            current.gameHasStarted = true;
        }

        current.state = GameState.Active;
        current.startTime = Time.time;
    }

    public static void PauseGame()
    {
        if (current.state == GameState.Ended)
            return;
        if (current.state == GameState.Paused)
            current.state = GameState.WaitingForInput;
        else
        {
            current.state = GameState.Paused;
            current.uiNav.ShowPauseScreen();
            float t = Time.time - current.startTime;
            current.cachedTime += t;
        }
    }
    public static Vector3 GetShipPos()
    {
        return current.shipTrans.position;
    }

    public static void EndGame()
    {
        current.state = GameState.Ended;
        float t = Time.time - current.startTime;
        t += current.cachedTime;
        string points = ((int) t).ToString();
        current.finalText.text = points;
        current.uiNav.EndGame();
    }
}
