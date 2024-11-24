using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    public Light sunLight;
    public SkyboxManager skyboxManager;

    private string apiKey = "ce08e47134ca4d6c289ee7ab9d9c31e5";

    [Header("Buttons")]
    public Button city1Button;
    public Button city2Button;
    public Button city3Button;
    public Button city4Button;
    public Button city5Button;

    [Header("Cities")]
    public string city1 = "Orlando";
    public string city2 = "New York";
    public string city3 = "Los Angeles";
    public string city4 = "Chicago";
    public string city5 = "Houston";

    [System.Serializable]
    public class WeatherInfo
    {
        public Coord coord;
        public Weather[] weather;
        public Main main;
        public long dt; // This is the time
        public long timezone;
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

    private void Start()
    {
        city1Button.onClick.AddListener(() => FetchWeather(city1));
        city2Button.onClick.AddListener(() => FetchWeather(city2));
        city3Button.onClick.AddListener(() => FetchWeather(city3));
        city4Button.onClick.AddListener(() => FetchWeather(city4));
        city5Button.onClick.AddListener(() => FetchWeather(city5));

        SetButtonText(city1Button, city1);
        SetButtonText(city2Button, city2);
        SetButtonText(city3Button, city3);
        SetButtonText(city4Button, city4);
        SetButtonText(city5Button, city5);
    }

    private void SetButtonText(Button button, string cityName)
    {
        TMPro.TextMeshProUGUI tmpText = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        tmpText.text = cityName;
    }

        private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Network problem: {request.error}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Response error: {request.responseCode}");
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public void FetchWeather(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},us&mode=json&appid={apiKey}";
        StartCoroutine(CallAPI(url, OnWeatherDataLoaded));
    }

    private void OnWeatherDataLoaded(string data)
    {
        Debug.Log(data);
        WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(data);

        Debug.Log("Location: " + weatherInfo.coord.lat + ", " + weatherInfo.coord.lon);
        Debug.Log("Temperature: " + weatherInfo.main.temp);
        Debug.Log("Weather: " + weatherInfo.weather[0].main + " - " + weatherInfo.weather[0].description);
        Debug.Log("Time: " + DateTimeOffset.FromUnixTimeSeconds(weatherInfo.dt).DateTime.ToString());

        ChangeLight(weatherInfo);
    }

    private void ChangeLight(WeatherInfo weatherInfo) // based on weather
    {
        if (weatherInfo.weather[0].main == "Clear")
            sunLight.intensity = 2f;
        else if (weatherInfo.weather[0].main == "Thunderstorm")
            sunLight.intensity = .1f;
        else if (weatherInfo.weather[0].main == "Drizzle")
            sunLight.intensity = .9f;
        else if (weatherInfo.weather[0].main == "Rain")
            sunLight.intensity = .5f;
        else if (weatherInfo.weather[0].main == "Snow")
            sunLight.intensity = .5f;
        else if (weatherInfo.weather[0].main == "Clouds")
            sunLight.intensity = .75f;
        ChangeSkyBox(weatherInfo);
    }

    public void ChangeSkyBox(WeatherInfo weatherInfo) // based on time
    {
        long unixTimeUTC = weatherInfo.dt;
        long timezoneShift = weatherInfo.timezone;
        DateTime cityTime = DateTimeOffset.FromUnixTimeSeconds(unixTimeUTC + timezoneShift).DateTime;

        Debug.Log($"Local time in the city: {cityTime.ToString("HH:mm")}");

        int morningStartHour = 6;  // 6 am
        int noonStartHour = 12;    // 12 am
        int eveningStartHour = 17; // 5 pm
        int nightStartHour = 20;   // 8 pm
        if (cityTime.Hour >= morningStartHour && cityTime.Hour < noonStartHour)
        {
            skyboxManager.MakeMorning();
            sunLight.color = new Color(1.0f, 0.85f, 0.6f);
        }
        else if (cityTime.Hour >= noonStartHour && cityTime.Hour < eveningStartHour)
        {
            skyboxManager.MakeNoon();
            sunLight.color = new Color(1.0f, 1.0f, 0.8f);
        }
        else if (cityTime.Hour >= eveningStartHour && cityTime.Hour < nightStartHour)
        {
            skyboxManager.MakeEvening();
            sunLight.color = new Color(1.0f, 0.5f, 0.3f);
        }
        else
        {
            skyboxManager.MakeNight();
            sunLight.color = new Color(0.2f, 0.2f, 0.35f);
        }
        DynamicGI.UpdateEnvironment();
    }
}