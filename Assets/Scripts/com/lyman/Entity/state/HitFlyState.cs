using System;
using System.Collections.Generic;
using UnityEngine;

public class HitFlyState : FSMState
{
    private Vector3 position;
    private Creature creature;
    private float flyDistance;
    private float flyTime;
    private TimerInfo timerInfo;

    private CharacterController characterController;
    private float speed;
    private float curTime;
    private List<string> actions = new List<string>();
    private int index;

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    public float FlyDistance
    {
        get { return flyDistance; }
        set { flyDistance = value; }
    }
    public float FlyTime
    {
        get { return flyTime; }
        set { flyTime = value; }
    }


    public HitFlyState(Creature creature) : base("Hit_Fly", CreatureStateType.HitFly)
    {
        this.creature = creature;
        actions.Add(AnimationType.Fly1);
        actions.Add(AnimationType.Fly2);
        actions.Add(AnimationType.Fly3);
    }

    public override void OnEnter(FSMTranslation translatioin)
    {
        
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        characterController = creature.CharacterController;
        index = 0;
        creature.FaceTo(position);
        creature.PlayAnimation(actions[index]);
        speed = FlyDistance / FlyTime;
        curTime = 0f;
        timerInfo = TimerManager.AddHandler(OnTimeHandler);
    }
    public override void Destroy()
    {
        base.Destroy();
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
    }

    private void OnTimeHandler(float delay)
    {

        curTime += delay;
        if (curTime >= FlyTime)
        {

            TimerManager.RemoveHandler(timerInfo);
            timerInfo = null;
            OnEndHandler();
        }
        else
        {
            Vector3 step = creature.Model.Container.transform.TransformDirection(speed * Vector3.back);
            characterController.SimpleMove(step);
        }
    }


    private void OnEndHandler()
    {
        index++;
        switch (index)
        {
            case 1:

                creature.PlayAnimation(actions[index], true, null, OnEndHandler);
                break;
            case 2:
                if (!creature.IsDead)
                {

                    creature.PlayAnimation(actions[index], true, null, OnEndHandler);
                }
                else
                {
                    //todo 击飞死亡后逻辑
                    creature.DoDead();
                }
                break;
            case 3:
                if (creature.IsDead)
                {

                    creature.DoDead();
                }
                else
                {

                    creature.DoIdle();
                }
                break;
        }
    }

    public override void OnLeave(FSMTranslation translatioin)
    {

        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        creature.DoIdle();
    }
}

