using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum FSMActionType
{
    ACT_ENTER = 1,
    ACT_LEAVE = 2,
    ACT_TRANSLATION = 3,
}

public enum FSMStateType
{
    STATE_START = 1,
    STATE_END = 2,
    STATE_NORMAL = 3,
}

public delegate void FSMAction(FSMTranslation translatioin);
public class FSMState
{
    private uint id;
    private string name;
    private List<FSMState> children = new List<FSMState>();
    private FSMState parent;   
    private FSMState[] hierarchy;
    private string qualifyName;
    private FSMStateType type;
    public FSMState(string name, uint id, FSMStateType type, FSMState parent)
    {
        this.id = id;
        this.name = name;
        this.parent = parent;        
        this.type = type;

        if (parent == null)
        {
            hierarchy = new FSMState[1] { this };
            qualifyName = name;
        }
        else
        {
            hierarchy = new FSMState[parent.Hierarchy.Length + 1];
            parent.Hierarchy.CopyTo(hierarchy, 0);
            hierarchy[parent.Hierarchy.Length] = this;
            qualifyName = parent.QualifyName + "." + name;
        }
    }

    public virtual void Destroy()
    {

    }

    public FSMState(string name, uint id, FSMState parent) : this(name, id, FSMStateType.STATE_NORMAL, parent)
    {

    }

    public FSMState(string name, uint id) : this(name, id, null)
    {
    }
    public FSMState(string name, uint id, FSMStateType type):this(name,id,type,null)
    {

    }

    public void addChild(FSMState child)
    {
        children.Add(child);
    }

    public bool isSubStateOf(FSMState other)
    {
        if (parent == null)
            return false;

        if (parent == other)
            return true;
        else
            return parent.isSubStateOf(other);
    }

    public FSMState getLeastCommonParent(FSMState other)
    {
        FSMState least = null;
        FSMState[] otherHierarchy = other.Hierarchy;

        for (int i = hierarchy.Length - 1; i >= 0; i--)
        {
            for (int j = otherHierarchy.Length - 1; j >= 0; j--)
            {
                if (hierarchy[i] == otherHierarchy[j])
                {
                    least = hierarchy[i];
                    break;
                }
            }
        }
        return least;
    }
    public virtual void Update(float deltaTime) { }
    public virtual void OnEnter(FSMTranslation translation) { }
    public virtual void OnLeave(FSMTranslation translation) { }
    public List<FSMState> Children { get { return children; } }   
    public FSMState Parent { get { return parent; } }
    public uint ID { get { return id; } }
    public string Name { get { return name; } }
    public bool IsSubState { get { return parent == null; } }
    public string QualifyName { get { return qualifyName; } }
    public FSMState[] Hierarchy { get { return hierarchy; } }
    public FSMStateType StateType { get { return type; } }
}