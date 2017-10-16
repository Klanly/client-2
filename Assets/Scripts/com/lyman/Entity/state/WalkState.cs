using System;
using System.Collections.Generic;
using UnityEngine;
public class WalkState : FSMState
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

    public WalkState(Creature creature) : base("WALK", (uint)(CreatureStateType.Walk))
    {
        this.creature = creature;
    }
    
    public override void OnEnter(FSMTranslation translation)
    {
        isEntry = true;
        creature.PlayAnimation(AnimationType.Walk);
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
            if (Vector3.Distance(targetPosition, creature.GetPosition()) <= creature.WalkSpeed * 0.5f)
            {
                creature.DoIdle();
            }
            else
            {
                Vector3 targetPosition = creature.Container.transform.TransformDirection(Vector3.forward * creature.WalkSpeed);
                characterController.SimpleMove(targetPosition);
            }
        }
    }
}