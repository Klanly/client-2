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
            tCreature.PlayAnimation(actionName, true);
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
            if (currentActionInfo.ActionName != AnimationType.Idle)
            {
                tTimerInfo = TimerManager.AddDelayHandler(OnDelHandler, 2f, 1);
                tCreature.PlayAnimation(AnimationType.Idle, true);
            }
        }
    }
    private TimerInfo tTimerInfo;
    private void OnDelHandler(float del)
    {
        Alert.Hide();
        TimerManager.RemoveHandler(tTimerInfo);
        tTimerInfo = null;
        beAttacker.Show();
        Vector3 pos = tCreature.Container.transform.TransformDirection(currentActionInfo.AttackRadius * Vector3.forward);
        beAttacker.SetPosition(pos.x, pos.y, pos.z);
        beAttacker.FaceTo(tCreature.GetPosition());
        tCreature.PlayAnimation(currentActionInfo.ActionName, true, this.OnHitHandler, OnEndHandler);
    }

    private void OnHitHandler()
    {
        if (currentActionInfo != null && AnimationType.IsAttackAction(currentActionInfo.ActionName))
        {

            if (currentActionInfo.IsHitMove && currentActionInfo.HitMoveDistance > 0f && currentActionInfo.HitMoveTime > 0f)
            {
                beAttacker.DoHitMove(tCreature.GetPosition(), currentActionInfo.HitMoveDistance, currentActionInfo.HitMoveTime);
            }
            else if (currentActionInfo.IsHitFly && currentActionInfo.HitFlyDistance > 0f && currentActionInfo.HitFlyTime > 0f)
            {
                beAttacker.DoHitFly(tCreature.GetPosition(), currentActionInfo.HitMoveDistance, currentActionInfo.HitMoveTime);
            }
            else
            {
                beAttacker.DoHit();
            }
        }
    }

    private void OnEndHandler()
    {
        if (currentActionInfo != null && AnimationType.IsAttackAction(currentActionInfo.ActionName))
        {

        }
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
