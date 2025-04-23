using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class GetWeather : MonoBehaviour
{
    static public string apiKey = "6ed45a8dd19512226eb0de123d893809";
    static public string city = "Tainan";
    static public string countryCode = "TW";

    void Start()
    {
        StartCoroutine(WebGetWeather());
    }

    IEnumerator WebGetWeather()
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{countryCode}&appid={apiKey}&units=metric";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get weather data: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            WeatherData weatherData = JsonUtility.FromJson<WeatherData>(json);

            Debug.Log($"📍 地點：{weatherData.name}");
            Debug.Log($"🌡️ 溫度：{weatherData.main.temp} °C");
            Debug.Log($"💧 濕度：{weatherData.main.humidity}%");
            Debug.Log($"🌤️ 天氣狀態：{weatherData.weather[0].description}");

            // 你可以在這裡觸發場景變化，例如：改背景圖、開始下雨等
            //測試
        }
    }
}
[Serializable]
public class WeatherData
{
    public MainData main;
    public WeatherDescription[] weather;
    public string name;
}

[Serializable]
public class MainData
{
    public float temp;
    public int humidity;
}

[Serializable]
public class WeatherDescription
{
    public string description;
    public string icon;
}