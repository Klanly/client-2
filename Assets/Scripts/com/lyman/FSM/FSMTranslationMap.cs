using System;
using System.Collections.Generic;

public class FSMTranslationMap
{
    private Dictionary<FSMState, FSMTranslationMatcher> translationMap = new Dictionary<FSMState, FSMTranslationMatcher>();

    private Dictionary<uint, FSMState> stateIDMap = new Dictionary<uint, FSMState>();

    private FSMState startState;

    private bool initialized = false;

    public FSMState StartState
    {
        get { return startState; }
    }

    

    public FSMTranslationMap()
    {

    }
    private FSMState[] allStates;
    public void MixAll()
    {
        allStates = new FSMState[translationMap.Count];
        translationMap.Keys.CopyTo(allStates, 0);
        for (int i  = 0; i < allStates.Length;i++)
        {
            FSMState form = allStates[i];
            for(int j = 0; j < allStates.Length; j++)
            {
                FSMState to = allStates[j];
                if (form.ID != to.ID)
                {
                    addTranslation(new FSMTranslation(form, to));
                }
            }
        }
    }

    public void Destroy()
    {
        if (allStates != null)
        {
            for (int i = 0; i < allStates.Length; i++)
            {
                FSMState state = allStates[i];
                state.Destroy();
            }
        }        
        translationMap.Clear();
    }

    public FSMTranslation getNextTranslate(FSMState input, uint nextStateID)
    {
        if (!initialized)
        {
            UnityEngine.Debug.LogError("FSMTranslationMap.getNextTranslate failed,translationMap hasn't initialized yet");
            return null;
        }

        FSMState matchState = input;
        FSMTranslation translation = null;
        FSMTranslationMatcher matcher;
        bool noMatch = true;
        while (matchState != null)
        {
            if (translationMap.TryGetValue(input, out matcher))
            {
                translation = matcher.matchTranslation(nextStateID);
                if (translation != null)
                {
                    noMatch = false;
                    break;
                }
            }
            matchState = matchState.Parent;
        }

        if (noMatch)
        {
            UnityEngine.Debug.LogError(string.Format("FSMTranslationMap.getNextTranslate failed,can't find {0} in translationMap", input.Name));
        }

        if (translation == null)
        {
            UnityEngine.Debug.LogError(string.Format("FSMTranslationMap.getNextTranslate failed,not matched translation found,inputstate={0},nextStateID={1}", input.Name, nextStateID));
        }

        return translation;
    }

    public bool check()
    {
        if (translationMap.Count == 0)
        {
            UnityEngine.Debug.LogError("FSMTranslationMap.build Failed ,have no states in translationMap");
            return false;
        }

        if (startState == null)
        {
            UnityEngine.Debug.LogError("FSMTranslationMap.build Failed ,no start State");
            return false;
        }

        initialized = true;
        return initialized;
    }

    public FSMState addState(FSMState state)
    {
        if (translationMap.ContainsKey(state))
        {
            UnityEngine.Debug.LogError(string.Format("FSMTranslationMap.addState Failed,{0} is already present in translation map", state.QualifyName));
            return null;
        }

        if (state.StateType == FSMStateType.STATE_START)
        {
            if (startState != null)
            {
                UnityEngine.Debug.LogError(string.Format("FSMTranslationMap.addState {0} Failed, startState already present in translation map", state.QualifyName));
                return null;
            }
            startState = state;
        }

        translationMap.Add(state, new FSMTranslationMatcher());
        stateIDMap.Add(state.ID, state);
        return state;
    }

    public FSMState getStateByID(uint id)
    {
        FSMState state = null;
        stateIDMap.TryGetValue(id, out state);
        return state;    
    }

    public FSMState getStateByName(string name)
    {
        if (allStates != null)
        {
            for (int i = 0; i < allStates.Length; i++)
            {
                FSMState state = allStates[i];
                if (state.Name == name)
                {
                    return state;
                }
            }
        }
        return null;
    }

    public FSMTranslation addTranslation(FSMTranslation translation)
    {
        FSMTranslationMatcher matcher;
        if (!translationMap.TryGetValue(translation.Input, out matcher))
        {
            UnityEngine.Debug.LogError(string.Format("FSMTranslationMap.addTranslation Failed,can't find input state for translation,translation is:{0}", translation.Name));
            return null;
        }
        matcher.addTranslation(translation.Output.ID,translation);
        return translation;
    }

    public FSMTranslation addTranslation(FSMState input, FSMState output)
    {
        return addTranslation(new FSMTranslation(input, output,  null));
    }

    public FSMTranslation addTranslation(string name, FSMState input, FSMState output, FSMAction action)
    {
        return addTranslation(new FSMTranslation(name, input, output, action));
    }

    public FSMTranslation addTranslation(FSMState input, FSMState output, FSMAction action)
    {
        return addTranslation(new FSMTranslation(input, output, action));
    }

    public FSMTranslation addTranslation(string name, Type eventType, FSMState input, FSMState output, FSMAction action)
    {
        if (!eventType.IsSubclassOf(typeof(IEvent)))
        {
            UnityEngine.Debug.LogError(string.Format("FSMTranslationMap.addTranslation Failed,{0} is not a event type", eventType.ToString()));
            return null;
        }
        return addTranslation(name, input, output,  action);
    }
}

class FSMTranslationMatcher
{
    private Dictionary<uint, FSMTranslation> translations = new Dictionary<uint, FSMTranslation>();
    public void addTranslation(uint stateID, FSMTranslation translation)
    {
        translations.Add(stateID, translation);
    }
    public FSMTranslation matchTranslation(uint stateID)
    {
        FSMTranslation translate = null;
        translations.TryGetValue(stateID, out translate);
        return translate;
    }
}
