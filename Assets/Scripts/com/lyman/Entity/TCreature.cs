using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCreature : Creature
{
    public void Init(CharacterConfigInfo cInfos)
    {
        base.Init(cInfos, OnCreateHandler);
        FSM = FSMFactory.GetCharacterEditorFSM(this);
    }

    private void OnCreateHandler()
    {

    }


    

}