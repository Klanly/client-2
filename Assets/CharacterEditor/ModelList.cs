using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UIWidgets;
public class ModelList
{
    
    

    private ListView listView;
    private ObservableList<string> list = new ObservableList<string>();
    private string abHead;
    public Action<string> onSelectedHandler;
    protected GameObject container;
    public virtual void Init(GameObject content, string abPath)
    {
        container = content;
        abHead = abPath;
        string dP = Application.dataPath + "/" + GameConst.StreamingAssets + "/";
        string allP = dP + abHead;

        string[] models = Directory.GetFiles(allP);
        foreach (string file in models)
        {
            if (file.EndsWith(GameConst.ABExtensionName))
            {
                list.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        listView = content.transform.Find("ListView").GetComponent<ListView>();
        listView.DataSource = list;
        
        listView.OnSelect.AddListener(OnSelectedModelHandler);
    }

    public virtual void Init(GameObject content, ObservableList<string> vList)
    {
        list = vList;
        container = content;
        listView = content.transform.Find("ListView").GetComponent<ListView>();
        listView.DataSource = vList;
        listView.OnSelect.AddListener(OnSelectedModelHandler);
    }


    private int oldIndex = -1;

    private void OnSelectedModelHandler(int index, ListViewItem listViewItem)
    {
        if(oldIndex >= 0)
            listView.Deselect(oldIndex);

        string label = list.ToArray()[index];
        oldIndex = index;
        //Debug.LogError("label:" + label);
        if (onSelectedHandler != null)
        {
            onSelectedHandler(label);
        }
    }


    public void Show()
    {
        if (container != null)
        {
            container.SetActive(true);
        }
    }

    public void Hide()
    {
        if (container != null)
        {
            container.SetActive(false);
        }
    }
}
