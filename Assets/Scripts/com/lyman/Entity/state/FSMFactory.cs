using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMFactory
{
    public static FSMStateMachine GetCharacterEditorFSM(Creature owner)
    {
        FSMTranslationMap translationMap = new FSMTranslationMap();
        DeadState dead = new DeadState(owner);
        AttackState attackState = new AttackState(owner);
        IdleState idle = new IdleState(owner);
        WalkState walk = new WalkState(owner);
        HitState hitState = new HitState(owner);
        RunState runState = new RunState(owner);
        HitMoveState hitMoveState = new HitMoveState(owner);
        HitFlyState hitFlyState = new HitFlyState(owner);
        translationMap.addState(dead);
        translationMap.addState(idle);
        translationMap.addState(walk);
        translationMap.addState(runState);
        translationMap.addState(hitState);
        translationMap.addState(hitMoveState);
        translationMap.addState(attackState);
        translationMap.addState(hitFlyState);
        translationMap.MixAll();
        translationMap.check();
        FSMStateMachine fsm = new FSMStateMachine(translationMap);
        return fsm;
    }

    

}
