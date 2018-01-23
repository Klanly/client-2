using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Alert
{
    private static GameObject content;
    private static GameObject mask;
    private static Text text;
    private static TimerInfo timerInfo;
    private static int index;
    private static string msgContent;
    public static void Init(GameObject container)
    {
        content = container;
        mask = content.transform.Find("mask").gameObject;
        text = content.transform.Find("Text").GetComponent<Text>();
        content.SetActive(false);
    }

    public static void Show(string msg,int hideTime  = 0)
    {
        msgContent = msg; 
        if (content != null)
        {
            content.SetActive(true);
        }
        if (hideTime <= 0)
        {
            text.text = msgContent;
        }
        else
        {
            index = hideTime;
            text.text = msgContent + " " + index;
            timerInfo = TimerManager.AddDelayHandler(OnDelHandler, 1f, (uint)hideTime+1);
        }
    }
    
    private static void OnDelHandler(float del)
    {
        index--;
        if (index > 0)
        {
            text.text = msgContent + " " + index;
        }
        else
        {
            Hide();
        }
    }

    public static void Hide()
    {
        if (content != null)
        {
            content.SetActive(false);
        }
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
    }

}
