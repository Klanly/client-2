using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

public class Client : MonoBehaviour
{
    //private float CurrentEngineTime = 0.0f;
    //private float LastEngineTime = 0.0f;
    //private float CurrentRealTime = 0.0f;
    //private float LastRealTime = 0.0f;
    
    void Start ()
    {
        //CurrentEngineTime = LastEngineTime = Time.time;
        //CurrentRealTime = LastRealTime = Time.realtimeSinceStartup;
        //Debug.LogError("Application.persistentDataPath:"+ Application.persistentDataPath);
        //Debug.LogError("Application.dataPath:" + Application.dataPath);
        //Debug.LogError("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        //Debug.LogError("Application.temporaryCachePath:" + Application.temporaryCachePath);    
        //string fromPath = Application.dataPath + "/ArtAssets/prefabs";
        //string toPath = Application.dataPath + "/ArtAssets/prefabs_temp.zip";
        //ZipHelper.ZipDirectory(fromPath, toPath);
        //FastZip fzip = new FastZip();
        //fzip.CreateZip(toPath, fromPath, true, "");
        ////fzip.ExtractZip(toPath, Application.dataPath + "/ArtAssets", "");
    }
    
    void Update ()
    {
        //LastRealTime = CurrentRealTime;
        //CurrentRealTime = Time.realtimeSinceStartup;
        //LastEngineTime = CurrentRealTime;
        //CurrentEngineTime = Time.time;
        //float deltaTime = CurrentRealTime - LastRealTime;
        TimerManager.Update(Time.deltaTime);
    }
}
