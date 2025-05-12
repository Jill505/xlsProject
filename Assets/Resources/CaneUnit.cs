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

    [Tooltip("�o�Ӭ�true���ɭԾ�Ӷ}�l�]")]
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
        //�P�_�O�_���b�ͪ����A��
        if (!loadingCane.isPlantingCane || loadingCane.isAbleToHarvest)
        {
            // �Y���شөΤw�g�����A���ݭn�p��
            Debug.Log(loadingCane + " ���ݭn�p�����u�����C");
            return;
        }

        // �P�_�O�_�w�g�W�L�ͪ���
        if (loadingCane.plantTime.Add(GameCore.CaneGrowTime) <= DateTime.Now)
        {
            // �i�J�������A
            loadingCane.isAbleToHarvest = true;
            loadingCane.isPlantingCane = false;
            loadingCane.leftTimeSpan = TimeSpan.Zero;

            Debug.Log(loadingCane + " ���u�����̽��w�����A�i�H�����I");
            return;
        }

        double baseGrowth = 1 * 0.2 / 60 / 50; // ��¦���� (0.2 kg / min)
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
        // �ͪ��ɶ��w��A���|���i�J�i�������A
        if (loadingCane.leftTimeSpan <= TimeSpan.Zero && loadingCane.isAbleToHarvest == false)
        {
            // ��s���A���i����
            Debug.Log("loadingCane.leftTimeSpan: " + loadingCane.leftTimeSpan.TotalMinutes);
            Debug.Log("�ͦ�clog�Ӧ� CaneBasicCheck");
            loadingCane.leftTimeSpan = TimeSpan.Zero;
            loadingCane.isAbleToHarvest = true;
            loadingCane.isPlantingCane = false;

            //Console.WriteLine("�̽��w�����A�i�H�����F�A�����ƥ�I");
        }
        // �p�G�ثe���\����
        else if (loadingCane.isAbleToHarvest == true)
        {
            //Console.WriteLine("�̽��i�Ѧ������C");
        }
        // �٦b�ͪ���
        else if (loadingCane.isPlantingCane == true)
        {
            //Console.WriteLine($"�̽��������A�Ѿl�ɶ��G{loadingCane.leftTimeSpan.TotalMinutes} �����C");
        }
        else
        {
            // �D�w�����A�]�ҡG���شӡ^
            Console.WriteLine("�|���شӥ̽��C");
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
            return 1f; // ���b�A�p�G�����ɶ���}�l�ɶ��٦��A�����^�� 100%

        double totalDuration = (endTime.Add(GameCore.CaneGrowTime) - loadingCane.plantTime).TotalSeconds;
        double elapsedDuration = (DateTime.Now - loadingCane.plantTime).TotalSeconds;

        double progress = elapsedDuration / totalDuration;

        loadingCane.leftTimeSpan = endTime.Add(GameCore.CaneGrowTime) - DateTime.Now;
        
        CaneBasicCheck();

        return Mathf.Clamp01((float)progress); // ����b 0~1����
    }
}
[Serializable]
public class Cane
{
    public float level;//�̽����� �R������
    public float CaneKg; //�̽����q�A�H�ɶ��W�[�ӼW�[�A�]�Τl�Ӵ��
    public int bugNumber = 0;
    public bool isPlantingCane = false;
    public bool isAbleToHarvest = false;

    public float nowWater = 0;

    public string plantTimeString; // ��� string �x�s�ɶ� (ISO 8601�榡)
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
            //�p����B�@
        }

        realKgGrowth -= warmCount * warmYield;
    }









    public void transformDateTimeToJsonElement()
    {
        plantTimeString = plantTime.ToString("o"); // "o" �O ISO 8601 �榡 ("2024-05-07T13:45:30.1234567Z")
        //Debug.Log("�s�J�ѧǦC - plantTimeString: " + plantTimeString);
    }
    public void transformJsonElementDateTime()
    {
        //Debug.Log("�ѧǦC - plantTimeString: " +plantTimeString);
        if (!string.IsNullOrEmpty(plantTimeString))
        {
            plantTime = DateTime.Parse(plantTimeString);
        }
        else
        {
            plantTime = DateTime.Now; // �Y�S��ƴN���{�b�ɶ�
        }
    }
    public void calculatePassTime()
    {
        DateTime nowTime = DateTime.Now;
        TimeSpan passTime = nowTime - plantTime;
        Debug.Log($"�g�L�F {passTime.TotalSeconds} ��");
        // �A�i�H�b�o�̮ھ� passTime.TotalSeconds �W�[ CaneKg �ΰ���L�޿�
    }
}
