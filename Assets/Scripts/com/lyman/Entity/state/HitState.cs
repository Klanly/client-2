using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class HitState : FSMState
{
    private Creature creature;
    

    public HitState(Creature creature) : base("Hit", (uint)(CreatureStateType.Hit))
    {
        this.creature = creature;
    }
    
    public override void OnEnter(FSMTranslation translatioin)
    {
        creature.PlayAnimation(AnimationType.Hit,true, null, EndHandler);
        
    }
    
    private void EndHandler()
    {
        creature.DoIdle();
    }
}
