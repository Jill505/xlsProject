using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class lobby_entry : MonoBehaviour
{
    public static int lastPastTime;
    static bool login = true;
    public Text nowTimeShowcase;
    public Text lastLogTimeShowcase;

    public DateTime savedTime_LastTimeLogin;


    void Awake()
    {
        SaveTime ST = new SaveTime();
        if (login == true)
        {//����������^�ӤG���p��
            login = false;
            if (!PlayerPrefs.HasKey("LastLoginTime"))
            {
                //��l��
                Debug.Log("�����C�����C��");
                savedTime_LastTimeLogin = DateTime.Now;
                Debug.Log("��{�ɶ����m");
                //�x�s�{�b�ɶ�
                ST = SaveTime.giveTime(DateTime.Now);
                PlayerPrefs.SetString("LastLoginTime", JsonUtility.ToJson(ST));//�ǦC��
                PlayerPrefs.Save();
            }
            else
            {
                //���o�W���ɶ�
                SaveTime STR = JsonUtility.FromJson<SaveTime>(PlayerPrefs.GetString("LastLoginTime"));//�ϧǦC��
                savedTime_LastTimeLogin = SaveTime.giveDate(STR);
                //�x�s�{�b�ɶ�
                ST = SaveTime.giveTime(DateTime.Now);
                PlayerPrefs.SetString("LastLoginTime", JsonUtility.ToJson(ST));//�ǦC��
                PlayerPrefs.Save();
            }

            //syncer.text = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            Debug.Log("�o���n���G" + DateTime.Now + "\n" + "�W���n���G" + savedTime_LastTimeLogin);
            //�p��ɶ��t
            TimeSpan TS = DateTime.Now - savedTime_LastTimeLogin;
            lobby_entry.lastPastTime = (int)TS.TotalSeconds;

            //�˭Ӵ��ܦr
            if (lastLogTimeShowcase != null)
            {
                if (TS.Days > 0)
                {
                    //lastLogTimeShowcase.text = "�w��^��\n�Z���W���n���w�g�L�F\n" + TS.Days + "��" + TS.Hours + "�p�� " + TS.Minutes + "���� " + TS.Seconds + "��";
                }
                else if (TS.Hours > 0)
                {
                    //lastLogTimeShowcase.text = "�w��^��\n�Z���W���n���w�g�L�F\n" + TS.Hours + "�p�� " + TS.Minutes + "���� " + TS.Seconds + "��";
                }
                else
                {
                    //lastLogTimeShowcase.text = "�w��^��\n�Z���W���n���w�g�L�F\n" + TS.Minutes + "���� " + TS.Seconds + "��";
                }
                Destroy(lastLogTimeShowcase.gameObject, 10f);
            }
        }
        else
        {
            //�������� or ����������Ĳ�o
        }
    }

    void Update()
    {
         //�P�B�{�b�ɶ�
        //showNowTime.text = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        //�x�s�W���ɶ� �p�v�T�կ����u���Ҽ{�R��
        SaveTime ST = new SaveTime();
        ST = SaveTime.giveTime(DateTime.Now);
        PlayerPrefs.SetString("LastLoginTime", JsonUtility.ToJson(ST));//�ǦC��
        PlayerPrefs.Save();
    }
}
[Serializable]
public class SaveTime
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;

    static public SaveTime giveTime(int year, int month, int day, int hour, int minute, int second)
    {
        SaveTime swapTime = new SaveTime();
        swapTime.year = year;
        swapTime.month = month;
        swapTime.day = day;
        swapTime.hour = hour;
        swapTime.minute = minute;
        swapTime.second = second;

        return swapTime;
    }
    static public SaveTime giveTime(DateTime dt)
    {
        SaveTime swapTime = new SaveTime();
        swapTime.year = dt.Year;
        swapTime.month = dt.Month;
        swapTime.day = dt.Day;
        swapTime.hour = dt.Hour;
        swapTime.minute = dt.Minute;
        swapTime.second = dt.Second;

        return swapTime;
    }

    static public DateTime giveDate(SaveTime saveTime)
    {
        DateTime dt = new DateTime(saveTime.year, saveTime.month, saveTime.day, saveTime.hour, saveTime.minute, saveTime.second);
        return dt;
    }
}

[Serializable]
public class LogData
{
    public int loginTimes;
}

