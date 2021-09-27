using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.PlayerLoop;

public class PlayFabManager : MonoBehaviour
{
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
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/account creation");
        connected = true;
        UpdateDisplayName();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("error logging into playfab");
        Debug.Log(error.GenerateErrorReport());
        connected = false;
    }

    public void UpdateDisplayName()
    {
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = PlayerPrefs.GetString("displayName", "NEW"),
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
