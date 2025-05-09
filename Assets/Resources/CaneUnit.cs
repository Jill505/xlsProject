using JetBrains.Annotations;
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
    void Update()
    {
        DateTime endTime = loadingCane.plantTime;
        endTime.Add(GameCore.CaneGrowTime);

        SyncCaneData();
        //sync the rate to the image
        
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

    public float warmSpawnChance = 50f; // ( 1/number)
    public float warmYield = 0.03f;
    public void calKgGrowth()
    {

    }


    public void transformDateTimeToJsonElement()
    {
        plantTimeString = plantTime.ToString("o"); // "o" �O ISO 8601 �榡 ("2024-05-07T13:45:30.1234567Z")
    }
    public void transformJsonElementDateTime()
    {
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
