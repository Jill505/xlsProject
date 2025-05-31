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
        {//防止場景切回來二次計算
            login = false;
            if (!PlayerPrefs.HasKey("LastLoginTime"))
            {
                //初始化
                Debug.Log("首次遊玩本遊戲");
                savedTime_LastTimeLogin = DateTime.Now;
                Debug.Log("實現時間重置");
                //儲存現在時間
                ST = SaveTime.giveTime(DateTime.Now);
                PlayerPrefs.SetString("LastLoginTime", JsonUtility.ToJson(ST));//序列化
                PlayerPrefs.Save();
            }
            else
            {
                //取得上次時間
                SaveTime STR = JsonUtility.FromJson<SaveTime>(PlayerPrefs.GetString("LastLoginTime"));//反序列化
                savedTime_LastTimeLogin = SaveTime.giveDate(STR);
                //儲存現在時間
                ST = SaveTime.giveTime(DateTime.Now);
                PlayerPrefs.SetString("LastLoginTime", JsonUtility.ToJson(ST));//序列化
                PlayerPrefs.Save();
            }

            //syncer.text = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            Debug.Log("這次登陸：" + DateTime.Now + "\n" + "上次登陸：" + savedTime_LastTimeLogin);
            //計算時間差
            TimeSpan TS = DateTime.Now - savedTime_LastTimeLogin;
            lobby_entry.lastPastTime = (int)TS.TotalSeconds;

            //弄個提示字
            if (lastLogTimeShowcase != null)
            {
                if (TS.Days > 0)
                {
                    //lastLogTimeShowcase.text = "歡迎回來\n距離上次登陸已經過了\n" + TS.Days + "天" + TS.Hours + "小時 " + TS.Minutes + "分鐘 " + TS.Seconds + "秒";
                }
                else if (TS.Hours > 0)
                {
                    //lastLogTimeShowcase.text = "歡迎回來\n距離上次登陸已經過了\n" + TS.Hours + "小時 " + TS.Minutes + "分鐘 " + TS.Seconds + "秒";
                }
                else
                {
                    //lastLogTimeShowcase.text = "歡迎回來\n距離上次登陸已經過了\n" + TS.Minutes + "分鐘 " + TS.Seconds + "秒";
                }
                Destroy(lastLogTimeShowcase.gameObject, 10f);
            }
        }
        else
        {
            //切換視窗 or 切換場景時觸發
        }
    }

    void Update()
    {
         //同步現在時間
        //showNowTime.text = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        //儲存上次時間 如影響校能應優先考慮刪除
        SaveTime ST = new SaveTime();
        ST = SaveTime.giveTime(DateTime.Now);
        PlayerPrefs.SetString("LastLoginTime", JsonUtility.ToJson(ST));//序列化
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

