using System;
using System.Collections.Generic;
using UnityEngine;

public class RunState : FSMState
{
    private Creature creature;
    private bool isEntry;

    private Vector3 targetPosition;

    private CharacterController characterController;

    public Vector3 TargetPosition
    {
        set
        {
            targetPosition = value;
            creature.FaceTo(targetPosition);
        }
    }


    public RunState(Creature creature)
        : base("RUN", (uint)(CreatureStateType.Run))
    {
        this.creature = creature;
    }

    public override void OnEnter(FSMTranslation translatioin)
    {
        isEntry = true;
        creature.PlayAnimation(AnimationType.Run);
    }



    public override void OnLeave(FSMTranslation translation)
    {
        base.OnLeave(translation);
        isEntry = false;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (isEntry)
        {
            if (Vector3.Distance(targetPosition, creature.GetPosition()) <= creature.RunSpeed * 0.5f)
            {
                creature.DoIdle();
            }
            else
            {
                Vector3 targetPosition = creature.Container.transform.TransformDirection(Vector3.forward * creature.RunSpeed);
                characterController.SimpleMove(targetPosition);
            }
        }
    }



}
