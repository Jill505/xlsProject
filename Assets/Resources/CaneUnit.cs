using JetBrains.Annotations;
using NUnit.Framework.Internal.Commands;
using System;
using System.Globalization;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaneUnit : MonoBehaviour
{
    public Cane loadingCane;
    public GameCore gameCore;

    public int myCaneSort = 0;//0~3 ABCD

    [Tooltip("這個為true的時候整個開始跑")]
    // bool isCanePlanting = false;
    public bool isCanePlantFinished = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        //Load cane data   
        //loadCaneData();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DateTime endTime = loadingCane.plantTime;
        endTime.Add(GameCore.CaneGrowTime);

        SyncCaneData();
        CaneBasicCheck();
    }

    private void FixedUpdate()
    {
        CaneGameGoingCalculate();
    }

    public void CaneGameGoingCalculate()
    {
        //判斷是否正在生長狀態中
        if (!loadingCane.isPlantingCane || loadingCane.isAbleToHarvest)
        {
            // 若未種植或已經成熟，不需要計算
            Debug.Log(loadingCane + " 不需要計算離線成長。");
            return;
        }

        // 判斷是否已經超過生長期
        if (loadingCane.plantTime.Add(GameCore.CaneGrowTime) <= DateTime.Now)
        {
            // 進入成熟狀態
            loadingCane.isAbleToHarvest = true;
            loadingCane.isPlantingCane = false;
            loadingCane.leftTimeSpan = TimeSpan.Zero;

            Debug.Log(loadingCane + " 離線期間甘蔗已成熟，可以收成！");
            return;
        }

        double baseGrowth = 1 * 0.2 / 60 / 50; // 基礎成長 (0.2 kg / min)
        double growthPenaltyPerMinute = 0.0;

        int ctConst = 1;
        if (GetWeather.nowWeather == "Sun")
        {
            ctConst = 2;
        }

        if (UnityEngine.Random.Range(0, 240 * 60 * 50) <= (GetWeather.nowTemp / 3 * ctConst))
        {
            loadingCane.warmCount += 1;
        }

        growthPenaltyPerMinute += loadingCane.warmCount * 0.05 / 60 / 50;

        float waterCT = 0;
        float waterSunDec = 1;

        if (GetWeather.nowWeather == "Sun")
        {
            waterSunDec = 1.4f;
        }

       
        float randomWN =(1+ UnityEngine.Random.Range(0f, Mathf.Clamp01(0.6f + (GetWeather.nowTemp * 0.1f))))/60/50;
        randomWN *= waterSunDec;

        if (GetWeather.nowWeather == "Rain")
        {
            loadingCane.nowWater += (GetWeather.nowHumidity - GetWeather.nowTemp)/100/60/50;
        }
        else
        {
            loadingCane.nowWater -= randomWN;
            waterCT += randomWN;
        }


        if (loadingCane.nowWater < 20)
        {
            double deficit = 20 - loadingCane.nowWater;
            growthPenaltyPerMinute += (deficit / 2) * 0.03 / 60 / 50;
        }
        else if (loadingCane.nowWater > 80)
        {
            double excess = loadingCane.nowWater - 80;
            growthPenaltyPerMinute += (excess / 2) * 0.03 / 60 / 50;
        }

        double actualGrowth = baseGrowth - (growthPenaltyPerMinute/60 / 50);
        loadingCane.CaneKg += (float)actualGrowth;
    }

    public void CaneRolling()
    {

    }
    public void SyncCaneData()
    {
        switch(myCaneSort) { 
        case 0:
                gameCore.CaneA = loadingCane;
            break;
        case 1:
                gameCore.CaneB = loadingCane;
                break;
        case 2:
                gameCore.CaneC = loadingCane;
                break;
        case 3:
                gameCore.CaneD = loadingCane;
                break;
        }
    }

    public void CaneBasicCheck()
    {
        DateTime endTime = loadingCane.plantTime;

        endTime.Add(GameCore.CaneGrowTime);
        loadingCane.leftTimeSpan = endTime.Add(GameCore.CaneGrowTime) - DateTime.Now;

        //Debug.Log("loadingCane.plantTime: " + loadingCane.plantTime);
        //Debug.Log("Checking, loadingCane.leftTimeSpan: " + loadingCane.leftTimeSpan.TotalMinutes);
        // 生長時間已到，但尚未進入可收成狀態
        if (loadingCane.leftTimeSpan <= TimeSpan.Zero && loadingCane.isAbleToHarvest == false)
        {
            // 更新狀態為可收割
            Debug.Log("loadingCane.leftTimeSpan: " + loadingCane.leftTimeSpan.TotalMinutes);
            Debug.Log("生成clog來自 CaneBasicCheck");
            loadingCane.leftTimeSpan = TimeSpan.Zero;
            loadingCane.isAbleToHarvest = true;
            loadingCane.isPlantingCane = false;

            //Console.WriteLine("甘蔗已成熟，可以收成了，瞬間事件！");
        }
        // 如果目前允許收割
        else if (loadingCane.isAbleToHarvest == true)
        {
            //Console.WriteLine("甘蔗可供收成中。");
        }
        // 還在生長中
        else if (loadingCane.isPlantingCane == true)
        {
            //Console.WriteLine($"甘蔗成長中，剩餘時間：{loadingCane.leftTimeSpan.TotalMinutes} 分鐘。");
        }
        else
        {
            // 非預期狀態（例：未種植）
            Console.WriteLine("尚未種植甘蔗。");
        }
    }

    public void Harvest()
    {
        loadingCane.isAbleToHarvest = false;
        loadingCane.isPlantingCane = false;

        gameCore.holdingCaneKg += (int)loadingCane.CaneKg;

        //reset cane data
        loadingCane.warmCount = 0;
        loadingCane.CaneKg = 0;
    }

    public void loadingCaneAndCaneUnitSync()
    {
    }

    public float CalculateWater()
    {
        float waterPercentage = Mathf.Clamp01(loadingCane.nowWater / 100); 
        return waterPercentage;
    }

    public float CalculateTimeProgress()
    {
        DateTime endTime = loadingCane.plantTime;
        endTime.Add(GameCore.CaneGrowTime);

        if (endTime.Add(GameCore.CaneGrowTime) <= loadingCane.plantTime)
            return 1f; // 防呆，如果結束時間比開始時間還早，直接回傳 100%

        double totalDuration = (endTime.Add(GameCore.CaneGrowTime) - loadingCane.plantTime).TotalSeconds;
        double elapsedDuration = (DateTime.Now - loadingCane.plantTime).TotalSeconds;

        double progress = elapsedDuration / totalDuration;

        loadingCane.leftTimeSpan = endTime.Add(GameCore.CaneGrowTime) - DateTime.Now;
        
        CaneBasicCheck();

        return Mathf.Clamp01((float)progress); // 限制在 0~1之間
    }
}
[Serializable]
public class Cane
{
    public float level;//甘蔗等級 刪除機制
    public float CaneKg; //甘蔗重量，隨時間增加而增加，因蟲子而減少
    public int bugNumber = 0;
    public bool isPlantingCane = false;
    public bool isAbleToHarvest = false;

