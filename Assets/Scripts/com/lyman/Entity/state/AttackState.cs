using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AttackState : FSMState
{
    private Creature creature;
    private int skillId;
    private string targetId = string.Empty;
    
    public AttackState(Creature creature) : base("Attack", (uint)(CreatureStateType.Attack))
    {
        this.creature = creature;
    }
    
    private void onPlayEndHandler()
    {
       
    }
    
    private void OnPlayHitHandler()
    {
        
    }
    
    public override void OnLeave(FSMTranslation translation)
    {
        base.OnLeave(translation);
    }
    
    public override void OnEnter(FSMTranslation translation)
    {
        base.OnEnter(translation);
        
    }
    


    private void exec(bool clearOld)
    {
       
    }
    public override void Destroy()
    {
        base.Destroy();
    }
}
