using System;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class UiCore : MonoBehaviour
{
    public GameCore gameCore;

    public Text YenSync;
    public Text KgSync;

    [Header("Cane Canvas")]
    public CaneUnit nowLoadingCaneUnit;

    public GameObject CaneCanvas;
    public Image waterPercentage;
    public Image timePercentage;
    public Text timePercentageText;
    public Image muckPercentage;
    public Text nowKg;

    public GameObject StartPlantButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
    }

    // Update is called once per frame
    void Update()
    {
        syncData();
    }

    public void openCaneCanvas(CaneUnit caneUnit)
    {
        StartPlantButton.SetActive(false);

        //Load in Cane Data
        CaneCanvas.SetActive(true);
        nowLoadingCaneUnit = caneUnit;
        Debug.Log(JsonUtility.ToJson(nowLoadingCaneUnit));


        if (nowLoadingCaneUnit.isCanePlantFinished == true)
        {
            //make all other button fade and only left harvest button.
        }
        else if (nowLoadingCaneUnit.loadingCane.isPlantingCane == false)
        {
            //make all other button fade;
            //make plant button light up;
            StartPlantButton.SetActive(true);
        }
        else
        {
            waterPercentage.fillAmount = nowLoadingCaneUnit.CalculateWater();
            timePercentage.fillAmount = nowLoadingCaneUnit.CalculateTimeProgress();


            DateTime endTime = caneUnit.loadingCane.plantTime;
            TimeSpan TS = endTime.Add(GameCore.CaneGrowTime) - caneUnit.loadingCane.plantTime;
            //timePercentageText.text = "剩餘時間：" + nowLoadingCaneUnit.loadingCane.leftTimeSpan;
            TimeSpan ts = nowLoadingCaneUnit.loadingCane.leftTimeSpan;
            timePercentageText.text = $"剩餘時間：{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
        
    }

    public void syncData()
    {
        YenSync.text = "資產：" +gameCore.money+"円";
        KgSync.text = "甘蔗庫存：" + gameCore.holdingCaneKg + "斤";

        if (nowLoadingCaneUnit != null)
        {
            DateTime endTime = nowLoadingCaneUnit.loadingCane.plantTime;
            TimeSpan AllTime = endTime.Add(GameCore.CaneGrowTime) - nowLoadingCaneUnit.loadingCane.plantTime;

            //nowKg.text = "現有重量：" + nowLoadingCaneUnit.loadingCane.CaneKg;
            nowKg.text = "現有重量：" + nowLoadingCaneUnit.loadingCane.CaneKg.ToString("F1") + " 公斤";
            //判斷甘蔗是否長成
            if (nowLoadingCaneUnit.loadingCane.leftTimeSpan <= TimeSpan.Zero)
            {

            }
            else
            {

            }


            TimeSpan ts = nowLoadingCaneUnit.loadingCane.leftTimeSpan;
            timePercentageText.text = $"剩餘時間：{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";

            //Debug.Log("現載入進度：" + nowLoadingCaneUnit.CalculateTimeProgress());
            waterPercentage.fillAmount = nowLoadingCaneUnit.CalculateWater();
            timePercentage.fillAmount = nowLoadingCaneUnit.CalculateTimeProgress();
        }
    }

    public void StartPlant()
    {
        nowLoadingCaneUnit.loadingCane.isPlantingCane = true;

        //Set start currnet tume
        nowLoadingCaneUnit.loadingCane.plantTime = DateTime.Now;
        nowLoadingCaneUnit.loadingCane.warmCount = 0;

        nowLoadingCaneUnit.loadingCane.CaneKg = 0;

        //transform?
        //saveFile
        gameCore.SaveFile();
        //reset all data
        openCaneCanvas(nowLoadingCaneUnit);
    }

    public void waterPlus()
    {
        nowLoadingCaneUnit.loadingCane.nowWater += 5f;
    }
    public void waterMinus()
    {
        nowLoadingCaneUnit.loadingCane.nowWater -= 5f;
    }
}
