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
    public WarmManager warmManager;
    public UiCore uiCore;

    [Header("Cane System")]
    public CaneUnit[] caneUnits;

    public int money;//��� yen
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

    //�R�A�ŧi
    [Header("Static public data")] 
    public static TimeSpan CaneGrowTime = new TimeSpan(8, 0, 0);//12�p��
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
            Debug.Log("�إ߷s�s��");
            //saveFile = new SaveFile();
            saveFile.DataInitialize();
            saveFile.SaveSaveFile(saveFile);
        }





        //load conditions
        LoadWeather();
        //load saveFile
        loadSaveFile();
        //�ѥ]
        unlockSaveFilePack();
    }

    void Update()
    {
        SaveFile();
        CalculateProtection(CaneA);
        CalculateProtection(CaneB);
        CalculateProtection(CaneC);
        CalculateProtection(CaneD);


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
    /// ����
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
        Debug.Log("�s�ɸ�ơG");
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
        lastLogInTimeStr = lastLogInTime.ToString("o"); // "o" �O ISO 8601 �榡 ("2024-05-07T13:45:30.1234567Z")
    }
    public void transformJsonElementDateTime()
    {
        if (!string.IsNullOrEmpty(lastLogInTimeStr))
        {
            lastLogInTime = DateTime.Parse(lastLogInTimeStr);
        }
        else
        {
            lastLogInTime = DateTime.Now; // �Y�S��ƴN���{�b�ɶ�
        }
    }


    public int money;//��� yen
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
        Debug.Log("���J�s�ɤ�...");
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(PlayerPrefs.GetString("SaveFile"));
        Debug.Log("���\�ѧǦC");

        transformJsonElementDateTime();
        saveFile.caneA.transformJsonElementDateTime();
        saveFile.caneB.transformJsonElementDateTime();
        saveFile.caneC.transformJsonElementDateTime();
        saveFile.caneD.transformJsonElementDateTime();
        Debug.Log("�̽��ഫ");
        //�ѥ]
        GameCore.saveFile = saveFile;
        caneA = saveFile.caneA;
        caneB = saveFile.caneB;
        caneC = saveFile.caneC;
        caneD = saveFile.caneD;
        Debug.Log("���J�s��");
        //�s�ɸ��J����

        //�p��P�e���n�J���ɶ��t
        //timeSpanNotOnLine = DateTime.Now - lastLogInTime;

        lobby_entry game_entry = GameObject.Find("GameCore").GetComponent<lobby_entry>();
        timeSpanNotOnLine = DateTime.Now - game_entry.savedTime_LastTimeLogin;
        Debug.Log("game_entry.savedTime_LastTimeLogin: " + game_entry.savedTime_LastTimeLogin);
        Debug.Log("timeSpanNotOnLine: "+ timeSpanNotOnLine);

        Debug.Log("timeSpanNotOnLine�G" + timeSpanNotOnLine.TotalMinutes);
        Debug.Log("�{�b�����ơG" + DateTime.Now);
        Debug.Log("lastLogInTime�G" + lastLogInTime);
        //���o�ɶ��t�h�������p��ɥ�

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
        //Debug.Log("���\�x�s" + str);
    }

    public void CalNotOnline(Cane theCane)
    {
        if (!theCane.isPlantingCane || theCane.isAbleToHarvest)
        {
            // �Y���شөΤw�g�����A���ݭn�p��
            Debug.Log(theCane + " ���ݭn�p�����u�����C");
            return;
        }

        // �P�_�O�_�w�g�W�L�ͪ���
        if (theCane.plantTime.Add(GameCore.CaneGrowTime) <= DateTime.Now)
        {
            // �i�J�������A
            theCane.isAbleToHarvest = true;
            theCane.isPlantingCane = false;
            theCane.leftTimeSpan = TimeSpan.Zero;

            Debug.Log(theCane + " ���u�����̽��w�����A�i�H�����I");
            return;
        }

        // �H�U�����u�����ͪ��޿�
        double totalMinutesOffline = timeSpanNotOnLine.TotalMinutes;
        Debug.Log(totalMinutesOffline + " �������u");

        double baseGrowth = totalMinutesOffline * 0.2; // ��¦���� (0.2 kg / min)
        double growthPenaltyPerMinute = 0.0;

        // --- �ή`���� ---
        int warmCT = 0;
        int ctConst = 1;
        if (GetWeather.nowWeather == "Sun")
        {
            ctConst = 2;
        }
        for (int i = 0; i < totalMinutesOffline; i += 1)
        {
            if (UnityEngine.Random.Range(0, 240) <= (GetWeather.nowTemp /3 * ctConst))
            {
                theCane.warmCount += 1;
                warmCT += 1;
            }
        }
        growthPenaltyPerMinute += theCane.warmCount * 0.05;

        // --- �����y���P����g�@ ---
        float waterCT = 0;
        float waterSunDec = 1;
        if (GetWeather.nowWeather == "Sun")
        {
            waterSunDec = 1.4f;
        }
        for (int i = 0; i < totalMinutesOffline; i += 15)
        {
            float randomWN = UnityEngine.Random.Range(0f, Mathf.Clamp01(0.6f + (GetWeather.nowTemp * 0.1f)));
            randomWN *= waterSunDec;

            if (GetWeather.nowWeather == "Rain")
            {
                theCane.nowWater += (GetWeather.nowHumidity - GetWeather.nowTemp) /100;
            }
            else
            {
                theCane.nowWater -= randomWN;
                waterCT += randomWN;
            }

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

        // --- �p���ڦ��� ---
        double actualGrowth = baseGrowth - growthPenaltyPerMinute;
        actualGrowth = Math.Max(0, actualGrowth / 5);  // �קK�t�ƨå��żƭ�

        theCane.CaneKg += (float)actualGrowth;

        Debug.Log($"{theCane} ���a���b�u�����ͪ��F {actualGrowth:F2} kg �̽��C");
        Debug.Log($"{theCane} ���a���b�u�����]�o�F {waterCT:F2}% �����C");
        Debug.Log($"{theCane} ���a���b�u�������X�F {warmCT} ���ΡC");
    }
}