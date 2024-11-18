using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    public string City; //input city
    private string jsonApi = "https://api.openweathermap.org/data/2.5/weather?q=Orlando,us&mode=json&appid=ce08e47134ca4d6c289ee7ab9d9c31e5";

    [System.Serializable]
    public class WeatherInfo
    {
        public Coord coord;
        public Weather[] weather;
        public Main main;
        public long dt; //this is the time
    }

    [System.Serializable]
    public class Coord
    {
        public float lon;
        public float lat;
    }

    [System.Serializable]
    public class Weather
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [System.Serializable]
    public class Main
    {
        public float temp;
        public int pressure;
        public int humidity;
    }


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
        WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(data);

        Debug.Log("Location: " + weatherInfo.coord.lat + ", " + weatherInfo.coord.lon);
        Debug.Log("Temperature: " + weatherInfo.main.temp);
        Debug.Log("Weather: " + weatherInfo.weather[0].main + " - " + weatherInfo.weather[0].description);
        Debug.Log("Time: " + System.DateTimeOffset.FromUnixTimeSeconds(weatherInfo.dt).DateTime.ToString());
        Debug.Log("Time: " + weatherInfo.dt);
    }
}