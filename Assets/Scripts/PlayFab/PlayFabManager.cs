using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.PlayerLoop;

public class PlayFabManager : MonoBehaviour
{

    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnLoginError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/account creation");
        UpdateDisplayName();
    }

    void OnLoginError(PlayFabError error)
    {
        Debug.Log("error logging into playfab");
        GetComponent<UINavigator>().connectionErrorText.gameObject.SetActive(true);
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());

    }

    public void UpdateDisplayName()
    {
        string name = PlayerPrefs.GetString("displayName", "NEW");
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateDisplayName, OnError);
    }

    void OnUpdateDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Display Name Updated to " + result.DisplayName);
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
        
        PlayFabClientAPI.GetLeaderboard(request, GetComponent<UINavigator>().OnLeaderBoardGet, OnError);
    }
}
