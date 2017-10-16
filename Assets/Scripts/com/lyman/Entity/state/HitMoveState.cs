using System;
using System.Collections.Generic;
using UnityEngine;

public class HitMoveState : FSMState
{
    public Creature AnAtt;
    private Creature creature;
    private float moveDistance;
    private float moveTime;
    private CharacterController characterController;
    private float speed;
    private float curTime;

    public float MoveDistance
    {
        get { return moveDistance; }
        set { moveDistance = value; }
    }
    public float MoveTime
    {
        get { return moveTime; }
        set { moveTime = value; }
    }

    public HitMoveState(Creature creature):base("Hit_Move",CreatureStateType.HitMove)
    {
        this.creature = creature;
    }
   
    public override void OnEnter(FSMTranslation translatioin)
    {
        ReStart();
    }
    public void Stop()
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
    }

    public override void Destroy()
    {
        base.Destroy();
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
    }

    public void ReStart()
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        creature.FaceTo(AnAtt.GetPosition());
        creature.PlayAnimation(AnimationType.Hit,true,null, OnEndHandler);
        speed = MoveDistance / MoveTime;
        curTime = 0f;
        timerInfo = TimerManager.AddHandler(OnTimeHandler);
    }
    private TimerInfo timerInfo;
    private void OnEndHandler()
    {
        if (creature.IsDead)
        {
           // creature.DoDead();
        }
        else
        {
            //creature.DoIdle();
        }
    }
    private void OnTimeHandler(float delay)
    {
        curTime += delay;
        if (curTime >= MoveTime)
        {
            TimerManager.RemoveHandler(timerInfo);
            timerInfo = null;
        }
        else
        {
            Vector3 targetPosition = creature.Model.Container.transform.TransformDirection(speed * Vector3.back);
            characterController.SimpleMove(targetPosition);
        }
    }


    public override void OnLeave(FSMTranslation translatioin)
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
    }
}

