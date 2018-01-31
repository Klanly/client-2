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
    
    private void OnCopySelectedHandler(string copyName)
    {
        Debug.Log(copyName);
        CopyInfo copyInfo = ConfigManager.GetCopyInfo(copyName, true);
        if (copyInfo == null)
        {
            Debug.LogError("缺少配置：" + copyName + ".xml");
        }
        else
        {
            SceneInfo sceneInfo = ConfigManager.GetSceneConfigInfo(copyInfo.ResName, true);
            if (sceneInfo == null)
            {
                Debug.LogError("缺少配置：" + copyInfo.ResName + ".xml");
            }
            else
            {
                //开始加载地图
                copyInfoView.Show(copyInfo,sceneInfo);
                if (scene == null)
                {
                    scene = new Scene();
                    scene.Init(sceneInfo);
                }
            }
        }
    }


    void Update()
    {
        TimerManager.Update(Time.deltaTime);
    }
}
