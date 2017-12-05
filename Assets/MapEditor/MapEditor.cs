using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    private ModelList modelList;
	void Start ()
    {
        ResourceManager.Init();
        Alert.Init(GameObject.Find("alert"));
        Alert.Hide();
        modelList = new ModelList();
        modelList.onSelectedHandler = OnCopySelectedHandler;
        modelList.Init(gameObject.transform.FindChild("copyList").gameObject, DataManager.GetCopyList());

    }

    private void OnCopySelectedHandler(string copyName)
    {
        Debug.Log(copyName);
    }


    void Update()
    {
        TimerManager.Update(Time.deltaTime);

    }
}
