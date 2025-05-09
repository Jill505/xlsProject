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
        
        showText.text = "�I���ù��}�l�C��";
        getWeather.WebGetWeather(true);
        showText.gameObject.GetComponent<Animator>().SetTrigger("startShine");
        showText.text = "���b����Ѯ��T...";
    }

    public void GameStart()
    {
        //Load Scene
    }

    void Update()
    {
        
    }
}