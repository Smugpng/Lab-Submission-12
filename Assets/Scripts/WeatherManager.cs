using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    public string City;
    private string jsonApi = "https://api.openweathermap.org/data/2.5/weather?q=Orlando,us&mode=json&appid=ce08e47134ca4d6c289ee7ab9d9c31e5";

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"network problem: {request.error}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"response error: {request.responseCode}");
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }
    public void Start()
    {
        string test  = ("https://api.openweathermap.org/data/2.5/weather?q=" + City + ",us&mode=json&appid=ce08e47134ca4d6c289ee7ab9d9c31e5");
        jsonApi = test;
        StartCoroutine(GetWeatherXML(OnXMLDataLoaded));
    }
    public void OnXMLDataLoaded(string data)
    {
        Debug.Log(data);
    }
}