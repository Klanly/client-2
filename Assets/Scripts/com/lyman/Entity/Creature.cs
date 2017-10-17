using System;
using System.Collections.Generic;
using UnityEngine;


public class Creature : SceneEntity
{
    public FSMStateMachine FSM;
    protected string modelName;
    private string currentActionName;
    protected CharacterConfigInfo characterConfigInfo;
    private ActionInfo currentActionInfo;
    private Action onCreateComplete;
    private CharacterController characterController;
    protected bool IsCreate;
    protected string nickName;
    protected int currentHp;
    protected int totalHp;
    protected bool isDead;
    private float walkSpeed = 1f;
    private float runSpeed = 1f;
    private Dictionary<int, NEffect> effectLsit = new Dictionary<int, NEffect>();
    private List<NEffect> nEffectCache = new List<NEffect>();
    private AudioSource audioSource;
    private Action OnActionEndHandler;
    private TimerInfo timerInfo;

    public Creature()
    {

    }

    public CharacterController CharacterController
    {
        get { return characterController; }
    }


    public float WalkSpeed
    {
        get { return walkSpeed; }
        set
        {
            if (value < 0) value = 0;
            walkSpeed = value;
        }
    }
    public float RunSpeed
    {
        get { return runSpeed; }
        set
        {
            if (value < 0) value = 0;
            runSpeed = value;
        }
    }

    public virtual int CurrentHp
    {
        get { return currentHp; }
        set
        {
            if (value < 0) value = 0;
            currentHp = value;
        }
    }
    
    public int TotalHp
    {
        get
        {
            return totalHp;
        }
        set
        {
            if (value < 0) value = 0;
            totalHp = value;
        }
    }
    
    public bool IsDead
    {
        set
        {
            isDead = value;
        }
        get { return isDead; }
    }
    
    public override void SetScale(float x, float y, float z)
    {
        base.SetScale(x,y,z);
        if (characterController != null)
        {
            characterController.height *= y;
            characterController.center = new Vector3(0f, 1f * y, 0f);
        }
    }
    
    public CharacterConfigInfo GetCharacterConfigInfo()
    {
        return characterConfigInfo;
    }
    
    public float GetAttackRange(string actionName)
    {
        ActionInfo actionInfo = characterConfigInfo.GetActionInfo(actionName);
        return actionInfo.AttackRadius;
    }

    //public bool GetIsLongAttack(string actionn)
    //{
    //    ActionInfo actionInfo = characterConfigInfo.GetActionInfo(actionn);
    //    return actionInfo.IsLong;
    //}


    public float GetAttackAngle(string actionName)
    {
        ActionInfo actionInfo = characterConfigInfo.GetActionInfo(actionName);
        return actionInfo.AttackAngle;
    }

    
    public virtual void Init(CharacterConfigInfo cInfos,Action createComplete = null)
    {
        IsCreate = false;
        onCreateComplete = createComplete;
        characterConfigInfo = cInfos;
        if (Model != null)
        {
            Model.Destroy();
            Model = null;
        }
        Model = new CommonModel();
        modelName = characterConfigInfo.ModelName;
        Model.Init(GameConst.CharacterModelABDirectory + modelName, modelName, OnCreateCompleteHandler);
    }

    public float GetCurrentActionLength()
    {
        if (Model != null && IsCreate)
        {
            return Model.GetCurrentActionLength();
        }
        return 0f;
    }


    private void OnCreateCompleteHandler()
    {

        characterController = Model.Container.GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = Model.Container.AddComponent<CharacterController>();
        }

