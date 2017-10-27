using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class BulletEntity:Entity
{
    private int skillId;
    private int skillLevel;
    private float maxRange;

    private int ownerId;
    private int targetId;
    
    private Vector3 targetPosition;
    private NEffect bullet;
    private Tweener tween;
    private ColiderCheackHandler coliderCheackHandler;

    public int OwnerId
    {
        get { return ownerId; }
        set { ownerId = value; }
    }

    public int TargetId
    {
        get { return targetId; }
        set { targetId = value; }
    }

    private bool hitMove;
    public bool HitMove
    {
        get { return hitMove; }
        set { hitMove = value; }
    }
    private float hitMoveDistance;
    public float HitMoveDistance
    {
        get { return hitMoveDistance; }
        set { hitMoveDistance = value; }
    }
    private float hitMoveTime;
    public float HitMoveTime
    {
        get { return hitMoveTime; }
        set { hitMoveTime = value; }
    }


    private bool hitFly;
    public bool HitFly
    {
        get { return hitFly; }
        set { hitFly = value; }
    }

    private float hitFlyDistance;
    public float HitFlyDistance
    {
        get { return hitFlyDistance; }
        set { hitFlyDistance = value; }
    }
    private float hitFlyTime;
    public float HitFlyTime
    {
        get { return hitFlyTime; }
        set { hitFlyTime = value; }
    }

    public void Play(Vector3 bPosition, Vector3 tPosition,float flySpeed, string effectABName, string effectName, string audioClipName = "", float delayTime = 0f)
    {
        
        targetPosition = tPosition;
        bullet = new NEffect();
        bullet.Init(effectABName, effectName, audioClipName, delayTime,true);
        GameObject go = bullet.GetEffect();
        go.transform.position = bPosition;
        go.tag = GameObjectTags.Bullet;
        CapsuleCollider capsuleCollider = go.GetComponent<CapsuleCollider>();
        if(capsuleCollider == null)
            capsuleCollider = go.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;

        Rigidbody rigidbody = go.GetComponent<Rigidbody>();
        if(rigidbody == null)
            rigidbody = go.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;


        coliderCheackHandler = go.GetComponent<ColiderCheackHandler>();
        if(coliderCheackHandler == null)
            coliderCheackHandler = go.AddComponent<ColiderCheackHandler>();
        coliderCheackHandler.ColliderCallBack = OnCollider;

        bullet.Play();
        float distance = Vector3.Distance(bPosition, tPosition);
        

        float duration = distance / flySpeed;
        tween = go.transform.DOMove(targetPosition, duration);
        tween.OnComplete(OnCompleteHandler);
    }

    private void OnCompleteHandler()
    {
        tween.Kill();
        if (bullet != null)
            bullet.Stop();
        Debug.LogError("tweener end");
    }



    private void OnCollider(GameObject colliderTarget)
    {

        if (colliderTarget.tag == GameObjectTags.Terrain || colliderTarget.tag == GameObjectTags.Bullet)
        {
            return;
        }
        else
        {
            Transform tempTransform = colliderTarget.transform;
            while (tempTransform.parent)
            {
                tempTransform = tempTransform.parent;
            }
            SelectorKeeper selectorKeeper = tempTransform.gameObject.GetComponent<SelectorKeeper>();
            if (selectorKeeper == null)
            {
                //说明不是攻击目标 直接结束
                if (tween != null)
                {
                    tween.Kill();
                }
                if (bullet != null)
                    bullet.Stop();  
            }
            else
            {
                if (ownerId == selectorKeeper.OwnerID) return;
                if (tween != null)
                {
                    tween.Kill();
                }
                if (bullet != null)
                    bullet.Stop();

                Creature owner = EntityManager.GetEntityByID(ownerId) as Creature;
                Vector3 pos = owner.GetPosition();
                Creature target = EntityManager.GetEntityByID(TargetId) as Creature;
                if (target != null)
                {
                    //target
                    if (hitMove)
                    {
                        target.DoHitFly(pos, hitMoveDistance, hitMoveTime);
                    }
                    else if (hitFly)
                    {
                        target.DoHitFly(pos, hitFlyDistance, hitFlyTime);
                    }
                    else
                    {
                        target.DoHit();
                    }
                }
            }
        }
    }


    public override void Destroy()
    {
        base.Destroy();
        if(bullet != null)
            bullet.Stop();
        Debug.LogError("destroy");
    }
}


public class ColiderCheackHandler : MonoBehaviour
{
    public Action<GameObject> ColliderCallBack;

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.gameObject.tag != GameObjectTags.Terrain)
            {

                if (ColliderCallBack != null)
                {
                    ColliderCallBack(other.gameObject);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}