using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AttackState : FSMState
{
    private Creature creature;
    
    private Creature target;

    private bool isEntry;

    private ActionInfo actionInfo;
    private TimerInfo timerInfo;
    private float speed;
    private float delTime;
    

    public ActionInfo SetActionInfo
    {
        set { actionInfo = value; }
    }

    public Creature Target
    {
        get { return target;}
        set { target = value; }
    }


    public AttackState(Creature creature) : base("Attack", (uint)(CreatureStateType.Attack))
    {
        this.creature = creature;
    }
    
    private void onPlayEndHandler()
    {
        creature.DoIdle();
    }
    
    private void OnPlayHitHandler()
    {
        if (Target == null) return;
        if (actionInfo != null && AnimationType.IsAttackAction(actionInfo.ActionName))
        {
            if (!actionInfo.IsLangAttack)
            {
                if (actionInfo.IsHitMove && actionInfo.HitMoveDistance > 0f && actionInfo.HitMoveTime > 0f)
                {
                    target.DoHitMove(creature.GetPosition(), actionInfo.HitMoveDistance, actionInfo.HitMoveTime);
                }
                else if (actionInfo.IsHitFly && actionInfo.HitFlyDistance > 0f && actionInfo.HitFlyTime > 0f)
                {
                    target.DoHitFly(creature.GetPosition(), actionInfo.HitFlyDistance, actionInfo.HitFlyTime);
                }
                else
                {
                    target.DoHit();
                }
            }
            else
            {

            }
        }
    }
    
    public override void OnLeave(FSMTranslation translation)
    {
        base.OnLeave(translation);
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        isEntry = false;
    }
    
    public override void OnEnter(FSMTranslation translation)
    {
        base.OnEnter(translation);
        delTime = 0f;
        if (actionInfo.SelfMoveDistance > 0f && actionInfo.SelfMoveTime > 0f)
        {
            speed = actionInfo.SelfMoveDistance / actionInfo.SelfMoveTime;
            if (actionInfo.SelfMoveDelayTime > 0f)
            {
                timerInfo = TimerManager.AddDelayHandler(OnDelayHandler, actionInfo.SelfMoveDelayTime, 1);
            }
            else
            {
                isEntry = true;
            }
        }
        creature.PlayAnimation(actionInfo.ActionName, true, OnPlayHitHandler, onPlayEndHandler);
        
    }

    private void OnDelayHandler(float del)
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        isEntry = true;
    }

    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (!isEntry) { return; }
        delTime += deltaTime;
        if (delTime <= actionInfo.SelfMoveDistance)
        {
            Vector3 step = creature.Container.transform.TransformDirection(speed * Vector3.forward);
            creature.CharacterController.SimpleMove(step);
        }
        else
        {
            isEntry = false;
            creature.DoIdle();
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
    }
}
