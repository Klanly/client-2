using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.IO;
using System;
public class AttributeSetting
{
    private static AttributeSetting attributeSetting;
    public static AttributeSetting Instance
    {
        get
        {
            if (attributeSetting == null)
            {
                attributeSetting = new AttributeSetting();
            }
            return attributeSetting;
        }
    }
    
    private CharacterConfigInfo currentCharacterConfigInfo;
    private ActionInfo currentActionInfo;

    private GameObject container;
    private Text modelnText;
    private Combobox leftweapon;
    private Combobox rightweapon;
    private RangeSliderFloat walkSpeedSet;
    private Text walkSpeedValue;

    private RangeSliderFloat runSpeedSet;
    private Text runSpeedValue;

    private Combobox actionNameSet;
    private Text actionLengthText;
    private RangeSliderFloat actionSpeedSet;
    private Text actionSpeedtext;
    private Toggle actionIsLoop;

    private Combobox actionSoundSet;

    private RangeSliderFloat actionSoundPlayPointSet;
    private Text actionSoundPlayPointText;

    private RangeSliderFloat selfMoveDelayTimeSet;
    private Text selfMoveDelayText;
    
    private RangeSliderFloat selfMoveDistanceSet;
    private Text selfMoveDistanceText;

    private RangeSliderFloat selfMoveTimeSet;
    private Text selfMoveTimeText;

    private RangeSliderFloat attackRadioSet;
    private Text attackRadioText;

    private RangeSliderFloat attackAngleSet;
    private Text attackAngleText;


    private UnityEngine.UI.Toggle isHitMoveSet;
    private Text hitMoveText;
    private RangeSliderFloat hitMoveDistanceSet;
    private Text hitMoveDistanceText;
    private RangeSliderFloat hitMoveTimeSet;
    private Text hitMoveTimeText;


    private UnityEngine.UI.Toggle isHitFlySet;
    private Text hitFlyText;
    private RangeSliderFloat hitFlyDistanceSet;
    private Text hitFlyDistanceText;
    private RangeSliderFloat hitFlyTimeSet;
    private Text hitFlyTimeText;

    private Toggle shakeScreenToggle;
    private Combobox shakeScreenSet;

    private ListView actionEffectsSet;
    private Button addActionEffectBtn;
    private ListView hitEffectsSet;
    private Button hitActionEffectBtn;
    private Button previewBtn;
    private Button saveBtn;

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
        Transform content = container.transform.Find("content");
        modelnText = content.Find("modelnameText").GetComponent<Text>();

        leftweapon = content.Find("leftweapon/combobox/Combobox").GetComponent<Combobox>();
        leftweapon.ListView.Sort = false;
        leftweapon.ListView.DataSource = DataManager.GetWeaponList();
        leftweapon.OnSelect.AddListener(OnLeftWeaponSelected);

        rightweapon = content.Find("rightweapon/combobox/Combobox").GetComponent<Combobox>();
        rightweapon.ListView.Sort = false;
        rightweapon.ListView.DataSource = DataManager.GetWeaponList();
        rightweapon.OnSelect.AddListener(OnRightWeaponSelected);

