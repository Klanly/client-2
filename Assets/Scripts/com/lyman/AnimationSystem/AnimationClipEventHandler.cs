using UnityEngine;
using System.Collections;
using System;
public class AnimationClipEventHandler : MonoBehaviour
{

    public Action hitHandler;
    public Action endHandler;

    public Action translateHandler;

    public void hitevent()
    {
        if (hitHandler != null)
        {
            hitHandler();
        }
    }

    public void endevent()
    {
        if (endHandler != null)
        {
            endHandler();
        }
    }
    
    public void translateevent()
    {
        if (translateHandler != null)
        {
            translateHandler();
        }
    }
}
