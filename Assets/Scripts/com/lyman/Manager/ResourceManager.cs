using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
/// <summary>
/// 資源管理
/// </summary>
public class ResourceManager
{
    private static ABManager abManager;
    public static void Init()
    {
        if (abManager == null)
        {
            abManager = new ABManager();
            abManager.Initialize();
        }
    }

    //同步创建，建议不用，或者比较小得assetbundle（比如50K内的）
    public static void CreateAssetBundle(string abPath)
    {
        abManager.LoadAssetBundle(abPath);
    }

    
    //异步创建,建议优先使用
    public static void CreateAssetBundleAsync(string abPath, Action onCompleteHandler)
    {
        abManager.LoadAssetBundleAsync(abPath, onCompleteHandler);
    }


    public static GameObject GetGameObjectInstance(string abPath, string assetName)
    {
        if (abManager != null)
        {
           GameObject gameObject = abManager.LoadAsset(abPath, assetName);
            if (gameObject != null)
            {
                return GameObject.Instantiate(gameObject);
            }
        }
        return null;
    }

    public static GameObject GetGameObjectPrefab(string abPath, string assetName)
    {
        if (abManager != null)
        {
            GameObject gameObject = abManager.LoadAsset(abPath, assetName);
            return gameObject;
        }
        return null;
    }
    
    public static string GetText(string abPath, string assetName)
    {
        if (abManager != null)
        {
            TextAsset textAsset = abManager.LoadTextAsset(abPath, assetName);
            if(textAsset != null)
                return textAsset.text;
        }
        return string.Empty;
    }



    private static Dictionary<string, Font> myFonts = new Dictionary<string, Font>();
    public static Font GetFont(string abPath, string assetName)
    {
        string key = abPath + "_" + assetName;
        Font font = null;
        myFonts.TryGetValue(key, out font);
        if (font == null && abManager != null)
        {
            font = abManager.LoadFontAsset(abPath, assetName);
            if (font != null)
            {
                myFonts.Add(key, font);
            }
        }
        return font;
    }


    /// <summary>
    /// 待測試
    /// </summary>
    /// <param n="abPath"></param>
    /// <param n="assetn"></param>
    /// <returns></returns>
    public static Sprite GetSprite(string abPath, string assetName)
    {
        if (abManager != null)
        {
            Sprite sprite = abManager.LoadSpriteAsset(abPath, assetName);
            return sprite;
        }
        return null;
    }


    private static Dictionary<string, AudioClip> myAudioClips = new Dictionary<string, AudioClip>(); 
    public static AudioClip GetAudioClip(string abPath, string assetName)
    {
        string key = abPath + "_" + assetName;
        AudioClip audioClip = null;
        myAudioClips.TryGetValue(key, out audioClip);
        if (audioClip == null && abManager != null)
        {
            audioClip = abManager.LoadAudioClipAsset(abPath, assetName);
            if (audioClip != null)
            {
                myAudioClips.Add(key, audioClip);
            }
        }
        return audioClip;
    }
}
