using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
public class CopyInfoView
{
    private Transform container;
    private Text mapNameText;
    private Text mapResText;
    private ListView listView;
    public void Init(Transform transform)
    {
        container = transform;
        mapNameText = container.Find("mapNameText").GetComponent<Text>();
        mapResText = container.Find("mapResText").GetComponent<Text>();
        listView = container.Find("ListView").GetComponent<ListView>();
        container.gameObject.SetActive(false);
    }

    public void Show(CopyInfo copyInfo, SceneInfo sceneInfo)
    {
        if(container)
            container.gameObject.SetActive(true);
        mapNameText.text = copyInfo.Name;
        mapResText.text = copyInfo.ResName;
        listView.Clear();
        //sceneInfo.TerrainInfo
    }

    public void Hide()
    {
        if (container)
            container.gameObject.SetActive(false);
    }
}
