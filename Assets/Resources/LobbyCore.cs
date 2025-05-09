using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyCore : MonoBehaviour
{
    public GetWeather getWeather;
    public Text showText;

    void Start()
    {
        //Get Local Data
        
        showText.text = "點擊螢幕開始遊戲";
        getWeather.WebGetWeather(true);
        showText.gameObject.GetComponent<Animator>().SetTrigger("startShine");
        showText.text = "正在獲取天氣資訊...";
    }

    public void GameStart()
    {
        //Load Scene
    }

    void Update()
    {
        
    }
}