    public float nowWater = 0;

    public string plantTimeString; // 改用 string 儲存時間 (ISO 8601格式)
    public DateTime plantTime = new DateTime();
    public TimeSpan leftTimeSpan = new TimeSpan();

    public string muckTimeString;
    public DateTime muckUseTime = new DateTime();
    public bool isUsingMuck = false;
    public int muckType = 0;

    public int warmCount = 0;

    public float realKgGrowth = 0f;

    public float kgGrowthPerMin = 0.2f;
    public float waterCostPerMin = 0.05f;

    public float waterYield = 0.03f;

    public float warmSpawnChance = 50f; // ( 1/number)
    public float warmYield = 0.03f;


    public void calKgGrowth()
    {
        realKgGrowth = 0;

        realKgGrowth += kgGrowthPerMin;

        if (nowWater > 80 || nowWater < 20)
        {
            float dist = 0;
            if (nowWater > 80)
            {
                dist = nowWater - 80;
            }
            else
            {
                dist = 20 - nowWater;
            }
            realKgGrowth -= waterYield * dist;
            //計算水處罰
        }

        realKgGrowth -= warmCount * warmYield;
    }









    public void transformDateTimeToJsonElement()
    {
        plantTimeString = plantTime.ToString("o"); // "o" 是 ISO 8601 格式 ("2024-05-07T13:45:30.1234567Z")
        //Debug.Log("存入解序列 - plantTimeString: " + plantTimeString);
    }
    public void transformJsonElementDateTime()
    {
        //Debug.Log("解序列 - plantTimeString: " +plantTimeString);
        if (!string.IsNullOrEmpty(plantTimeString))
        {
            plantTime = DateTime.Parse(plantTimeString);
        }
        else
        {
            plantTime = DateTime.Now; // 若沒資料就取現在時間
        }
    }
    public void calculatePassTime()
    {
        DateTime nowTime = DateTime.Now;
        TimeSpan passTime = nowTime - plantTime;
        Debug.Log($"經過了 {passTime.TotalSeconds} 秒");
        // 你可以在這裡根據 passTime.TotalSeconds 增加 CaneKg 或做其他邏輯
    }
}
