using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Serialization;

public class PlayFabManager : MonoBehaviour
{
    [FormerlySerializedAs("name")] public string displayName = "NEW";
    void Start()
    {
        displayName = PlayerPrefs.GetString("displayName", "NEW");

        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = DeviceUniqueIdentifier,
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
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = displayName
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
    
    public static string DeviceUniqueIdentifier
    {
        get
        {
            var deviceId = "";
 
 
#if UNITY_EDITOR
            deviceId = SystemInfo.deviceUniqueIdentifier + "-editor1";
#elif UNITY_ANDROID
                AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
                AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");
                AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
                deviceId = secure.CallStatic<string> ("getString", contentResolver, "android_id");
#elif UNITY_WEBGL
                if (!PlayerPrefs.HasKey("UniqueIdentifier"))
                    PlayerPrefs.SetString("UniqueIdentifier", Guid.NewGuid().ToString());
                deviceId = PlayerPrefs.GetString("UniqueIdentifier");
#else
                deviceId = SystemInfo.deviceUniqueIdentifier;
#endif
            return deviceId;
        }
    }
}
