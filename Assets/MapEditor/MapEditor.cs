using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    private ModelList modelList;
    private CopyInfoView copyInfoView;
    private Scene scene;
    void Start ()
    {
        ResourceManager.Init();
        Alert.Init(GameObject.Find("alert"));
        Alert.Hide();
        modelList = new ModelList();
        modelList.onSelectedHandler = OnCopySelectedHandler;
        modelList.Init(gameObject.transform.Find("copyList").gameObject, DataManager.GetCopyList());
        copyInfoView = new CopyInfoView();
        copyInfoView.Init(GameObject.Find("copyInfoList").transform);
        
    }
    private CopyInfo copyInfo;
    private SceneInfo sceneInfo;
    private void OnCopySelectedHandler(string copyName)
    {
        Debug.Log(copyName);
        copyInfo = ConfigManager.GetCopyInfo(copyName);
        if (copyInfo == null)
        {
            Debug.LogError("缺少配置：" + copyName + ".xml");
        }
        else
        {
            sceneInfo = ConfigManager.GetSceneConfigInfo(copyInfo.ResName);
            if (sceneInfo == null)
            {
                Debug.LogError("缺少配置：" + copyInfo.ResName + ".xml");
            }
            else
            {
                ConfigManager.ParseSceneGrids(copyInfo.ResName);
                for (int i = 0; i < sceneInfo.AllGameObjectInfos.Count; ++i)
                {
                    GameObjectInfo gameObjectInfo = sceneInfo.AllGameObjectInfos[i];
                    if (gameObjectInfo.Type == GameObjectTypes.Effect)
                    {
                        assetbundleList.Add(GameConst.SceneEffectABDirectory + gameObjectInfo.PrefabName + GameConst.ABExtensionName);
                    }
                    else
                    {
                        assetbundleList.Add(GameConst.SceneModelABDirectory + gameObjectInfo.PrefabName + GameConst.ABExtensionName);
                    }
                }
                Alert.Show("初始化中，请稍等......");
                string abPath = assetbundleList[assetbundleList.Count - 1];
                assetbundleList.RemoveAt(assetbundleList.Count - 1);
                ResourceManager.CreateAssetBundleAsync(abPath, onCreateABCompleteHandler);
            }
        }
    }

    private void onCreateABCompleteHandler()
    {
        if (assetbundleList.Count > 0)
        {
            string abPath = assetbundleList[assetbundleList.Count - 1];
            assetbundleList.RemoveAt(assetbundleList.Count - 1);
            ResourceManager.CreateAssetBundleAsync(abPath, onCreateABCompleteHandler);
        }
        else
        {
            Alert.Hide();
            ////开始加载地图
            copyInfoView.Show(copyInfo, sceneInfo);
            if (scene == null)
            {
                scene = new Scene();
                scene.Init(sceneInfo);
            }
            scene.LoadAllThings();
        }
    }


    private List<string> assetbundleList = new List<string>();

    void Update()
    {
        TimerManager.Update(Time.deltaTime);
    }
}
