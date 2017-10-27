using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCreature : Creature
{

    public TCreature()
    {
        this.ID = Entity.UniqueID;
    }

    public void Init(CharacterConfigInfo cInfos)
    {
        base.Init(cInfos, OnCreateHandler);
        
        FSM = FSMFactory.GetCharacterEditorFSM(this);
    }

    private void OnCreateHandler()
    {
        SelectorKeeper selectorKeeper = this.Container.AddComponent<SelectorKeeper>();
        selectorKeeper.OwnerID = this.ID;
        Tag = GameObjectTags.Entity;
    }


    

}