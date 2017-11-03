using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using System.IO;
public class AutoMakeSceneConfig
{

    [MenuItem("GameTools/自动生成场景配置",false,5)]
    public static void AutoMakeScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetActiveScene();
        Debug.Log("scene name:"+ scene.name+" / scene path:"+scene.path);
        GameObject[] gos = scene.GetRootGameObjects();
        SceneInfo sceneInfo = new SceneInfo();
        for (int i = 0; i < gos.Length; ++i)
        {
            GameObject go = gos[i];
            Debug.Log(i+"/go.name:"+go.name);
            Transform ts;
            GameObjectInfo gameObjectInfo;
            if (go.name == "terrain")
            {
                //地形
                ts = go.transform.GetChild(0);
                gameObjectInfo  = GetGameObjectInfo(ts,true);
                sceneInfo.AddGameObjectInfo(gameObjectInfo);
            }
            else if(go.name == "nonblocks")
            {
                //非阻挡
                int count = go.transform.childCount;
                for (int j = 0; j < count; ++j)
                {
                    ts = go.transform.GetChild(j);
                    gameObjectInfo = GetGameObjectInfo(ts);
                    sceneInfo.AddGameObjectInfo(gameObjectInfo);
                }
            }
        }
        SaveConfig(sceneInfo, scene.path);
    }


    private static GameObjectInfo GetGameObjectInfo(Transform ts, bool isTerrain = false)
    {
        if (ts == null) return null;
        GameObjectInfo gameObjectInfo = new GameObjectInfo();
        gameObjectInfo.GameObjectName = ts.gameObject.name;
        gameObjectInfo.X = ts.position.x;
        gameObjectInfo.Y = ts.position.y;
        gameObjectInfo.Z = ts.position.z;
        gameObjectInfo.RotationX = ts.eulerAngles.x;
        gameObjectInfo.RotationY = ts.eulerAngles.y;
        gameObjectInfo.RotationZ = ts.eulerAngles.z;
        gameObjectInfo.ScaleX = ts.localScale.x;
        gameObjectInfo.ScaleY = ts.localScale.y;
        gameObjectInfo.ScaleZ = ts.localScale.z;
        return gameObjectInfo;
    }


    private static void SaveConfig(SceneInfo sceneInfo,string path)
    {
        string content = sceneInfo.ToXMLString();
        path = path.Replace(".uinty",".xml");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        StreamWriter SW;
        SW = File.CreateText(path);
        SW.WriteLine(content);
        SW.Close();
    }
    
}




