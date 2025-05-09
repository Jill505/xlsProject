using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UIElements.Experimental;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetWeather : MonoBehaviour
{
    public Text showText;


    public static string nowWeather = "";
    public static float nowHumidity = 0;
    public static float nowTemp = 0;

    public static bool nowDataFake = false;
    public string nowDataFakeReason = "";

    static public string apiKey = "6ed45a8dd19512226eb0de123d893809";
    static public string city = "Tainan";
    static public string countryCode = "TW";

    void Start()
    {

    }

    public void WebGetWeather(bool loadGame)
    {
        StartCoroutine(WebGetWeatherCoroutine(loadGame));
    }

    IEnumerator WebGetWeatherCoroutine(bool loadGame)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{countryCode}&appid={apiKey}&units=metric";

        UnityWebRequest request = UnityWebRequest.Get(url);
        
        yield return request.SendWebRequest();
        Debug.Log("開始調用API");

        yield return new WaitForSeconds(1f);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get weather data: " + request.error);
            nowDataFake = true;
            nowDataFakeReason = "internet error";
            fakeTheData();
        }
        else
        {
            string json = request.downloadHandler.text;
            WeatherData weatherData = JsonUtility.FromJson<WeatherData>(json);

            Debug.Log($"地點：{weatherData.name}");
            Debug.Log($"溫度：{weatherData.main.temp} °C");
            Debug.Log($"濕度：{weatherData.main.humidity}%");
            Debug.Log($"天氣狀態：{weatherData.weather[0].description}");

            // 你可以在這裡觸發場景變化，例如：改背景圖、開始下雨等
            //測試

            if (weatherData.main.temp > 30f && weatherData.weather[0].description == "clear sky" || weatherData.weather[0].description == "few clouds || scattered clouds" || weatherData.weather[0].description == "broken clouds")
            {
                makeWeatherSun(weatherData.main.temp, weatherData.main.humidity);
            }
            else if (weatherData.weather[0].description == "clear sky" || weatherData.weather[0].description == "few clouds || scattered clouds" || weatherData.weather[0].description == "broken clouds")
            {
                makeWeatherRain(weatherData.main.temp, weatherData.main.humidity);
            }
            else 
            {
                makeWeatherCloud(weatherData.main.temp, weatherData.main.humidity);
            }
        }


        if (loadGame)
        {
            //load into the game
            SceneManager.LoadScene(1);
        }
    }
    public void makeWeatherSun(float temp, float wetness)
    {
        //烈陽天
        nowWeather = "Sun";
        nowTemp = temp;
        nowHumidity = wetness;
    }
    public void makeWeatherRain(float temp, float wetness)
    {
        //雨天
        nowWeather = "Rain";
        nowTemp = temp;
        nowHumidity = wetness;
    }
    public void makeWeatherCloud(float temp, float wetness)
    {
        //正常天
        nowWeather = "Cloud";
        nowTemp = temp;
        nowHumidity = wetness;
    }

    public void fakeTheData()
    {
        nowHumidity = UnityEngine.Random.Range(50, 90);
        int randomN = UnityEngine.Random.Range(0, 3);
        switch (randomN)
        {
            case 0:
                makeWeatherCloud(UnityEngine.Random.Range(20, 30),UnityEngine.Random.Range(40, 70));
                break;
            case 1:
                makeWeatherRain(UnityEngine.Random.Range(18, 29), UnityEngine.Random.Range(60, 90));
                break;
            case 2:
                makeWeatherSun(UnityEngine.Random.Range(29, 35), UnityEngine.Random.Range(20, 60));
                break;
            default:
                break;
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