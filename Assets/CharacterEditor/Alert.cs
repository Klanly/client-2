using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Alert
{
    private static GameObject content;
    private static GameObject mask;
    private static Text text;
    public static void Init(GameObject container)
    {
        content = container;
        mask = content.transform.FindChild("mask").gameObject;
        text = content.transform.FindChild("Text").GetComponent<Text>();
        content.SetActive(false);
    }

    public static void Show(string msg)
    {
        if (content != null)
        {
            content.SetActive(true);
        }
        text.text = msg;
    }

    public static void Hide()
    {
        if (content != null)
        {
            content.SetActive(false);
        }
    }

}
