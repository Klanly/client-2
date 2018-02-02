

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.UI;
using System;



public class ABManager
{
    private string[] variants = { };
    private AssetBundleManifest manifest;
    private AssetBundle assetbundle;
    private Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {   
        string path = GameConst.DataPath + GameConst.StreamingAssets;
        assetbundle = AssetBundle.LoadFromFile(path);
        manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param n="abn"></param>
    /// <param n="assetn"></param>
    /// <returns></returns>
    public GameObject LoadAsset(string abName, string prefabName)
    {
        abName = abName.ToLower();
        AssetBundle bundle = LoadAssetBundle(abName);
        if (bundle != null)
            return bundle.LoadAsset<GameObject>(prefabName);
        
        return null;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param n="abn"></param>
    /// <param n="filen"></param>
    /// <returns></returns>
    public TextAsset LoadTextAsset(string abName, string textName)
    {
        
        abName = abName.ToLower();
        AssetBundle bundle = LoadAssetBundle(abName);
        if (bundle != null)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            TextAsset textAsset = bundle.LoadAsset<TextAsset>(textName);
            stopwatch.Stop();
            Debug.Log("LoadTextAsset:" + textName + ",用时:" + stopwatch.ElapsedMilliseconds + "毫秒");
            return textAsset;
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param n="abn"></param>
    /// <param n="filen"></param>
    /// <returns></returns>
    public Font LoadFontAsset(string abName, string fontName)
    {
        abName = abName.ToLower();
        AssetBundle bundle = LoadAssetBundle(abName);
        if (bundle != null)
            return bundle.LoadAsset<Font>(fontName);
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param n="abn"></param>
    /// <param n="filen"></param>
    /// <returns></returns>
    public Sprite LoadSpriteAsset(string abName, string spriteName)
    {
        abName = abName.ToLower();
        AssetBundle bundle = LoadAssetBundle(abName);
        if (bundle != null)
            return bundle.LoadAsset<Sprite>(spriteName);
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param n="abn"></param>
    /// <param n="filen"></param>
    /// <returns></returns>
    public AudioClip LoadAudioClipAsset(string abName, string audioClipName)
    {
        abName = abName.ToLower();
        AssetBundle bundle = LoadAssetBundle(abName);
        if (bundle != null)
            return bundle.LoadAsset<AudioClip>(audioClipName);
        return null;
    }



    public bool HaveBundle(string abName)
    {
        return bundles.ContainsKey(abName);
    }

    
    /// <summary>
    /// 载入AssetBundle
    /// </summary>
    /// <param n="abn"></param>
    /// <returns></returns>
    public AssetBundle LoadAssetBundle(string abName)
    {
        AssetBundle bundle = null;
        try
        {
            abName = abName.ToLower();
            if (!abName.EndsWith(GameConst.ABExtensionName))
            {
                abName += GameConst.ABExtensionName;
            }
            if (!bundles.ContainsKey(abName))
            {
                string uri = GameConst.DataPath + abName;
                LoadDependencies(abName);
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                bundle = AssetBundle.LoadFromFile(uri); //关联的素材绑定
                stopwatch.Stop();
                Debug.Log("创建:" + uri + ",用时:" + stopwatch.ElapsedMilliseconds + "毫秒");
                bundles.Add(abName, bundle);
            }
            else
            {
                bundles.TryGetValue(abName, out bundle);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(abName + " 不存在");
        }
        return bundle;
    }
    
    private Dictionary<string, CreateAssetBundleAsync> asyncCreateList = new Dictionary<string, CreateAssetBundleAsync>();

    /// <summary>
    /// 异步创建ab
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="onCreateCompleteHandler"></param>
    public void LoadAssetBundleAsync(string abName,Action onCreateCompleteHandler)
    {
        abName = abName.ToLower();
        if (!abName.EndsWith(GameConst.ABExtensionName))
        {
            abName += GameConst.ABExtensionName;
        }
        bool haveBundle = HaveBundle(abName);
        if (haveBundle)
        {
            onCreateCompleteHandler();
            return;
        }
        CreateAssetBundleAsync createAssetBundleAsync = null;
        asyncCreateList.TryGetValue(abName, out createAssetBundleAsync);
        bool isNewCreate = false;
        if (createAssetBundleAsync == null)
        {
            isNewCreate = true;
            createAssetBundleAsync = new CreateAssetBundleAsync();
        }
        createAssetBundleAsync.AddCB(onCreateCompleteHandler);
        if (isNewCreate)
        {
            createAssetBundleAsync.Init(abName, cacheAssetBundle);
        }
    }

    private void cacheAssetBundle(string abName, AssetBundle assetBundle)
    {
        if (!bundles.ContainsKey(abName))
        {
            bundles.Add(abName, assetBundle);
            asyncCreateList.Remove(abName);
        }
    }

    /// <summary>
    /// 载入依赖
    /// </summary>
    /// <param n="n"></param>
    private void LoadDependencies(string abName)
    {
        if (manifest == null)
        {
            Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
            return;
        }
        string[] dependencies = manifest.GetAllDependencies(abName);
        if (dependencies.Length == 0) return;
        int i = 0;
        for (i = 0; i < dependencies.Length; ++i)
        {
            dependencies[i] = RemapVariantn(dependencies[i]);
        }
        for (i = 0; i < dependencies.Length; ++i)
        {
            LoadAssetBundle(dependencies[i]);
        }
    }


    public List<string> GetAssetbundleDependencies(string abName)
    {
        List<string> dps = new List<string>();
        if (manifest == null)
        {
            Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
            return dps;
        }
        // Get dependecies from the AssetBundleManifest object..
        string[] dependencies = manifest.GetAllDependencies(abName);
        if (dependencies.Length == 0) return dps;
        int i = 0;
        for (i = 0; i < dependencies.Length; ++i)
        {
            dependencies[i] = RemapVariantn(dependencies[i]);
        }
        for (i = 0; i < dependencies.Length; ++i)
        {
            dps.Add(dependencies[i]);
        }
        return dps;
    }

    
    private string RemapVariantn(string assetBundleName)
    {
        string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();
        if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
            return assetBundleName;
        string[] split = assetBundleName.Split('.');
        int bestFit = int.MaxValue;
        int bestFitIndex = -1;
        int length = bundlesWithVariant.Length;
        for (int i = 0; i < length; ++i)
        {
            string[] curSplit = bundlesWithVariant[i].Split('.');
            if (curSplit[0] != split[0])
                continue;
            int found = System.Array.IndexOf(variants, curSplit[1]);
            if (found != -1 && found < bestFit)
            {
                bestFit = found;
                bestFitIndex = i;
            }
        }
        if (bestFitIndex != -1)
            return bundlesWithVariant[bestFitIndex];
        else
            return assetBundleName;
    } 
}



public class CreateAssetBundleAsync
{
    private List<Action> cbList = new List<Action>();
    private AssetBundleCreateRequest assetBundleCreateRequest;
    private string assetBundleName;
    private TimerInfo timerInfo;
    private Action<string, AssetBundle> assetBundleCacheHandler;
    public void Init(string abName,Action<string, AssetBundle> abCacheHandler)
    {
        assetBundleName = abName;
        assetBundleCacheHandler = abCacheHandler;
        assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(GameConst.DataPath + abName);
        timerInfo = TimerManager.AddHandler(OnUpdateHandler);
    }

    private void OnUpdateHandler(float delta)
    {
        if (assetBundleCreateRequest.isDone)
        {
            TimerManager.RemoveHandler(timerInfo);
            timerInfo = null;
            if (assetBundleCacheHandler != null)
            {
                assetBundleCacheHandler(assetBundleName, assetBundleCreateRequest.assetBundle);
            }
            for (int i = 0; i < cbList.Count; ++i)
            {
                Action onCreateCompleteHandler = cbList[i];
                onCreateCompleteHandler();
            }
        }
    }

    public void AddCB(Action onCreateCompleteHandler)
    {
        if (!cbList.Contains(onCreateCompleteHandler))
        {
            if (assetBundleCreateRequest != null && assetBundleCreateRequest.isDone)
            {
                onCreateCompleteHandler();
            }
            else
            {
                cbList.Add(onCreateCompleteHandler);
            }
            
        }
    }
}