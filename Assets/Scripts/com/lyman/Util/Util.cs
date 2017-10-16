using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Util
{


    public static void HideAllChild(Transform owner)
    {
        int imax = owner.childCount;
        for (int i = 0; i < imax; ++i)
        {
            owner.GetChild(i).gameObject.SetActive(false);
        }
    }
    public static void ShowAllChild(Transform owner)
    {
        int imax = owner.childCount;
        for (int i = 0; i < imax; ++i)
        {
            owner.GetChild(i).gameObject.SetActive(true);
        }
    }

    public static Transform FindInChildByName(Transform parent, string childName)
    {
        if (childName == null)
            return null;
        Transform child = parent.FindChild(childName);
        if (child == null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                child = FindInChildByName(parent.GetChild(i), childName);
                if (child != null)
                {
                    break;
                }
            }
        }
        return child;
    }

    public static void SetParent(Transform mySelf, Transform parent)
    {
        mySelf.SetParent(parent);
        mySelf.localPosition = Vector3.zero;
        mySelf.localRotation = Quaternion.identity;
        mySelf.localScale = Vector3.one;
    }

    public static void SetUIParent(Transform mySelf, Transform parent)
    {
        mySelf.SetParent(parent);
        RectTransform rectTransform = mySelf.gameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.localScale = Vector3.one;
        }
    }



    

}
