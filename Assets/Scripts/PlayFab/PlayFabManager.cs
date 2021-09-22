using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    private string[] leaderboardInfo = new string[10];

    public bool connected = false;
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = PlayerPrefs.GetString("leaderboardName", "NEW"),
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/account creation");
        connected = true;
        GetLeaderBoard();

    }

    void OnError(PlayFabError error)
    {
        Debug.Log("error logging into playfab");
        Debug.Log(error.GenerateErrorReport());
        connected = false;
    }

    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard send");
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        UINavigator uiNav = GetComponent<UINavigator>();
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            int position = result.Leaderboard[i].Position + 1;
            string info = position + ". Score: " + result.Leaderboard[i].StatValue;

            uiNav.leaderBoardItems[i] = info;
        }
    }
}
