using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditor : MonoBehaviour {

    private ModelList modelList;
    void Start()
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
    private TCreature beAttacker;

    private void OnModelSelectedHandler(string abn)
    {
        //to do
        CharacterConfigInfo characterConfigInfo = DataManager.GetCharacterConfigInfo(abn);
        if (tCreature == null)
            tCreature = new TCreature();
        tCreature.Init(characterConfigInfo);

        if (beAttacker == null)
        {
            beAttacker = new TCreature();
            beAttacker.Init(tCreature.GetCharacterConfigInfo());
        }
        beAttacker.Hide();


        AttributeSetting.Instance.StartSetting(characterConfigInfo);
        EditorCameraController.Instance.Target = tCreature.Container;

        AttributeSetting.Instance.OnWearWeaponHandler = ShowWeapon;
        AttributeSetting.Instance.OnActionSelectedHandler = OnActionSelectedHandler;
        AttributeSetting.Instance.GetActionLength = GetActionLength;
        AttributeSetting.Instance.PreviewHandler = PreviewHandler;
    }

    private void ShowWeapon(string name, string weaponName, string parentName)
    {
        if (tCreature != null)
        {
            tCreature.WearEquipment(name, weaponName, parentName);
        }
    }
    private void OnActionSelectedHandler(string actionName)
    {
        if (tCreature != null)
        {
            tCreature.PlayAnimation(actionName, true, null, OnEndHandler);
        }
    }

    private float GetActionLength()
    {
        if(tCreature != null)
            return tCreature.GetCurrentActionLength();
        return 0f;
    }

    private CharacterConfigInfo currentCharacterConfigInfo;
    private ActionInfo currentActionInfo;
    private void PreviewHandler(CharacterConfigInfo characterConfigInfo, ActionInfo actionInfo)
    {
        Debug.LogError("start preview");
        Alert.Show("预览准备中，请稍等......");
        currentCharacterConfigInfo = characterConfigInfo;
        currentActionInfo = actionInfo;
        if (currentCharacterConfigInfo != null && currentActionInfo != null && tCreature != null)
        {
            bool isCorrect = CheckActionInfo(currentActionInfo);
            if (!isCorrect) return;

            if (currentActionInfo.ActionName != AnimationType.Idle)
            {
                tTimerInfo = TimerManager.AddDelayHandler(OnDelHandler, 2f, 1);
                tCreature.PlayAnimation(AnimationType.Idle, true);
            }
        }
    }

    private bool CheckActionInfo(ActionInfo actionInfo)
    {
        if (!actionInfo.IsLoop)
        {
            if (!string.IsNullOrEmpty(actionInfo.SoundName) && actionInfo.SoundName != BindTypes.NONE)
            {
                if (actionInfo.SoundPlayDelayTime > actionInfo.Length)
                {
                    Alert.Show(actionInfo.ActionName + "的动作音效的延迟播放时间不能大于动作自身时间!!!", 5);
                    return false;
                }
            }
            if (actionInfo.SelfMoveDistance > 0f && actionInfo.SelfMoveTime > 0f)
            {
                if (actionInfo.SelfMoveDelayTime + actionInfo.SelfMoveTime > actionInfo.Length)
                {
                    Alert.Show(actionInfo.ActionName + "为非循环动作,自身位移延迟时间+自身位移时间不能大于动作自身时间!!!", 5);
                    return false;
                }
            }
        }
        else
        {

        }


        if (actionInfo.IsHitFly && actionInfo.IsHitMove)
        {
            Alert.Show(actionInfo.ActionName + "动作不能同时击飞和击退!!!", 5);
            return false;
        }

        if (actionInfo.IsHitMove && (actionInfo.HitMoveDistance <= 0f || actionInfo.HitMoveTime <= 0f))
        {
            Alert.Show(actionInfo.ActionName + "带击退功能的动作,击退时间和击退距离都得大于0!!!", 5);
            return false;
        }
        if (actionInfo.IsHitFly && (actionInfo.HitFlyDistance <= 0f || actionInfo.HitFlyTime <= 0f))
        {
            Alert.Show(actionInfo.ActionName + "带击飞功能的动作,击飞时间和击飞距离都得大于0!!!", 5);
            return false;
        }


        return true;
    }


    private TimerInfo tTimerInfo;
    private void OnDelHandler(float del)
    {
        Alert.Hide();
        TimerManager.RemoveHandler(tTimerInfo);
        tTimerInfo = null;
        tCreature.SetPosition(0f, 0f, 0f);
        Vector3 pos = tCreature.Container.transform.position + tCreature.Container.transform.TransformDirection(currentActionInfo.AttackRadius * Vector3.forward);
        beAttacker.SetPosition(pos.x, pos.y, pos.z);
        beAttacker.FaceTo(tCreature.GetPosition());
        if (AnimationType.IsAttackAction(currentActionInfo.ActionName))
        {
            beAttacker.Show();
            tCreature.DoAttack(beAttacker, currentActionInfo);
           // tCreature.PlayAnimation(currentActionInfo.ActionName, true, OnHitHandler, OnEndHandler);
        }
        else
        {
            beAttacker.Hide();
            tCreature.PlayAnimation(currentActionInfo.ActionName, true,null, OnEndHandler);
        }
    }

    //private void OnHitHandler()
    //{
    //    if (currentActionInfo != null && AnimationType.IsAttackAction(currentActionInfo.ActionName))
    //    {
    //        if (currentActionInfo.IsHitMove && currentActionInfo.HitMoveDistance > 0f && currentActionInfo.HitMoveTime > 0f)
    //        {
    //            beAttacker.DoHitMove(tCreature.GetPosition(), currentActionInfo.HitMoveDistance, currentActionInfo.HitMoveTime);
    //        }
    //        else if (currentActionInfo.IsHitFly && currentActionInfo.HitFlyDistance > 0f && currentActionInfo.HitFlyTime > 0f)
    //        {
    //            beAttacker.DoHitFly(tCreature.GetPosition(), currentActionInfo.HitMoveDistance, currentActionInfo.HitMoveTime);
    //        }
    //        else
    //        {
    //            beAttacker.DoHit();
    //        }
    //    }
    //}

    private void OnEndHandler()
    {
        tCreature.DoIdle();
    }

    void Update ()
    {
        TimerManager.Update(Time.deltaTime);
        if (tCreature != null)
        {
            tCreature.Update(Time.deltaTime);
        }
        if (beAttacker != null)
        {
            beAttacker.Update(Time.deltaTime);
        }
    }
}
