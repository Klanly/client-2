using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfoView
{
    public static EntityInfoView Instance
    {
        get
        {
            if (entityInfoView == null) entityInfoView = new EntityInfoView();
            return entityInfoView;
        }
    }

    public static EntityInfoView entityInfoView;

    private Transform container;
    public void Init(Transform transform)
    {
        container = transform;
    }

    public void Show()
    {

    }

    public void Hide()
    {

    }

}
