using JetBrains.Annotations;
using System;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;

public class GameCore : MonoBehaviour
{
    public lobby_entry game_entry;
    [Header("Cane System")]
    public CaneUnit[] caneUnits;

    public int money;//日幣 yen
    public int holdingCaneKg;

    public bool unlockCaneFec;
    public bool unlockStore;

    public int holdingManureNumber = 0;
    public int holdingPesticideNumber = 0;
    public int holdingMoisturizerNumber = 0;

    public Cane CaneA;
    public Cane CaneB;
    public Cane CaneC;
    public Cane CaneD;


    //靜態宣告
    [Header("Static public data")]
    public static TimeSpan CaneGrowTime = new TimeSpan(8, 0, 0);//12小時
    public static SaveFile saveFile = new SaveFile();
    public Sprite[] caneStateSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        CaneGrowTime = new TimeSpan(8, 0, 0);
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("SaveFile"))
        {
            Debug.Log("建立新存檔");
            //saveFile = new SaveFile();
            saveFile.DataInitialize();
            saveFile.SaveSaveFile(saveFile);
        }
        //load conditions
        LoadWeather();
        //load saveFile
        loadSaveFile();
        //解包
        unlockSaveFilePack();
    }

    void Update()
    {
        SaveFile();
        CalculateProtection(CaneA);
        CalculateProtection(CaneB);
        CalculateProtection(CaneC);
        CalculateProtection(CaneD);

        GameGoing();
    }

    public void CalculateProtection(Cane theCane)
    {
        if (theCane.nowWater > 100)
        {
            theCane.nowWater = 100;
        }
        else if (theCane.nowWater < 0)
        {
            theCane.nowWater = 0;
        }
    }
    /// <summary>
    /// ㄐㄐ
    /// </summary>
    public void LoadWeather()
    {
        if (GetWeather.nowWeather == "Sun")
        {

        }
        else if(GetWeather.nowWeather == "Rain")
        {

        }
        else //Cloud
        {

        }
    }
    
    public void GameGoing()
    {

    }

    public void SaveFile()
    {
        syncLocalDataToStaticData();
        saveFile.SaveSaveFile(saveFile);
    }

    public void syncLocalDataToStaticData()
    {
        saveFile.lastLogInTime = DateTime.Now;
        saveFile.transformDateTimeToJsonElement();

        saveFile.money = money;
        saveFile.holdingCaneKg = holdingCaneKg;

        saveFile.unlockCaneFec = unlockCaneFec;
        saveFile.unlockStore = unlockStore;

        saveFile.holdingManureNumber = holdingManureNumber;
        saveFile.holdingPesticideNumber = holdingPesticideNumber;
        saveFile.holdingMoisturizerNumber = holdingMoisturizerNumber;

        saveFile.caneA =  caneUnits[0].loadingCane;
        saveFile.caneA = CaneA;
        saveFile.caneB = caneUnits[1].loadingCane;
        saveFile.caneB = CaneB;
        saveFile.caneC = caneUnits[2].loadingCane;
        saveFile.caneC = CaneC;
        saveFile.caneD = caneUnits[3].loadingCane;
        saveFile.caneD = CaneD;
    }

    public void loadSaveFile()
    {
        saveFile.LoadSaveFile();
        Debug.Log("存檔資料：");
        Debug.Log(JsonUtility.ToJson(saveFile));

        //update timespan data;
    }

    public void unlockSaveFilePack()
    {
        money = saveFile.money;
        holdingCaneKg = saveFile.holdingCaneKg;

        unlockCaneFec = saveFile.unlockCaneFec;
        unlockStore = saveFile.unlockStore;

        holdingManureNumber = saveFile.holdingManureNumber;
        holdingPesticideNumber = saveFile.holdingPesticideNumber;
        holdingMoisturizerNumber = saveFile.holdingMoisturizerNumber;

        

        CaneA = saveFile.caneA;
        caneUnits[0].loadingCane = CaneA;
        CaneB = saveFile.caneB;
        caneUnits[1].loadingCane = CaneB;
        CaneC = saveFile.caneC;
        caneUnits[2].loadingCane = CaneC;
        CaneD = saveFile.caneD;
        caneUnits[3].loadingCane = CaneD;
    }
}


[Serializable]
public class SaveFile
{
    //last log in DateTime
    public DateTime lastLogInTime;
    public string lastLogInTimeStr;
    public TimeSpan timeSpanNotOnLine;

    public void transformDateTimeToJsonElement()
    {
        lastLogInTimeStr = lastLogInTime.ToString("o"); // "o" 是 ISO 8601 格式 ("2024-05-07T13:45:30.1234567Z")
    }
    public void transformJsonElementDateTime()
    {
        if (!string.IsNullOrEmpty(lastLogInTimeStr))
        {
            lastLogInTime = DateTime.Parse(lastLogInTimeStr);
        }
        else
        {
            lastLogInTime = DateTime.Now; // 若沒資料就取現在時間
        }
    }


    public int money;//日幣 yen
    public int holdingCaneKg;

