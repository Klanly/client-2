using System;
using System.Collections.Generic;
using UnityEngine;
public class DeadState : FSMState
{
    private Creature creature;
    
    public DeadState(Creature creature):base("Dead",CreatureStateType.Dead)
    {
        this.creature = creature;
    }
    public override void OnEnter(FSMTranslation translatioin)
    {
        creature.PlayAnimation(AnimationType.Dead,true,null,OnEndHandler);
    }
    

    public override void OnLeave(FSMTranslation translatioin)
    {
        
    }
    
    
   
    private void OnEndHandler()
    {
        
    }
    
    public override void Destroy()
    {
        
    }
}

