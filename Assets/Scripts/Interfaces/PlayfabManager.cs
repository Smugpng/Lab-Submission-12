using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using Unity.VisualScripting;

public class PlayfabManager
{
    private LoginManager loginManager;
    private string savedEmailKey = "SavedEmail";
    private string userEmail;
    private void Start()
    {
        loginManager = new LoginManager();

        if (PlayerPrefs.HasKey(savedEmailKey))
        {
            string savedEmail = PlayerPrefs.GetString(savedEmailKey);

            EmailLoginButtonClicked(savedEmail, "SavedPassword");
        }
    }

    public void EmailLoginButtonClicked(string email, string password)
    {
        userEmail = email;
        //loginManager.SetLoginMathod(new EmailLogin(email, password));
        loginManager.Login(OnLoginSuccess, OnLoginFailure);
    }

    public void DeviceIDLoginButtonClicked(string deviceId)
    {
        loginManager.SetLoginMathod(new DeviceLogin(deviceId));
        loginManager.Login(OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        if (!string.IsNullOrEmpty(userEmail))
        {
            PlayerPrefs.SetString(savedEmailKey, userEmail);
            LoadPlayerData(result.PlayFabId);
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login Failed" + error.ErrorMessage);
    }

    private void LoadPlayerData(string playfabId)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playfabId
        };
        PlayFabClientAPI.GetUserData(request, OnDataSuccess, OnDataFailure);
    }
    private void OnDataSuccess(GetUserDataResult result)
    {
        Debug.Log("Player Data Loaded successfully");
    }
    private void OnDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to load player data" + error.ErrorMessage);
    }
}
