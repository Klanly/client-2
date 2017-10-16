using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateChangeEvent : IEvent
{    
    public uint oldStateID;
    public uint newStateID;
    public static string sceID = "StateChange";
    public StateChangeEvent(uint oldStateID, uint newStateID)
    {
        ID = sceID;        
        this.oldStateID = oldStateID;
        this.newStateID = newStateID;
    }
}
//有限状态机
public class FSMStateMachine
{
    private uint nextStateID=uint.MaxValue;
    private FSMTranslationMap map;
    private uint maxTranslations = 0;
    private FSMState currentState;
    private FSMState previousState;
    protected EventDispatcher eventDispatcher = new EventDispatcher(); 
    public FSMState CurrentState { get { return currentState; } }
    public FSMState PreviousState { get { return previousState; } }
    public FSMTranslationMap Map { get { return map; } }
    public uint MaxTranslations
    {
        get { return maxTranslations; }
        set { maxTranslations = value; }
    }

    public void Destroy()
    {
        if (this.map != null)
            this.map.Destroy();
    }

    public FSMStateMachine(FSMTranslationMap map)
    {
        this.map = map;        
        this.previousState = null;
        this.currentState = map.StartState;
        this.currentState.OnEnter(null);
        this.eventDispatcher.registerEvent(StateChangeEvent.sceID, typeof(StateChangeEvent));
    }

    public bool bindStateChangeHandler(EventHandler handler)
    {
        return eventDispatcher.bindHandler(StateChangeEvent.sceID, handler);
    }

    public void unBindStateChangehandler(EventHandler handler)
    {
        eventDispatcher.unbindHandler(StateChangeEvent.sceID, handler);
    }   
    public bool changeState(uint stateID)
    {
		if(stateID!=currentState.ID && map.getNextTranslate(currentState, stateID) != null)
        {
            nextStateID = stateID;
            return true;
        }
        return false;
    }
    public void update(float deltaTime)
    {
        if (nextStateID!=uint.MaxValue && nextStateID != currentState.ID)
        {            
            doChangeState(nextStateID);
        }                
        currentState.Update(deltaTime);        
    }
    private void doChangeState(uint stateID)
    {
        FSMTranslation translation = map.getNextTranslate(currentState, stateID);
        if (translation != null)
        {
            FSMState nextState = translation.Output;
            if (nextState == currentState)
            {
                return;
            }
            FSMState commonParent = currentState.getLeastCommonParent(nextState);
            FSMState exitState = currentState;
            while (exitState != null && exitState != commonParent)
            {                        
                exitState.OnLeave(translation);
                exitState = exitState.Parent;
            }

            if (translation.Action != null)
            {
                translation.Action(translation);
            }

            FSMState[] enterStates = nextState.Hierarchy;
            int startIndex = 0;
            if (commonParent != null)
            {
                for(int i = 0; i< enterStates.Length;i++)
                {
                    FSMState state = enterStates[i];
                    if (state == commonParent)
                    {
                        startIndex++;
                        break;
                    }
                    startIndex++;
                }
            }

            for (int j = startIndex; j < enterStates.Length; j++)
            {
                enterStates[j].OnEnter(translation);
            }

            previousState = currentState;
            currentState = nextState;

            StateChangeEvent sce = new StateChangeEvent(previousState.ID, currentState.ID);
            eventDispatcher.post(sce);
        }
    }
    public FSMState getStateByID(uint id)
    {
        return map.getStateByID(id);
    }
    public FSMState getStateByName(string name)
    {
        return map.getStateByName(name);
    }
}