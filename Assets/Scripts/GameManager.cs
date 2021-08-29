using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private bool gameIsActive = false;

    [SerializeField] private Text timerText;

    private float startTime;

    private Transform shipTrans;
    void Start()
    {
        current = this;
        shipTrans = GameObject.FindGameObjectWithTag("Ship").transform;
    }

    private void Update()
    {
        if (gameIsActive)
        {
            float t = Time.time - startTime;
            string minutes = ((int) t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + " : " + seconds;         
        }

    }

    public static void StartGame()
    {
        current.StartGameImpl();
    }

    private  void StartGameImpl()
    {
        gameIsActive = true;
        startTime = Time.time;
    }

    public static bool GameIsActive()
    {
        return current.GameIsActiveImpl();
    }

    private  bool GameIsActiveImpl()
    {
        return gameIsActive;
    }

    public static Vector3 GetShipPos()
    {
        return current.GetShipPosImpl();
    }

    private  Vector3 GetShipPosImpl()
    {
        return shipTrans.position;
    }

    public static void EndGame()
    {
        current.EndGameImpl();
    }

    private void EndGameImpl()
    {
        gameIsActive = false;
        float finalScore = Time.time - startTime;
        string minutes = ((int) finalScore / 60).ToString();
        string seconds = (finalScore % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
    }
}