        walkSpeedSet = content.Find("walkSpeedSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        walkSpeedSet.SetLimit(DataManager.WalkSpeedMin, DataManager.WalkSpeedMax);
        walkSpeedSet.Step = DataManager.Step;
        walkSpeedValue = content.Find("walkSpeedSet/value").GetComponent<Text>();
        walkSpeedSet.OnValuesChange.AddListener(OnWalkSpeedSliderChangeHandler);
        
        runSpeedSet = content.Find("runSpeedSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        runSpeedSet.SetLimit(DataManager.RunSpeedMin, DataManager.RunSpeedMax);
        runSpeedSet.Step = DataManager.Step;
        runSpeedValue = content.Find("runSpeedSet/value").GetComponent<Text>();
        runSpeedSet.OnValuesChange.AddListener(OnRunSpeedSliderChangeHandler);
        
        actionNameSet = content.Find("actionNameSet/combobox/Combobox").GetComponent<Combobox>();
        actionNameSet.ListView.Sort = false;
        actionNameSet.ListView.DataSource = DataManager.GetActionList();
        actionNameSet.OnSelect.AddListener(OnActionnSelected);

        actionLengthText = content.Find("actionNameSet/actionLengthText").GetComponent<Text>();
        actionLengthText.text = "0";



        actionSpeedSet = content.Find("actionSpeedSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        actionSpeedSet.SetLimit(DataManager.ActionSpeedMin, DataManager.ActionSpeedMax);
        actionSpeedSet.Step = DataManager.Step;
        actionSpeedtext = content.Find("actionSpeedSet/Text").GetComponent<Text>();
        actionSpeedSet.OnValuesChange.AddListener(OnActionSpeedSliderChangeHandler);

        actionIsLoop = content.Find("actionIsLoop").GetComponent<Toggle>();
        actionIsLoop.isOn = false;
        actionIsLoop.onValueChanged.AddListener(OnActionIsLoopHandler);
        

        actionSoundSet = content.Find("actionSoundSet/combobox/Combobox").GetComponent<Combobox>();
        actionSoundSet.ListView.Sort = false;
        actionSoundSet.ListView.DataSource = DataManager.GetSoundList();
        actionSoundSet.OnSelect.AddListener(OnActionSoundSelected);
        
        //todo 动作音效延迟播放时间 最小值为0f 最大值为当前动作的总时间 暂时先按照配置
        actionSoundPlayPointSet = content.Find("actionSoundPlayPointSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        
        actionSoundPlayPointSet.SetLimit(DataManager.ActionSoundDelayMin, DataManager.ActionSoundDelayMax);
        actionSoundPlayPointSet.Step = DataManager.ActionSoundStep;
        actionSoundPlayPointText = content.Find("actionSoundPlayPointSet/Text").GetComponent<Text>();
        actionSoundPlayPointSet.OnValuesChange.AddListener(OnActionSoundPointSliderChangeHandler);
        
        selfMoveDelayTimeSet = content.Find("selfMoveDelayTimeSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        selfMoveDelayTimeSet.SetLimit(0f, 10f);
        selfMoveDelayTimeSet.Step = DataManager.Step;
        selfMoveDelayText = content.Find("selfMoveDelayTimeSet/Text").GetComponent<Text>();
        selfMoveDelayTimeSet.OnValuesChange.AddListener(OnSelfMoveDelaySliderChangeHandler);
        
        selfMoveDistanceSet = content.Find("selfMoveDistanceSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        selfMoveDistanceSet.SetLimit(DataManager.SelfMoveDistanceMin, DataManager.SelfMoveDistanceMax);
        selfMoveDistanceSet.Step = DataManager.Step;
        selfMoveDistanceText = content.Find("selfMoveDistanceSet/Text").GetComponent<Text>();
        selfMoveDistanceSet.OnValuesChange.AddListener(OnSelfMoveDistanceSliderChangeHandler);

        selfMoveTimeSet = content.Find("selfMoveTimeSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        selfMoveTimeSet.SetLimit(DataManager.SelfMoveTimeMin, DataManager.SelfMoveTimeMax);
        selfMoveTimeSet.Step = DataManager.Step;
        selfMoveTimeText = content.Find("selfMoveTimeSet/Text").GetComponent<Text>();
        selfMoveTimeSet.OnValuesChange.AddListener(OnSelfMoveTimeSliderChangeHandler);

        attackRadioSet = content.Find("attackRadioSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        attackRadioSet.SetLimit(DataManager.AttackRadioMin, DataManager.AttackRadioMax);
        attackRadioSet.Step = DataManager.Step;
        attackRadioText = content.Find("attackRadioSet/Text").GetComponent<Text>();
        attackRadioSet.OnValuesChange.AddListener(OnAttackRadioSliderChangeHandler);
        
        attackAngleSet = content.Find("attackAngleSet/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        attackAngleSet.SetLimit(DataManager.AttackAngleMin, DataManager.AttackAngleMax);
        attackAngleSet.Step = DataManager.Step;
        attackAngleText = content.Find("attackAngleSet/Text").GetComponent<Text>();
        attackAngleSet.OnValuesChange.AddListener(OnAttackAngleSliderChangeHandler);
        
        isHitMoveSet = content.Find("isHitMoveSet/Toggle").GetComponent<Toggle>();
        hitMoveText = content.Find("isHitMoveSet/Text").GetComponent<Text>();
        isHitMoveSet.isOn = false;
        hitMoveText.text = "0";
        isHitMoveSet.onValueChanged.AddListener(OnHitMoveSelectHandler);
        hitMoveDistanceSet = content.Find("isHitMoveSet/moveDistanceText/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        hitMoveDistanceSet.SetLimit(DataManager.HitMoveDistanceMin, DataManager.HitMoveDistanceMax);
        hitMoveDistanceSet.Step = DataManager.Step;
        hitMoveDistanceSet.OnValuesChange.AddListener(OnHitMoveDistanceSliderChangeHandler);

        hitMoveDistanceText = content.Find("isHitMoveSet/moveDistanceText/Text").GetComponent<Text>();
        hitMoveTimeSet = content.Find("isHitMoveSet/moveTimeText/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        hitMoveTimeSet.SetLimit(DataManager.HitMoveTimeMin, DataManager.HitMoveTimeMax);
        hitMoveTimeSet.Step = DataManager.Step;
        hitMoveTimeSet.OnValuesChange.AddListener(OnHitMoveTimeSliderChangeHandler);
        hitMoveTimeText = content.Find("isHitMoveSet/moveTimeText/Text").GetComponent<Text>();
        
        isHitFlySet = content.Find("isHitFlySet/Toggle").GetComponent<Toggle>();
        hitFlyText = content.Find("isHitFlySet/Text").GetComponent<Text>();
        isHitFlySet.isOn = false;
        hitFlyText.text = "0";
        isHitFlySet.onValueChanged.AddListener(OnHitFlySelectHandler);

        hitFlyDistanceSet = content.Find("isHitFlySet/moveDistanceText/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        hitFlyDistanceSet.SetLimit(DataManager.HitFlyDistanceMin, DataManager.HitFlyDistanceMax);
        hitFlyDistanceSet.Step = DataManager.Step;
        hitFlyDistanceText = content.Find("isHitFlySet/moveDistanceText/Text").GetComponent<Text>();
        hitFlyDistanceSet.OnValuesChange.AddListener(OnHitFlyDistanceSliderChangeHandler);
        hitFlyTimeSet = content.Find("isHitFlySet/moveTimeText/RangeSliderFloat").GetComponent<RangeSliderFloat>();
        hitFlyTimeSet.SetLimit(DataManager.HitFlyTimeMin, DataManager.HitFlyTimeMax);
        hitFlyTimeSet.Step = DataManager.Step;
        hitFlyTimeText = content.Find("isHitFlySet/moveTimeText/Text").GetComponent<Text>();
        hitFlyTimeSet.OnValuesChange.AddListener(OnHitFlyTimeSliderChangeHandler);
        
        shakeScreenSet  = content.Find("shakeScreenSet/combobox/Combobox").GetComponent<Combobox>();
        shakeScreenSet.ListView.DataSource = DataManager.GetShakeCameraList();
        shakeScreenSet.OnSelect.AddListener(OnShakeCameraHandler);

        shakeScreenToggle = content.Find("shakeScreenToggle").GetComponent<Toggle>();
        shakeScreenToggle.isOn = false;
        shakeScreenToggle.onValueChanged.AddListener(OnShakeCameraSelectHandler);

        

        actionEffectsSet = content.Find("actionEffectsSet/ListView").GetComponent<ListView>();
        actionEffectsSet.OnSelect.AddListener(OnActionEffectSelectedHandler);
        addActionEffectBtn = content.Find("actionEffectsSet/addBtn").GetComponent<Button>();
        addActionEffectBtn.onClick.AddListener(OnAddActionEffectHandler);


        hitEffectsSet = content.Find("hitEffectsSet/ListView").GetComponent<ListView>();
        hitEffectsSet.OnSelect.AddListener(OnHitEffectSelectedHandler);
        hitActionEffectBtn = content.Find("hitEffectsSet/addBtn").GetComponent<Button>();
        hitActionEffectBtn.onClick.AddListener(OnAddHitEffectHandler);


        previewBtn = content.Find("previewBtn").GetComponent<Button>();
        previewBtn.onClick.AddListener(OnPreviewBtnClickHandler);
        saveBtn = content.Find("saveBtn").GetComponent<Button>();
        saveBtn.onClick.AddListener(OnSaveBtnClickHandler);
        
    }

    private void OnActionEffectSelectedHandler(int index, ListViewItem listViewItem)
    {
        string label = actionEffectsSet.DataSource.ToArray()[index];
        string key = label.Split(':')[0];
        foreach (EffectInfo eInfo in currentActionInfo.ActionEffectInfos)
        {
            if (eInfo.Index.ToString() == key)
            {
                currentActionInfo.ActionEffectInfos.Remove(eInfo);
                ShowEffect(currentActionInfo.ActionEffectInfos);
                AddEffectSetting.Instance.Show("编辑动作特效", eInfo, false, OnActionEffectEditorCompleteHandler);
                return;
            }
        }
    }

    
    private void OnActionEffectEditorCompleteHandler(EffectInfo effectInfo)
    {
        if (currentActionInfo != null)
        {
            currentActionInfo.ActionEffectInfos.Add(effectInfo);
            ShowEffect(currentActionInfo.ActionEffectInfos);
        }
    }

    private static ObservableList<string> actionEffectList = new ObservableList<string>();
    private static ObservableList<string> hitEffectList = new ObservableList<string>();

    private void ShowEffect(List<EffectInfo> list, bool isActionEffect = true)
    {
        EffectInfo effectInfo;
        ObservableList<string> tOList;
        ListView tListView;
        if (isActionEffect)
        {
            tOList = actionEffectList;
            tListView = actionEffectsSet;
        } else
        {
            tOList = hitEffectList;
            tListView = hitEffectsSet;
        }
        tOList.RemoveRange(0, tOList.Count);
        for (int i = 0; i < list.Count; ++i)
        {
            effectInfo = list[i];
            tOList.Add(effectInfo.Index + ":" + effectInfo.EffectName);
        }
        tListView.DataSource = tOList;
    }

    

    private void OnHitEffectSelectedHandler(int index, ListViewItem listViewItem)
    {
        string label = hitEffectsSet.DataSource.ToArray()[index];
        string key = label.Split(':')[0];
        foreach (EffectInfo eInfo in currentActionInfo.HitEffectInfos)
        {
            if (eInfo.Index.ToString() == key)
            {
                currentActionInfo.HitEffectInfos.Remove(eInfo);
                ShowEffect(currentActionInfo.HitEffectInfos,false);
                AddEffectSetting.Instance.Show("编辑受击特效",eInfo, false, OnHitEffectEditorCompleteHandler);
                return;
            }
        }
    }
    private void OnHitEffectEditorCompleteHandler(EffectInfo effectInfo)
    {
        if (currentActionInfo != null)
        {
            currentActionInfo.HitEffectInfos.Add(effectInfo);
            ShowEffect(currentActionInfo.HitEffectInfos,false);
        }
    }

    public Action<string, string, string> OnWearWeaponHandler; 
    private void OnLeftWeaponSelected(int index, string title)
    {
        Debug.LogError(index + ":" + title);
        if (currentCharacterConfigInfo == null)
            return;
        currentCharacterConfigInfo.LeftWeaponName = title;
        if (OnWearWeaponHandler != null)
        {
            OnWearWeaponHandler("added_leftweapon", currentCharacterConfigInfo.LeftWeaponName, BoneTypes.Bone_Lwq);
        }
        
    }
    private void OnRightWeaponSelected(int index, string title)
    {
        Debug.LogError(index + ":" + title);
        if (currentCharacterConfigInfo == null)
            return;
        currentCharacterConfigInfo.RightWeaponName = title;
        if (OnWearWeaponHandler != null)
        {
            OnWearWeaponHandler("added_rightweapon", currentCharacterConfigInfo.RightWeaponName, BoneTypes.Bone_Rwq);
        }
    }
    

    private void OnWalkSpeedSliderChangeHandler(float a, float b)
    {
        if (currentCharacterConfigInfo == null)
            return;
        string v = a.ToString("0.00");
        currentCharacterConfigInfo.WalkSpeed = float.Parse(v);
        walkSpeedValue.text = v;
    }
    private void OnRunSpeedSliderChangeHandler(float a, float b)
    {
        if (currentCharacterConfigInfo == null)
            return;
        string v = a.ToString("0.00");
        currentCharacterConfigInfo.RunSpeed = float.Parse(v);
        runSpeedValue.text = v;
    }

    public Action<string> OnActionSelectedHandler;

    private Dictionary<string, Dictionary<string, float>> actionLengthMap = new Dictionary<string, Dictionary<string, float>>();
    private TimerInfo timerInfo;
    Dictionary<string, float> lengthMap;
    private void OnActionnSelected(int index, string title)
    {
        Debug.Log(index + ":" + title);
        string actionName = title;
        currentActionInfo = currentCharacterConfigInfo.GetActionInfo(actionName);
        SetActionAttribute(currentActionInfo);
        if (OnActionSelectedHandler != null)
        {
            OnActionSelectedHandler(actionName);
        }
        //if (tCreature == null) return;
        //tCreature.PlayAnimation(actionName, true);
        
        actionLengthMap.TryGetValue(currentCharacterConfigInfo.ModelName, out lengthMap);
        if (lengthMap == null)
        {
            lengthMap = new Dictionary<string, float>();
            actionLengthMap.Add(currentCharacterConfigInfo.ModelName, lengthMap);
        }
        if (lengthMap.ContainsKey(actionName))
        {
            ReSet();
        }
        else
        {
            //to do 提示计算时间中 加遮罩屏蔽所有操作
            timerInfo = TimerManager.AddDelayHandler(OnDelayHandler, 0.2f, 1);
        }
    }
    public Func<float> GetActionLength;
    private void OnDelayHandler(float del)
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        float length = GetActionLength();
        Debug.Log("n:" + currentActionInfo.ActionName + " / length:" + length);
        currentActionInfo.Length = length;
        lengthMap.Add(currentActionInfo.ActionName, length);
        
        ReSet();
    }

    private void ReSet()
    {
        float length = 0f;
        lengthMap.TryGetValue(currentActionInfo.ActionName, out length);
        length = length / currentActionInfo.PlaySpeed;
        actionLengthText.text = length.ToString()+"秒";
        //设置delay时间点
        if (string.IsNullOrEmpty(currentActionInfo.SoundName) && currentActionInfo.SoundName != BindTypes.NONE)
        {
            actionSoundPlayPointSet.SetLimit(DataManager.ActionSoundDelayMin, length);
            if (currentActionInfo.SoundPlayDelayTime > length)
                currentActionInfo.SoundPlayDelayTime = length;
            actionSoundPlayPointSet.SetValue(currentActionInfo.SoundPlayDelayTime, length);
        }
        //if (currentActionInfo.SelfMoveDistance > 0f && currentActionInfo.SelfMoveTime > 0f)
        //{
        //    if (!currentActionInfo.IsLoop)
        //    {
        //        if (currentActionInfo.SelfMoveDelayTime + currentActionInfo.SelfMoveTime > length)
        //        {
        //            Alert.Show("请重新设置自身位移时间，和自身位移延迟时间！！！");
        //        }
        //    }
        //    else
        //    {

        //    }
        //}
    }


    private void SetActionAttribute(ActionInfo actionInfo)
    {
        int index;
        actionSpeedSet.SetValue(actionInfo.PlaySpeed, DataManager.ActionSpeedMax);
        actionSpeedtext.text = actionInfo.PlaySpeed.ToString();

        actionIsLoop.isOn = actionInfo.IsLoop;

        selfMoveDelayTimeSet.SetValue(actionInfo.SelfMoveDelayTime,10f);
        selfMoveDelayText.text = actionInfo.SelfMoveDelayTime.ToString();

        selfMoveDistanceSet.SetValue(actionInfo.SelfMoveDistance, DataManager.SelfMoveDistanceMax);
        selfMoveDistanceText.text = actionInfo.SelfMoveDistance.ToString();

        selfMoveTimeSet.SetValue(actionInfo.SelfMoveTime, DataManager.SelfMoveTimeMax);
        selfMoveTimeText.text = actionInfo.SelfMoveTime.ToString();

        index = DataManager.GetSoundIndex(actionInfo.SoundName);
        actionSoundSet.ListView.Select(index);

        actionSoundPlayPointSet.SetValue(actionInfo.SoundPlayDelayTime, DataManager.ActionSoundDelayMax);
        actionSoundPlayPointText.text = actionInfo.SoundPlayDelayTime.ToString();

        attackRadioSet.SetValue(actionInfo.AttackRadius, DataManager.AttackRadioMax);
        attackRadioText.text = actionInfo.AttackRadius.ToString();

        attackAngleSet.SetValue(actionInfo.AttackAngle, DataManager.AttackAngleMax);
        attackAngleText.text = actionInfo.AttackAngle.ToString();

        isHitMoveSet.isOn = actionInfo.IsHitMove;
        if (!actionInfo.IsHitMove)
        {
            actionInfo.HitMoveDistance = DataManager.HitMoveDistanceMin;
            actionInfo.HitMoveTime = DataManager.HitMoveTimeMin;
        }
        
        hitMoveDistanceSet.SetValue(actionInfo.HitMoveDistance, DataManager.HitMoveDistanceMax);
        hitMoveDistanceText.text = actionInfo.HitMoveDistance.ToString();
        hitMoveTimeSet.SetValue(actionInfo.HitMoveTime, DataManager.HitMoveTimeMax);
        hitMoveTimeText.text = actionInfo.HitMoveTime.ToString();
        if (!actionInfo.IsHitMove)
        {
            hitMoveDistanceSet.enabled = false;
            hitMoveTimeSet.enabled = false;
        }
        else
        {
            hitMoveDistanceSet.enabled = true;
            hitMoveTimeSet.enabled = true;
        }
        
        isHitFlySet.isOn = actionInfo.IsHitFly;
        if (!actionInfo.IsHitFly)
        {
            actionInfo.HitFlyDistance = DataManager.HitFlyDistanceMin;
            actionInfo.HitFlyTime = DataManager.HitFlyTimeMin;
        }
        hitFlyDistanceSet.SetValue(actionInfo.HitFlyDistance, DataManager.HitFlyDistanceMax);
        hitFlyDistanceText.text = actionInfo.HitFlyDistance.ToString();
        hitFlyTimeSet.SetValue(actionInfo.HitFlyTime, DataManager.HitFlyTimeMax);
        hitFlyTimeText.text = actionInfo.HitFlyTime.ToString();


        index = DataManager.GetShakeCameraIndex(actionInfo.ShakeScreen);
        shakeScreenSet.ListView.Select(index);

        //shakeScreenSet
        shakeScreenToggle.isOn = actionInfo.DefaultShakeScene;


        ShowEffect(actionInfo.ActionEffectInfos);
        
        ShowEffect(actionInfo.HitEffectInfos,false);
    }




    private void OnActionSoundSelected(int index, string title)
    {
        Debug.LogError(index + ":" + title);
        if (currentActionInfo != null)
        {
            currentActionInfo.SoundName = title;

            
        }
    }


    private void OnShakeCameraSelectHandler(bool isOn)
    {
        if (currentActionInfo != null)
        {
            currentActionInfo.DefaultShakeScene = isOn;
        }
    }
    private void OnShakeCameraHandler(int index, string title)
    {
        Debug.LogError(index + ":" + title);
        if (currentActionInfo != null)
        {
            currentActionInfo.ShakeScreen = title;
        }
    }
    

    private void OnActionSpeedSliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);
        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.PlaySpeed = float.Parse(v);
            actionSpeedtext.text = v;

            float length = currentActionInfo.Length / currentActionInfo.PlaySpeed;
            actionLengthText.text = length.ToString() + "秒";

        }
    }
    private void OnActionIsLoopHandler(bool isOn)
    {
        if (currentActionInfo != null)
        {
            currentActionInfo.IsLoop = isOn;
        }
    }



    private void OnActionSoundPointSliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);

        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.SoundPlayDelayTime = float.Parse(v);
            actionSoundPlayPointText.text = v;
        }

    }

    
    private void OnSelfMoveDelaySliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);
        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.SelfMoveDelayTime = float.Parse(v);
            selfMoveDelayText.text = v;
        }
    }



    private void OnSelfMoveDistanceSliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);
        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.SelfMoveDistance = float.Parse(v);
            selfMoveDistanceText.text = v;
        }
    }
    private void OnSelfMoveTimeSliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);
        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.SelfMoveTime = float.Parse(v);
            selfMoveTimeText.text = v;
        }
    }

    private void OnAttackRadioSliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);
        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.AttackRadius = float.Parse(v);
            attackRadioText.text = v;
        }
    }
    private void OnAttackAngleSliderChangeHandler(float a, float b)
    {
        Debug.LogError("a:" + a + "/b:" + b);
        if (currentActionInfo != null)
        {
            string v = a.ToString("0.00");
            currentActionInfo.AttackAngle = float.Parse(v);
            attackAngleText.text = v;
        }
    }

    
        

    private void OnHitMoveSelectHandler(bool isSelect)
    {
        if (currentActionInfo != null)
        {
            currentActionInfo.IsHitMove = isSelect;
            if (!currentActionInfo.IsHitMove)
            {
                currentActionInfo.HitMoveDistance = DataManager.HitMoveDistanceMin;
                currentActionInfo.HitMoveTime = DataManager.HitMoveTimeMin;
            }
            hitMoveDistanceSet.SetValue(currentActionInfo.HitMoveDistance, DataManager.HitMoveDistanceMax);
            hitMoveDistanceText.text = currentActionInfo.HitMoveDistance.ToString();
            hitMoveTimeSet.SetValue(currentActionInfo.HitMoveTime, DataManager.HitMoveTimeMax);
            hitMoveTimeText.text = currentActionInfo.HitMoveTime.ToString();
        }
        hitMoveText.text = isSelect ? "1" : "0";
    }
    private void OnHitMoveDistanceSliderChangeHandler(float a, float b)
    {
        if (currentActionInfo != null && currentActionInfo.IsHitMove)
        {
            string v = a.ToString("0.00");
            currentActionInfo.HitMoveDistance = float.Parse(v);
            hitMoveDistanceSet.SetValue(currentActionInfo.HitMoveDistance, DataManager.HitMoveDistanceMax);
            hitMoveDistanceText.text = v;
        }
    }
    private void OnHitMoveTimeSliderChangeHandler(float a, float b)
    {
        if (currentActionInfo != null && currentActionInfo.IsHitMove)
        {
            string v = a.ToString("0.00");
            currentActionInfo.HitMoveTime = float.Parse(v);
            hitMoveTimeSet.SetValue(currentActionInfo.HitMoveTime, DataManager.HitMoveTimeMax);
            hitMoveTimeText.text = v;
        }
    }
    
    private void OnHitFlySelectHandler(bool isSelect)
    {
        if (currentActionInfo != null)
        {
            currentActionInfo.IsHitFly = isSelect;
            hitFlyText.text = isSelect ? "1" : "0";
            //if (!currentActionInfo.IsHitFly)
            //{
            //    currentActionInfo.HitFlyDistance = DataManager.HitFlyDistanceMin;
            //    currentActionInfo.HitFlyTime = DataManager.HitFlyTimeMin;
            //}
            //hitFlyDistanceSet.SetValue(currentActionInfo.HitFlyDistance, DataManager.HitFlyDistanceMax);
            //hitFlyDistanceText.text = currentActionInfo.HitFlyDistance.ToString();
            //hitFlyTimeSet.SetValue(currentActionInfo.HitFlyTime, DataManager.HitFlyTimeMax);
            //hitFlyTimeText.text = currentActionInfo.HitFlyTime.ToString();
        }
    }


    private void OnHitFlyDistanceSliderChangeHandler(float a, float b)
    {
        if (currentActionInfo != null)// && currentActionInfo.IsHitFly)
        {
            string v = a.ToString("0.00");
            currentActionInfo.HitFlyDistance = float.Parse(v);
            hitFlyDistanceSet.SetValue(currentActionInfo.HitFlyDistance, DataManager.HitFlyDistanceMax);
            hitFlyDistanceText.text = v;
        }
    }
    private void OnHitFlyTimeSliderChangeHandler(float a, float b)
    {
        if (currentActionInfo != null)// && currentActionInfo.IsHitFly)
        {
            string v = a.ToString("0.00");
            currentActionInfo.HitFlyTime = float.Parse(v);
            hitFlyTimeSet.SetValue(currentActionInfo.HitFlyTime, DataManager.HitFlyTimeMax);
            hitFlyTimeText.text = v;
        }
    }

    
    public void StartSetting(CharacterConfigInfo characterConfigInfo)
    {
        if (container != null)
        {
            container.SetActive(true);
        }
        currentCharacterConfigInfo = characterConfigInfo;
        
        modelnText.text = currentCharacterConfigInfo.ModelName;
        int index = DataManager.GetWeaponIndex(characterConfigInfo.LeftWeaponName);
        leftweapon.ListView.Select(index);

        index = DataManager.GetWeaponIndex(characterConfigInfo.RightWeaponName);
        rightweapon.ListView.Select(index);
        
        walkSpeedSet.SetValue(currentCharacterConfigInfo.WalkSpeed, DataManager.WalkSpeedMax);
        walkSpeedValue.text = currentCharacterConfigInfo.WalkSpeed.ToString();
        runSpeedSet.SetValue(currentCharacterConfigInfo.RunSpeed, DataManager.RunSpeedMax);
        runSpeedValue.text = currentCharacterConfigInfo.RunSpeed.ToString();
        
        index = DataManager.GetActionIndex(AnimationType.Idle);
        actionNameSet.ListView.Select(index);
    }


    public Action<CharacterConfigInfo,ActionInfo> PreviewHandler;
    
    private void OnPreviewBtnClickHandler()
    {
        if (PreviewHandler != null)
            PreviewHandler(currentCharacterConfigInfo,currentActionInfo);
    }

    
    private void OnSaveBtnClickHandler()
    {
        Debug.LogError("save");
        Alert.Show("保存中。。。");
        DataManager.Save(OnCompleteHandler);
    }

    private void OnCompleteHandler()
    {
        Alert.Hide();
    }

    private void OnAddActionEffectHandler()
    {
        //to do 
        AddEffectSetting.Instance.Show("添加动作特效", new EffectInfo(), true, OnActionEffectEditorCompleteHandler);
    }
    private void OnAddHitEffectHandler()
    {
        AddEffectSetting.Instance.Show("添加受击特效", new EffectInfo(), true, OnHitEffectEditorCompleteHandler);
    }
    
}
