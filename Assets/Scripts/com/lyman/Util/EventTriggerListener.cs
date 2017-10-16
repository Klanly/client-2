using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    
    public Action<GameObject> onClick;
    public Action<GameObject> onDown;
    public Action<GameObject> onEnter;
    public Action<GameObject> onExit;
    public Action<GameObject> onUp;
    public Action<GameObject> onSelect;
    public Action<GameObject> onUpdateSelect;
    public Action<GameObject> onScroll;

    public Action<GameObject> onDragBeginHandler;
    public Action<GameObject> onDragHandler;
    public Action<GameObject> onDragEndHandler;
    
    static public EventTriggerListener Get(GameObject go)
    {
        if (go == null)
            return null;
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
            listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onDragBeginHandler != null)
            onDragBeginHandler(gameObject);
        
        holdClickTimer = 0;
    }
    public override void OnDrag(PointerEventData eventData)
    {
        
        if (onDragHandler != null)
            onDragHandler(gameObject);
        
        holdClickTimer = 0;

    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onDragEndHandler != null)
            onDragEndHandler(gameObject);
        holdClickTimer = 0;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
            onDown(gameObject);
        
        holdClickTimer = Time.time;
        if(Time.time - doubleClickTimer <= 0.3f)
        {
            doubleClickTimer = -1;       
        }
        else
        {
            doubleClickTimer = Time.time;
        }

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
            onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
            onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
            onUp(gameObject);
        if(holdClickTimer != 0 && Time.time >= holdClickTimer + HoldClickSecond)
        {
            holdClickTimer = 0;
        }
    }
    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null)
            onScroll(gameObject);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null)
            onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null)
            onUpdateSelect(gameObject);
    }

    /// <summary>
    /// 长按的时间设定(不主动设默认1秒)
    /// </summary>
    private float HoldClickSecond = 1f;
    public void SetHoldSecond(float value)
    {
        HoldClickSecond = value;
    }
    /// <summary>
    /// 长按的计时器
    /// </summary>
    private float holdClickTimer;
    /// <summary>
    /// 双击事件计时器
    /// </summary>
    private float doubleClickTimer = -1f;
    
}