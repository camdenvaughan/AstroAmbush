using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private UINavigator uiNav;

    private Text timerText;

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
        
        shipTrans = GameObject.FindGameObjectWithTag("Ship").transform;
    }

    private void Update()
    {
        if (state == GameState.Active)
        {
            float t = Time.time - startTime;
            t += cachedTime;
            string minutes = ((int) t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + " : " + seconds;         
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
        else if (current.state == GameState.Paused)
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
        float finalScore = Time.time - current.startTime;
        string minutes = ((int) finalScore / 60).ToString();
        string seconds = (finalScore % 60).ToString("f2");
        current.timerText.text = minutes + ":" + seconds;
    }
}
