using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
    private Creature creature;
    
    public IdleState(Creature creature)
		: base("Idle", (uint)(CreatureStateType.Idle),FSMStateType.STATE_START)
    {
        this.creature = creature;
    }
    public override void OnEnter(FSMTranslation translatioin)
    {
        creature.PlayAnimation(AnimationType.Idle);
    }
}