        if (onCreateComplete != null)
        {
            onCreateComplete();
        }
        IsCreate = true;
        if (!string.IsNullOrEmpty(characterConfigInfo.LeftWeaponName) && characterConfigInfo.LeftWeaponName != BindTypes.NONE)
        {
            WearEquipment("added_leftweapon",characterConfigInfo.LeftWeaponName, BoneTypes.Bone_Lwq);
        }
        if (!string.IsNullOrEmpty(characterConfigInfo.RightWeaponName) && characterConfigInfo.RightWeaponName != BindTypes.NONE)
        {
            WearEquipment("added_rightweapon",characterConfigInfo.RightWeaponName, BoneTypes.Bone_Rwq);
        }
    }
    
    public void WearEquipment(string saveName,string equipmentName, string parentName)
    {
        if (!IsCreate)
        {
            return;
        }
        GameObject equipment = ResourceManager.GetGameObjectInstance(GameConst.EquipmentModelABDirectory + equipmentName, equipmentName);
        Model.AddChild(saveName, equipment, parentName);
    }





    //public Transform GetChild(string cn)
    //{
    //    if (Model == null)
    //    {
    //        return null;
    //    }
    //    if (Model.Container == null)
    //    {
    //        return null;
    //    }
    //    return Model.GetChild(cn);
    //}

    //public void AddChild(GameObject child, string bonen, bool destroyOldChild = false)
    //{
    //    if (Model != null)
    //    {
    //        Model.AddChild(child, bonen, destroyOldChild);
    //    }
    //}

    public void StopOldActionEffect(string actionName)
    {
        ActionInfo actionInfo = characterConfigInfo.GetActionInfo(actionName);
        if (actionInfo == null) return;
        for (int i = 0; i < actionInfo.ActionEffectInfos.Count; ++i)
        {
            EffectInfo effectInfo = actionInfo.ActionEffectInfos[i];
            NEffect nEffect = null;
            effectLsit.TryGetValue(effectInfo.Index, out nEffect);
            if (nEffect != null)
            {
                nEffect.Stop();
            }
        }
    }
    private void OnEndHandler(float del)
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        if (OnActionEndHandler != null)
        {
            OnActionEndHandler();
        }
    }
    
    
    public void PlayActionSound(string soundName, bool loop = false, ulong delayTime = 0ul)
    {
        if (string.IsNullOrEmpty(soundName) || soundName == BindTypes.NONE)
        {
            return;
        }
        if (Container == null)
            return;
        if(audioSource == null)
            audioSource = Model.Container.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = Model.Container.AddComponent<AudioSource>();
        }
        if (audioSource != null)
        {
            AudioClip audioClip = ResourceManager.GetAudioClip(GameConst.SoundABDirectory + soundName, soundName);
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.loop = loop;
                audioSource.PlayDelayed(delayTime);
            }
        }
    }

    private void StopActionSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    
    private NEffect GetEffect(EffectInfo effectInfo)
    {
        NEffect nEffect = null;
        effectLsit.TryGetValue(effectInfo.Index, out nEffect);
        if (nEffect == null)
        {
            nEffect = new NEffect();
            effectLsit.Add(effectInfo.Index, nEffect);
            nEffectCache.Add(nEffect);
        }
        nEffect.Init(effectInfo.EffectName, effectInfo.EffectName, effectInfo.SoundName, effectInfo.SoundPlayDelayTime, effectInfo.IsLoop);
        return nEffect;
    }
    
    public void PlayAnimation(string actionName, bool isClearOldActionInfo = true, Action hitHandler = null,Action endHandler = null)
    {
        if (currentActionName == actionName) return;
        StopActionSound();
        if (isClearOldActionInfo)
        {
            StopOldActionEffect(currentActionName);
        }
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        OnActionEndHandler = null;
        if (Model != null && characterConfigInfo != null)
        {
            currentActionName = actionName;
            currentActionInfo = characterConfigInfo.GetActionInfo(currentActionName);
            if (currentActionInfo != null)
            {
                bool loop = AnimationType.IsLoopAction(currentActionName);
                if (currentActionInfo.IsSelfMove && currentActionInfo.SelfMoveTime > 0f && currentActionInfo.SelfMoveDistance > 0f && currentActionInfo.IsLoop)
                {
                    OnActionEndHandler = endHandler;
                    timerInfo = TimerManager.AddDelayHandler(OnEndHandler, currentActionInfo.SelfMoveTime, 1);
                    Model.PlayAnimation(currentActionName, currentActionInfo.PlaySpeed, hitHandler, null);
                }
                else
                {
                    Model.PlayAnimation(currentActionName, currentActionInfo.PlaySpeed, hitHandler, endHandler);
                }
                PlayActionSound(currentActionInfo.SoundName, loop, (ulong)currentActionInfo.SoundPlayDelayTime);
                for (int i = 0; i < currentActionInfo.ActionEffectInfos.Count; ++i)
                {
                    EffectInfo effectInfo = currentActionInfo.ActionEffectInfos[i];
                    if (effectInfo.EffectType == EffectTypes.Bullet)
                    {
                        continue;
                    }
                    if (effectInfo.EffectType == EffectTypes.Normal && effectInfo.BindType == BindTypes.Bone && (effectInfo.BindName == BindTypes.NONE || string.IsNullOrEmpty(effectInfo.BindName)))
                    {
                        continue;
                    } 
                    
                    NEffect nEffect = GetEffect(effectInfo);
                    GameObject effect = nEffect.GetEffect();
                    if (effect == null) continue;
                    if (effectInfo.BindType == BindTypes.OrginePoint)
                    {
                        //自身同坐标，且随自身一起移动
                        if (Container != null)
                        {
                            effect.transform.SetParent(Container.transform);
                            effect.transform.localPosition = Vector3.zero;
                            effect.transform.localScale = Vector3.one;
                            effect.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if (effectInfo.BindType == BindTypes.Self_Point)
                    {
                        //自身同坐标，且不随自身一起移动
                        if (Container != null)
                        {
                            effect.transform.SetParent(null);
                            effect.transform.localPosition = Vector3.zero;
                            effect.transform.localScale = Vector3.one;
                            effect.transform.localRotation = Quaternion.identity;
                            effect.transform.position = GetPosition();
                            effect.transform.eulerAngles = new Vector3(0f, Container.transform.eulerAngles.y, 0f);
                        }
                    }
                    else if (effectInfo.BindType == BindTypes.Bone)
                    {
                        //绑骨骼
                        Transform bone = Model.GetChild(effectInfo.BindName);
                        if (bone != null)
                        {
                            effect.transform.SetParent(bone);
                            effect.transform.localPosition = Vector3.zero;
                            effect.transform.localScale = Vector3.one;
                            effect.transform.localRotation = Quaternion.identity;
                        }
                    }
                    nEffect.Play();
                }
            }
        }
    }
    
    //public void PlayAnimation(string actName, bool isClearOldEffect = true, Action hitHandler = null, Action endHandler = null)
    //{
    //    if (currentActionName == actName) return;
    //    OnActionEndHandler = null;
    //    TimerManager.RemoveHandler(timerInfo);
    //    HideCEffect();
    //    if (isClearOldEffect)
    //    {
    //        ClearOldE();
    //    }
    //    if (Model != null && characterConfigInfo != null)
    //    {
    //        currentActionName = actName;
    //        currentActionInfo = characterConfigInfo.GetActionInfo(currentActionName);
    //        StopActionSound();
    //        if (currentActionInfo.IsSelfMove && currentActionInfo.SelfMoveTime > 0f && currentActionInfo.IsLoop)
    //        {
    //            OnActionEndHandler = endHandler;
    //            timerInfo = TimerManager.AddDelayHandler(OnEndHandler, currentActionInfo.SelfMoveTime, 1);
    //            Model.PlayAnimation(currentActionName, currentActionInfo.PlaySpeed, hitHandler, null);
    //        }
    //        else
    //        {
    //            Model.PlayAnimation(currentActionName, currentActionInfo.PlaySpeed, hitHandler, endHandler);
    //        }
    //        if (currentActionInfo != null)
    //        {
    //            if (currentActionInfo.SoundName != BindTypes.NONE && !string.IsNullOrEmpty( currentActionInfo.SoundName))
    //            {
    //                PlayActionSound(currentActionInfo.SoundName, AnimationType.IsLoopAction(currentActionName), (ulong)(currentActionInfo.SoundPlayDelayTime < 0f ? 0 : currentActionInfo.SoundPlayDelayTime));
    //            }
    //            if (currentActionName == AnimationType.Dead)
    //            {
    //                return;
    //            }
    //            //for (int i = 0; i < currentActionInfo.ActionEffectInfos.Count; i++)
    //            //{
    //            //    EffectInfo effectInfo = currentActionInfo.ActionEffectInfos[i];
    //            //    if (effectInfo.IsFlyEffect || effectInfo.BindType == BindTypes.None)
    //            //        continue;
    //            //    NEffect effect;
    //            //    if (effectInfo.BindType == BindTypes.OrginePoint)
    //            //    {
    //            //        bool loop = AnimationType.IsLoopAction(currentActionInfo.Actionn);
    //            //        string key = effectInfo.Effectn + "_" + effectInfo.Soundn + "_" + (loop ? "1" : "0");
    //            //        effectCacheList.TryGetValue(key, out effect);
    //            //        if (effect == null)
    //            //        {
    //            //            effect = new NEffect();
    //            //            effect.Init(effectInfo.Effectn, effectInfo.Soundn, loop, OnPlayEndHandler);
    //            //        }
    //            //        else
    //            //        {
    //            //            effectCacheList.Remove(key);
    //            //        }
    //            //        if (effect.GetEffect() != null)
    //            //        {
    //            //            effect.GetEffect().transform.SetParent(Model.Container.transform);
    //            //            effect.GetEffect().transform.localPosition = Vector3.zero;
    //            //            effect.GetEffect().transform.localScale = Vector3.one;
    //            //            effect.GetEffect().transform.localRotation = Quaternion.identity;
    //            //            effect.Play();
    //            //        }
    //            //        actionEffect.Add(effect);
    //            //    }
    //            //    else if (effectInfo.BindType == BindTypes.Bone)
    //            //    {
    //            //        string key = effectInfo.Effectn + "_" + effectInfo.Soundn + "_0";
    //            //        effectCacheList.TryGetValue(key, out effect);
    //            //        if (effect == null)
    //            //        {
    //            //            effect = new NEffect();
    //            //            effect.Init(effectInfo.Effectn, effectInfo.Soundn, false, OnPlayEndHandler);
    //            //        }
    //            //        else
    //            //        {
    //            //            effectCacheList.Remove(key);
    //            //        }
    //            //        if (effectInfo.Bindn != null && effectInfo.Bindn != String.Empty && effectInfo.Bindn != BindTypes.NONE)
    //            //        {
    //            //            Model.AddChild(effect.GetEffect(), effectInfo.Bindn, false);
    //            //        }
    //            //        effect.Play();
    //            //        actionEffect.Add(effect);
    //            //    }
    //            //    else if (effectInfo.BindType == BindTypes.Weapone)
    //            //    {
    //            //        if (effectInfo.Bindn != null && effectInfo.Bindn != String.Empty && effectInfo.Bindn != BindTypes.NONE)
    //            //        {
    //            //            GameObject go;
    //            //            acEffects.TryGetValue(effectInfo.Effectn, out go);
    //            //            if (go == null)
    //            //            {
    //            //                effect = new NEffect();
    //            //                effect.Init(effectInfo.Effectn, effectInfo.Soundn);
    //            //                go = effect.GetEffect();
    //            //                Model.AddChild(go, effectInfo.Bindn, false);

    //            //                if (acEffects.ContainsKey(effectInfo.Effectn))
    //            //                {
    //            //                    acEffects[effectInfo.Effectn] = go;
    //            //                }
    //            //                else
    //            //                {
    //            //                    acEffects.Add(effectInfo.Effectn, go);
    //            //                }
    //            //            }
    //            //            go.SetActive(true);
    //            //        }
    //            //    }
    //            //    else if (effectInfo.BindType == BindTypes.Self_Point)
    //            //    {

    //            //        string key = effectInfo.Effectn + "_" + effectInfo.Soundn + "_0";
    //            //        effectCacheList.TryGetValue(key, out effect);
    //            //        if (effect == null)
    //            //        {
    //            //            effect = new NEffect();
    //            //            effect.Init(effectInfo.Effectn, effectInfo.Soundn, false, OnPlayEndHandler);
    //            //        }
    //            //        else
    //            //        {
    //            //            effectCacheList.Remove(key);
    //            //        }
    //            //        effect.GetEffect().transform.SetParent(EntityManager.EntityContainer.transform);
    //            //        effect.GetEffect().transform.localPosition = Vector3.zero;
    //            //        effect.GetEffect().transform.localScale = Vector3.one;
    //            //        effect.GetEffect().transform.localRotation = Quaternion.identity;
    //            //        effect.GetEffect().transform.position = GetPosition();
    //            //        effect.GetEffect().transform.eulerAngles = new Vector3(0f, Model.Container.transform.eulerAngles.y, 0f);
    //            //        effect.Play();
    //            //        actionEffect.Add(effect);
    //            //    }
    //            //}
    //        }
    //    }
    //}
    

    public GameObject Container
    {
        get
        {
            if (Model != null && Model.Container != null)
                return Model.Container;
            return null;
        }
    }
    
    public override void Destroy()
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        for (int i = 0; i < nEffectCache.Count; ++i)
        {
            NEffect nEffect = nEffectCache[i];
            nEffect.Stop();
        }
        nEffectCache.Clear();
        effectLsit.Clear();
        if (FSM != null)
        {
            FSM.Destroy();
            FSM = null;
        }
        base.Destroy();
    }
    
    public override void Update(float delTime)
    {
        base.Update(delTime);
        if (FSM != null)
        {
            FSM.update(delTime);
        }
        if(Model != null)
            Model.Update();
    }

    public void DoIdle()
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID != CreatureStateType.Idle)
            {
                FSM.changeState(CreatureStateType.Idle);
            }
        }
    }

    public void DoDead()
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID != CreatureStateType.Dead)
            {
                FSM.changeState(CreatureStateType.Dead);
            }
        }
    }

    public void DoWalk(Vector3 position)
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID != CreatureStateType.Dead)
            {
                WalkState walkState = (WalkState)FSM.getStateByID(CreatureStateType.Walk);
                if (walkState != null)
                {
                    walkState.TargetPosition = position;
                    if (FSM.CurrentState.ID != CreatureStateType.Walk)
                    {
                        FSM.changeState(CreatureStateType.Walk);
                    }
                }
            }
            else
            {
                //死亡不可以移动
            }
        }
    }


    public void DoRun(Vector3 position)
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID != CreatureStateType.Dead)
            {
                RunState runState = (RunState)FSM.getStateByID(CreatureStateType.Run);
                if (runState != null)
                {
                    runState.TargetPosition = position;
                    if (FSM.CurrentState.ID != CreatureStateType.Run)
                    {
                        FSM.changeState(CreatureStateType.Run);
                    }
                }
            }
            else
            {
                //死亡不可以移动
            }
        }
    }
    public void DoHit()
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID == CreatureStateType.Dead)
            {
                return;
            }
            if (FSM.CurrentState.ID != CreatureStateType.Hit && FSM.CurrentState.ID != CreatureStateType.HitFly && FSM.CurrentState.ID != CreatureStateType.HitMove && FSM.CurrentState.ID != CreatureStateType.Attack)
            {
                FSM.changeState(CreatureStateType.Hit);
            }
        }
    }

    public void DoHitMove(float moveDistance, float moveTime)
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID == CreatureStateType.Dead)
            {
                return;
            }
            if (FSM.CurrentState.ID != CreatureStateType.Hit && FSM.CurrentState.ID != CreatureStateType.HitFly && FSM.CurrentState.ID != CreatureStateType.Attack)
            {
                FSM.changeState(CreatureStateType.HitMove);
            }
        }
    }

    public void DoHitFly(float moveDistance, float moveTime)
    {
        if (FSM != null)
        {
            if (FSM.CurrentState.ID == CreatureStateType.Dead)
            {
                return;
            }
            if (FSM.CurrentState.ID != CreatureStateType.Hit && FSM.CurrentState.ID != CreatureStateType.HitMove && FSM.CurrentState.ID != CreatureStateType.Attack)
            {
                FSM.changeState(CreatureStateType.HitFly);
            }
        }
    }

}
