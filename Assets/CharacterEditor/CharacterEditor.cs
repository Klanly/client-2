using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditor : MonoBehaviour {

    private ModelList modelList;
    void Start ()
    {
        
        ResourceManager.Init();
        Alert.Init(GameObject.Find("alert"));
        AttributeSetting.Instance.Init(GameObject.Find("settingArea"));
        AttributeSetting.Instance.Hide();
        AddEffectSetting.Instance.Init(GameObject.Find("addEffectView"));
        AddEffectSetting.Instance.Hide();

        modelList = new ModelList();
        modelList.onSelectedHandler = OnModelSelectedHandler;
        modelList.Init(GameObject.Find("Canvas/modelList"), "models/characters");
    }

    private TCreature tCreature;

    private void OnModelSelectedHandler(string abn)
    {
        //to do
        CharacterConfigInfo characterConfigInfo = DataManager.GetCharacterConfigInfo(abn);
        if(tCreature == null)
            tCreature = new TCreature();
        tCreature.Init(characterConfigInfo);
        AttributeSetting.Instance.Target = tCreature;
        AttributeSetting.Instance.StartSetting(characterConfigInfo);
        EditorCameraController.Instance.Target = tCreature.Container;
    }
	
	void Update ()
    {
        TimerManager.Update(Time.deltaTime);
        if (tCreature != null)
        {
            tCreature.Update(Time.deltaTime);
        }
    }
}
