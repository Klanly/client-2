using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class TimerTypes
{
    public const int EndlessLoop = 1;
    public const int Delay = 2;
}

public class TimerInfo
{
    private Action<float> delayHandler;
    private int type = TimerTypes.EndlessLoop;
    
    private uint repeatTimes;
    private float delayTime;
    private bool deleteFlag;
    private float currentDelay;
    private int currentTimes;
    private bool pause;

    public TimerInfo()
    {
        currentDelay = 0f;
        currentTimes = 0;
        deleteFlag = false;
    }
    

    public Action<float> DelayHandler
    {
        get { return delayHandler; }
        set
        {
            currentTimes = 0;
            currentDelay = 0f;
            delayHandler = value;
        }
    }

    public int Type
    {
        get { return type; }
        set { type = value; }
    }

   

    public uint RepeatTimes
    {
        get { return repeatTimes; }
        set
        {
            repeatTimes = value;
            if (repeatTimes < 1)
            {
                repeatTimes = 1;
            }
        }
    }
    
    public float DelayTime
    {
        set { delayTime = value; }
        get { return delayTime; }
    }

    public bool DeleteFlag
    {
        set { deleteFlag = value; }
        get { return deleteFlag; }
    }

    public bool Pause
    {
        set { pause = value; }
        get { return pause; }
    }


    public void Update(float delayTime)
    {
        if (Pause || DeleteFlag) return;
        if (type == TimerTypes.EndlessLoop)
        {
            if (delayHandler != null)
                delayHandler(delayTime);
        }
        else if (type == TimerTypes.Delay)
        {
            currentDelay += delayTime;
            int cTimer = (int)(currentDelay / DelayTime);
            int eTimer = cTimer - currentTimes;
            if (eTimer > 0)
            {
                float tDel = (currentDelay - currentTimes * DelayTime) / eTimer;
                for (int i = 0; i < eTimer; ++i)
                {
                    if (delayHandler != null)
                        delayHandler(tDel);
                }
                currentTimes = cTimer;
                if (currentTimes >= RepeatTimes)
                {
                    DeleteFlag = true;
                }
            }
        }
    }
    public void Clear()
    {
        DelayHandler = null;
        DeleteFlag = true;
    }
}

public class TimerManager
{
    
    private static List<TimerInfo> runningTimerInfoList = new List<TimerInfo>();
    private static List<TimerInfo> noRunTimerInfoList = new List<TimerInfo>();

    public static TimerInfo AddHandler(Action<float> delayHandler)
    {
        if (delayHandler != null)
        {
            TimerInfo timerInfo = GetTimerInfo();
            timerInfo.Type = TimerTypes.EndlessLoop;
            timerInfo.DelayHandler = delayHandler;
            timerInfo.DeleteFlag = false;
            runningTimerInfoList.Add(timerInfo);
            return timerInfo;
        }
        return null;
    }


    private static TimerInfo GetTimerInfo()
    {
        TimerInfo timerInfo = null;
        int length = noRunTimerInfoList.Count;
        if (length > 0)
        {
            timerInfo = noRunTimerInfoList[length - 1];
            noRunTimerInfoList.Remove(timerInfo);
        }
        if (timerInfo == null)
        {
            timerInfo = new TimerInfo();
        }
        return timerInfo;
    }

    public static TimerInfo AddDelayHandler(Action<float> delayHandler, float delaytime, uint repeattimes)
    {
        if (delayHandler != null)
        {
            TimerInfo timerInfo = GetTimerInfo();
            timerInfo.Type = TimerTypes.Delay;
            timerInfo.RepeatTimes = repeattimes;
            timerInfo.DelayHandler = delayHandler;
            timerInfo.DelayTime = delaytime;
            timerInfo.DeleteFlag = false;
            runningTimerInfoList.Add(timerInfo);
            return timerInfo;
        }
        return null;
    }


    public static void RemoveHandler(TimerInfo timerInfo)
    {
        if (timerInfo != null)
        {
            timerInfo.Clear();
        }   
    }

    public static int TimerInfoCount
    {
        get { return runningTimerInfoList.Count; }
    }
    
    public static void Update(float delayTime)
    {
        int count = runningTimerInfoList.Count;
        TimerInfo tInfo;
        int i = 0;
        for (i = 0; i < count; ++i)
        {
            tInfo = runningTimerInfoList[i];
            if (tInfo == null || tInfo.DeleteFlag || tInfo.Pause) continue;
            tInfo.Update(delayTime);
        }
        for (i = count - 1; i >= 0; i--)
        {
            tInfo = runningTimerInfoList[i];
            if (tInfo != null && tInfo.DeleteFlag)
            {
                runningTimerInfoList.Remove(tInfo);
                tInfo.Clear();
                noRunTimerInfoList.Add(tInfo);
            }
        }
    }   
}