using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System;
public class AddEffectSetting
{
    private static AddEffectSetting instance;
    public static AddEffectSetting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AddEffectSetting();
            }
            return instance;
        }
    }


    private Text titleText;
    //private GameObject closeBtn;
    private Combobox effectNameSet;
    private Combobox effectTypeSet;
    private RangeSliderFloat flySpeed;
    private Text speedText;
    private Combobox soundNameSet;
    private RangeSliderFloat soundPlayPointSet;

    private Combobox effectBindTypeSet;
    private Combobox effectBindNameSet;


    private EffectInfo effectInfo;
    private Action<EffectInfo> OnEditorCompleteHandler;

    private GameObject text1;
    private GameObject text2;

    private GameObject cancelBtn;
    private GameObject sureBtn;
    private GameObject delBtn;
    
    private GameObject container;


    private Text soundDelayText;

    private Toggle isLoop;

    public void Hide()
    {
        if (container != null)
        {
            container.SetActive(false);
        }
    }

    public void Init(GameObject go)
    {
        container = go;
        text1 = go.transform.Find("text1").gameObject;
        text2 = go.transform.Find("text2").gameObject;
        //closeBtn = go.transform.Find("closeBtn").gameObject;

        titleText = go.transform.Find("titleText").GetComponent<Text>();

        isLoop = go.transform.Find("isLoop").GetComponent<Toggle>();
        isLoop.isOn = false;
        isLoop.onValueChanged.AddListener(OnIsLoopSelectHandler);

        effectNameSet = go.transform.Find("effectName/Combobox").GetComponent<Combobox>();
        effectNameSet.ListView.Sort = false;
        effectNameSet.ListView.DataSource = DataManager.GetEffectList();
        effectNameSet.OnSelect.AddListener(OnEffectNameSelected);

        effectTypeSet = go.transform.Find("effectType/Combobox").GetComponent<Combobox>();
        effectTypeSet.ListView.Sort = false;
        effectTypeSet.ListView.DataSource = DataManager.GetEffectTypeList();
        effectTypeSet.OnSelect.AddListener(OnEffectTypeSelected);
        

        flySpeed = go.transform.Find("flySpeed").GetComponent<RangeSliderFloat>();
        flySpeed.SetLimit(DataManager.FlySpeedMin, DataManager.FlySpeedMax);
        flySpeed.Step = DataManager.FlySpeedStep;
        flySpeed.OnValuesChange.AddListener(OnFlySpeedChangeHandler);

        speedText = go.transform.Find("flySpeed/speedText").GetComponent<Text>();
        speedText.text = "";

        soundNameSet = go.transform.Find("soundName/Combobox").GetComponent<Combobox>();
        soundNameSet.ListView.Sort = false;
        soundNameSet.ListView.DataSource = DataManager.GetSoundList();
        soundNameSet.OnSelect.AddListener(OnSoundSelected);



        soundDelayText = go.transform.Find("soundDelayText").GetComponent<Text>();
        soundDelayText.text = DataManager.ActionSoundDelayMin.ToString();
        soundPlayPointSet = go.transform.Find("soundPlayPointSet").GetComponent<RangeSliderFloat>();
        soundPlayPointSet.SetLimit(DataManager.ActionSoundDelayMin, DataManager.ActionSoundDelayMax);
        soundPlayPointSet.Step = DataManager.ActionSoundStep;
        soundPlayPointSet.OnValuesChange.AddListener(OnSoundPointSliderChangeHandler);
        



        effectBindTypeSet = go.transform.Find("effectBindSet/Combobox").GetComponent<Combobox>();
        effectBindTypeSet.ListView.Sort = false;
        effectBindTypeSet.ListView.DataSource = DataManager.GetEffectBindList();
        effectBindTypeSet.OnSelect.AddListener(OnEffectBindTypeSelected);

        effectBindNameSet = go.transform.Find("effectBindNameSet/Combobox").GetComponent<Combobox>();
        effectBindNameSet.ListView.Sort = false;
        effectBindNameSet.ListView.DataSource = DataManager.GetEffectBindNameList();
        effectBindNameSet.OnSelect.AddListener(OnEffectBindNameSelected);

        cancelBtn = go.transform.Find("cancelBtn").gameObject;
        sureBtn = go.transform.Find("sureBtn").gameObject;
        delBtn = go.transform.Find("delBtn").gameObject;
        EventTriggerListener.Get(sureBtn).onClick = onSureHandler;
        EventTriggerListener.Get(cancelBtn).onClick = onCancelHandler;
        EventTriggerListener.Get(delBtn).onClick = onDelHandler;
    }


    private void OnIsLoopSelectHandler(bool isOn)
    {
        // && effectInfo.SoundName != String.Empty && effectInfo.SoundName != BindTypes.NONE
        if (effectInfo != null)
        {
            effectInfo.IsLoop = isOn;
        }
    }

    private void OnEffectNameSelected(int index, string title)
    {
        if (effectInfo != null)
        {
            effectInfo.EffectName = title;
        }
    }
    
    private void OnEffectTypeSelected(int index, string title)
    {
        //|| string.IsNullOrEmpty(effectInfo.EffectName) || effectInfo.EffectName == BindTypes.NONE
        if (effectInfo == null) return;
        int ty = EffectTypes.GetKey(title);
        if (effectInfo != null)
        {
            effectInfo.EffectType = ty;
        }
        if (ty == EffectTypes.Normal)
        {
            effectBindTypeSet.transform.parent.gameObject.SetActive(true);
            effectBindNameSet.transform.parent.gameObject.SetActive(true);
            text1.SetActive(true);
            text2.SetActive(true);
            flySpeed.gameObject.SetActive(false);
        }
        else if (ty == EffectTypes.Bullet)
        {
            flySpeed.gameObject.SetActive(true);
            //effectBindTypeSet.transform.parent.gameObject.SetActive(false);
            //effectBindNameSet.transform.parent.gameObject.SetActive(false);
            //text1.SetActive(false);
            //text2.SetActive(false);
            //if (effectInfo != null)
            //{
            //    effectInfo.BindType = BindTypes.None;
            //    effectInfo.BindName = BindTypes.NONE;
            //}
        }
    }

    private void OnSoundSelected(int index, string title)
    {
        if (effectInfo != null)
        {
            effectInfo.SoundName = title;
        }
    }
    
    private void OnFlySpeedChangeHandler(float a, float b)
    {
       
        if (effectInfo != null)
        {
            string v = a.ToString("0.00");
            effectInfo.FlySpeed = float.Parse(v);
            speedText.text = v;
        }
    }


    private void OnSoundPointSliderChangeHandler(float a, float b)
    {
        // && !string.IsNullOrEmpty(effectInfo.SoundName) && effectInfo.SoundName != BindTypes.NONE
        if (effectInfo != null)
        {
            string v = a.ToString("0.00");
            effectInfo.SoundPlayDelayTime = float.Parse(v);
            soundDelayText.text = v;
        }
    }

    private void OnEffectBindTypeSelected(int index, string title)
    {
        // && !string.IsNullOrEmpty(effectInfo.EffectName) && effectInfo.EffectName != BindTypes.NONE
        if (effectInfo != null)
        {
            int ty = BindTypes.GetType(title);
            effectInfo.BindType = ty;
        }
    }

    private void OnEffectBindNameSelected(int index, string title)
    {
        // && !string.IsNullOrEmpty(effectInfo.EffectName) && effectInfo.EffectName != BindTypes.NONE
        if (effectInfo != null)
        {
            string ty = BoneTypes.KeyToValue(title);
            effectInfo.BindName = ty;
        }
    }

    private bool isNewCreate;
    private EffectInfo oldEffectInfo;

    public void Show(string title,EffectInfo eInfo, bool isNew, Action<EffectInfo> onEditorCompleteHandler)
    {
        if (container != null)
        {
            container.SetActive(true);
        }
        titleText.text = title;
        effectInfo = eInfo;
        isNewCreate = isNew;
        oldEffectInfo = effectInfo.Clone();
        OnEditorCompleteHandler = onEditorCompleteHandler;
        flySpeed.SetValue(effectInfo.FlySpeed, DataManager.FlySpeedMax);
        speedText.text = effectInfo.FlySpeed.ToString();


        int index = DataManager.GetEffectIndex(effectInfo.EffectName);
        effectNameSet.ListView.Select(index);

        int eType = effectInfo.EffectType;
        string ety = EffectTypes.GetValue(eType);
        index = DataManager.GetEffectTypeIndex(ety);
        effectTypeSet.ListView.Select(index);


        index = DataManager.GetSoundIndex(effectInfo.SoundName);
        soundNameSet.ListView.Select(index);

        soundDelayText.text = effectInfo.SoundPlayDelayTime.ToString();
        soundPlayPointSet.SetValue(effectInfo.SoundPlayDelayTime,DataManager.ActionSoundDelayMax);


        isLoop.isOn = effectInfo.IsLoop;

        string by = BindTypes.GetKey(effectInfo.BindType);
        index = DataManager.GetEffectBindIndex(by);
        effectBindTypeSet.ListView.Select(index);

        by = BoneTypes.ValueToKey(effectInfo.BindName);
        index = DataManager.GetEffectBindnIndex(by);
        effectBindNameSet.ListView.Select(index);
    }
    
    private void onSureHandler(GameObject GO)
    {
        Hide();
        if (effectInfo.EffectType == EffectTypes.Normal && effectInfo.BindType == BindTypes.Bone && effectInfo.BindName == BindTypes.NONE)
            return;
        if (effectInfo.EffectType == EffectTypes.Normal && effectInfo.BindType == BindTypes.None) return;
        if (OnEditorCompleteHandler != null)
        {
            //if (effectInfo.SoundName == BindTypes.NONE)
            //{
            //    effectInfo.SoundPlayDelayTime = 0f;
            //    effectInfo.IsLoop = false;
            //}
            //if (effectInfo.BindType == BindTypes.None)
            //{
            //    effectInfo.BindName = BindTypes.NONE;
            //}
            OnEditorCompleteHandler(effectInfo);
        }
    }
    private void onCancelHandler(GameObject GO)
    {
        Hide();
        if (!isNewCreate)
        {
            if (OnEditorCompleteHandler != null)
            {
                effectInfo = oldEffectInfo;

                if (effectInfo.EffectType == EffectTypes.Normal && effectInfo.BindType == BindTypes.Bone && effectInfo.BindName == BindTypes.NONE)
                    return;
                if (effectInfo.EffectType == EffectTypes.Normal && effectInfo.BindType == BindTypes.None) return;

                OnEditorCompleteHandler(effectInfo);
            }
        }
    }
    private void onDelHandler(GameObject go)
    {
        Hide();
    }
}