    public bool unlockCaneFec;
    public bool unlockStore;

    public int holdingManureNumber = 0;
    public int holdingPesticideNumber = 0;
    public int holdingMoisturizerNumber = 0;

    public Cane caneA = new Cane();
    public Cane caneB = new Cane();
    public Cane caneC = new Cane();
    public Cane caneD = new Cane();

    //Cane Conditions
    public void DataInitialize()
    {
        money = 100;
        holdingCaneKg = 0;

        unlockCaneFec = false;
        unlockStore = false;

        holdingManureNumber = 0;
        holdingPesticideNumber =0;
        holdingMoisturizerNumber = 0;
    }


    public void LoadSaveFile()
    {
        Debug.Log("載入存檔中...");
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(PlayerPrefs.GetString("SaveFile"));
        Debug.Log("成功解序列");

        transformJsonElementDateTime();
        saveFile.caneA.transformJsonElementDateTime();
        saveFile.caneB.transformJsonElementDateTime();
        saveFile.caneC.transformJsonElementDateTime();
        saveFile.caneD.transformJsonElementDateTime();
        Debug.Log("甘蔗轉換");
        //解包
        GameCore.saveFile = saveFile;
        caneA = saveFile.caneA;
        caneB = saveFile.caneB;
        caneC = saveFile.caneC;
        caneD = saveFile.caneD;
        Debug.Log("載入存檔");
        //存檔載入完成

        //計算與前次登入的時間差
        //timeSpanNotOnLine = DateTime.Now - lastLogInTime;

        lobby_entry game_entry = GameObject.Find("GameCore").GetComponent<lobby_entry>();
        timeSpanNotOnLine = DateTime.Now - game_entry.savedTime_LastTimeLogin;

        Debug.Log("timeSpanNotOnLine：" + timeSpanNotOnLine.TotalMinutes);
        Debug.Log("現在分鐘數：" + DateTime.Now);
        Debug.Log("lastLogInTime：" + lastLogInTime);
        //拿這時間差去做相關計算補正

        CalNotOnline(caneA);
        CalNotOnline(caneB);
        CalNotOnline(caneC);
        CalNotOnline(caneD);
    }
    public void SaveSaveFile(SaveFile saveFile)
    {
        //transformDateTimeToJsonElement();

        caneA = GameObject.Find("GameCore").GetComponent<GameCore>().CaneA;
        caneA.transformDateTimeToJsonElement();
        caneB = GameObject.Find("GameCore").GetComponent<GameCore>().CaneB;
        caneB.transformDateTimeToJsonElement();
        caneC = GameObject.Find("GameCore").GetComponent<GameCore>().CaneC;
        caneC.transformDateTimeToJsonElement();
        caneD = GameObject.Find("GameCore").GetComponent<GameCore>().CaneD;
        caneD.transformDateTimeToJsonElement();

        
        lastLogInTime = DateTime.Now;
        transformDateTimeToJsonElement();
        string str = JsonUtility.ToJson(saveFile);
        PlayerPrefs.SetString("SaveFile", str);
        PlayerPrefs.Save();
        //Debug.Log("成功儲存" + str);
    }

    public void CalNotOnline(Cane theCane)
    {
        if (theCane.isPlantingCane)
        {
            if (theCane.plantTime.Add(GameCore.CaneGrowTime) - DateTime.Now <= TimeSpan.Zero)
            {
                //Fin the process
                Debug.Log(theCane + "結束");
            }
            else
            {
                //計算理想生長量
                double totalMinutesOffline = timeSpanNotOnLine.TotalMinutes;
                Debug.Log(totalMinutesOffline + "經過分鐘數");
                double baseGrowth = totalMinutesOffline * 0.2; // 0.2 kg / min
                                                               //計算蟲害產生
                double growthPenaltyPerMinute = 0.0;

                // warm 數量
                for (int i = 0; i < totalMinutesOffline; i += 5)
                {
                    if (UnityEngine.Random.Range(0, 120 + GetWeather.nowTemp * 3) == 0)
                    {
                        theCane.warmCount += 1;
                    }
                }

                growthPenaltyPerMinute += theCane.warmCount * 0.05;


                for (int i = 0; i < totalMinutesOffline; i += 15)
                {
                    theCane.nowWater -= 3;

                    // 濕度影響
                    if (theCane.nowWater < 20)
                    {
                        double deficit = 20 - theCane.nowWater;
                        growthPenaltyPerMinute += (deficit / 2) * 0.03;
                    }
                    else if (theCane.nowWater > 80)
                    {
                        double excess = theCane.nowWater - 80;
                        growthPenaltyPerMinute += (excess / 2) * 0.03;
                    }
                }
                // 最後實際成長量
                double actualGrowth = baseGrowth + ( - growthPenaltyPerMinute);

                // 保護，不要小於0
                if (actualGrowth < 0) actualGrowth = 0;
                //計算水份流失

                theCane.CaneKg += (float)actualGrowth;
                Debug.Log(theCane + "玩家不在的期間生長了 " + actualGrowth);
            }
        }
    }
}