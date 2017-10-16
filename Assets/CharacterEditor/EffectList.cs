using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectList : ModelList
{

    private static EffectList effectList;
    public static EffectList Instance
    {
        get
        {
            if (effectList == null)
            {
                effectList = new EffectList();
            }
            return effectList;
        }
        
    }


    private GameObject closeBtn;
    public override void Init(GameObject content, string abPath)
    {
        base.Init(content, abPath);
        closeBtn = content.transform.Find("closeBtn").gameObject;
        EventTriggerListener.Get(closeBtn).onClick = onCloseHandler;
    }

    private void onCloseHandler(GameObject go)
    {
        Hide();
    }
}
