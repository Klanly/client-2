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
    private ListView thingsListView;
    private ObservableList<string> things = new ObservableList<string>();

    private ListView effectsListView;
    private ObservableList<string> effects = new ObservableList<string>();

    private ListView npcListView;
    private ObservableList<string> npcs = new ObservableList<string>();

    public void Init(Transform transform)
    {
        container = transform;
        mapNameText = container.Find("mapNameText").GetComponent<Text>();
        mapResText = container.Find("mapResText").GetComponent<Text>();
        thingsListView = container.Find("thingsListView").GetComponent<ListView>();
        effectsListView = container.Find("effectsListView").GetComponent<ListView>();
        npcListView = container.Find("npcListView").GetComponent<ListView>();
        container.gameObject.SetActive(false);
    }
    
    public void Show(CopyInfo copyInfo, SceneInfo sceneInfo)
    {
        if(container)
            container.gameObject.SetActive(true);
        mapNameText.text = copyInfo.Name;
        mapResText.text = copyInfo.ResName;
        thingsListView.Clear();
        things.Clear();
        GameObjectInfo gameObjectInfo = null;
        for (int i = 0; i < sceneInfo.Things.Count; ++i)
        {
            gameObjectInfo = sceneInfo.Things[i];
            things.Add(gameObjectInfo.myIndex+":"+ gameObjectInfo.PrefabName);
        }
        gameObjectInfo = sceneInfo.TerrainInfo;
        things.Add(gameObjectInfo.myIndex + ":" + gameObjectInfo.PrefabName);
        thingsListView.DataSource = things;

        effectsListView.Clear();
        effects.Clear();
        for (int i = 0; i < sceneInfo.Effects.Count; ++i)
        {
            gameObjectInfo = sceneInfo.Effects[i];
            effects.Add(gameObjectInfo.myIndex + ":" + gameObjectInfo.PrefabName);
        }
        effectsListView.DataSource = effects;

        npcListView.Clear();
        NpcInfo npcInfo = null;
        for (int i = 0; i < copyInfo.NPCList.Count; ++i)
        {
            npcInfo = copyInfo.NPCList[i];
           // npcs.Add();
        }

    }

    public void Hide()
    {
        if (container)
            container.gameObject.SetActive(false);
    }
}
