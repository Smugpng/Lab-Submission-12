using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
public class LoginManager
{
    private ILogin loginMethod;
    public void SetLoginMathod(ILogin method)
    {
        loginMethod = method;
    }
    public void Login(System.Action<LoginResult> onSuccess, System.Action<PlayFabError> onFailure)
    {
        if (loginMethod != null) 
        {
            loginMethod.Login(onSuccess, onFailure);
        }
        else
        {
            Debug.LogError("No Login Method Set!");
        }
    }
}