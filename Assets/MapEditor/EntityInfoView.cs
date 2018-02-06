using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
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
    private Text entityPositionText;
    private Text entityRotationText;
    private Combobox entityTypeCombobox;
    private Combobox entityResPathCombobox;


    public void Init(Transform transform)
    {
        container = transform;
        entityPositionText = container.Find("entityPositionText").GetComponent<Text>();
        entityRotationText = container.Find("entityRotationText").GetComponent<Text>();
        entityTypeCombobox = container.Find("entityTypeCombobox/Combobox").GetComponent<Combobox>();
        entityResPathCombobox = container.Find("entityResPathCombobox/Combobox").GetComponent<Combobox>();
    }

    public void Show()
    {

    }

    public void Hide()
    {

    }

}
