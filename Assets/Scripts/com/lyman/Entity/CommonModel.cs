using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommonModel
{
    protected GameObject container;

    protected GameObject body;

    protected bool isCreate;

    protected string abPath;

    private string pbName;

    protected CharacterAnimatorController animator;

    private AnimationClipEventHandler animationClipEventHandler;

    private string lastAnimationName;

    private Dictionary<string, Transform> childrens = new Dictionary<string, Transform>();//模型自身帶的tranfrom，比如一些武器挂点

    private Dictionary<string, GameObject> otherChilds = new Dictionary<string, GameObject>(); //从外部添加进来的transfrom 比如换武器 装扮等


    private string tag;

    private Action onCreateCompelteHandler;

    private string name;

    private int layer = -1;

    private Vector3 position = Vector3.zero;

    public CommonModel()
    {
        container = new GameObject();
    }
    
    public string Name
    {
        set
        {
            name = value;
            if (container != null)
            {
                container.name = name;
            }
        }
    }
    public int Layer
    {
        set
        {
            layer = value;
            if (container != null)
            {
                container.layer = value;
            }
        }
        get { return layer; }
    }

    public Vector3 LocalScale
    {
        set
        {
            if (container != null)
            {
                container.transform.localScale = value;
            }
        }
    }
    
    public Vector3 Position
    {
        set
        {
            position.Set(value.x, value.y, value.z);
            if (container != null)
            {
                container.transform.position = value;
            }
        }
        get
        {
            if (container != null)
            {
                position.Set(container.transform.position.x, container.transform.position.y, container.transform.position.z);
            }
            return position;
        }
    }

    public void LookAt(Vector3 position)
    {
        Container.transform.LookAt(position);
    }

    public Vector3 Euler
    {
        get { return container.transform.eulerAngles; }
    }

    public Quaternion Rotation
    {
        set
        {
            if (container != null)
            {
                container.transform.rotation = value;
            }
        }
    }

    

    public GameObject Container
    {
        get { return container; }
    }
    public GameObject Body
    {
        get { return body; }
    }

    public bool IsCreate
    {
        get { return isCreate; }
        set { isCreate = value; }
    }


    public string Tag
    {
        get { return tag; }
        set
        {
            tag = value;
            if (Container != null)
                Container.tag = tag;
        }
    }

    public void Init(string assetBundlePath, string prefabName,Action createCompelteHandler = null)
    {
        childrens.Clear();
        if (body != null)
        {
            GameObject.Destroy(body);
            body = null;
        }
        abPath = assetBundlePath;
        pbName = prefabName;
        onCreateCompelteHandler = createCompelteHandler;
        ResourceManager.CreateAssetBundleAsync(abPath, onCreateABCompelteHandler);
    }

    private void onCreateABCompelteHandler()
    {
        body = ResourceManager.GetGameObjectInstance(abPath, pbName);
        if (body == null)
        {
            Debug.LogError(abPath+" 不存在");
            return;
        }
        body.transform.SetParent(Container.transform);
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = Vector3.one;
        if (onCreateCompelteHandler != null)
        {
            onCreateCompelteHandler();
        }
    }



    public void EnableAnimator()
    {
        if (animator == null && body != null)
        {
            animator = body.AddComponent<CharacterAnimatorController>();
        }
        if(animator != null)
            animator.enabled = true;
    }
    public void DisableAnimator()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
    }



    public void EnableAnimationEvent()
    {
        if (animationClipEventHandler == null && body != null)
        {
            animationClipEventHandler = body.AddComponent<AnimationClipEventHandler>();
        }
        if(animationClipEventHandler != null)
            animationClipEventHandler.enabled = true;
    }
    public void DisableAnimationEvent()
    {
        if (animationClipEventHandler != null)
        {
            animationClipEventHandler.enabled = false;
        }
    }
    
    public void PlayAnimation(string actionName, float playSpeed = 1f, Action hitHandler = null, Action endHandler = null, Action translateHandler = null)
    {
        if (animator == null)
        {
            EnableAnimator();
            EnableAnimationEvent();
        }
        if (animator != null)
        {
            animationClipEventHandler.hitHandler = hitHandler;
            animationClipEventHandler.endHandler = endHandler;
            animationClipEventHandler.translateHandler = translateHandler;
            animator.ChangeAnimation(actionName, playSpeed);
            lastAnimationName = actionName;
        }
    }

    public float GetCurrentActionLength()
    {
        if (animator != null)
        {
            return animator.GetCurrentActionLength();
        }
        return 0f;
    }

    public void RemoveChildren(string boneName)
    {
        Transform bone = GetChild(boneName);
        if (bone != null)
        {
            if (bone.childCount > 0)
            {
                int cCount = bone.childCount;
                for (int i = 0; i < cCount; ++i)
                {
                    GameObject.Destroy(bone.GetChild(i).gameObject);
                }
            }
        }
    }
    
    public bool AddChild(string childName, GameObject child, string parentName)
    {
        if (child == null || body == null || string.IsNullOrEmpty(parentName) || string.IsNullOrEmpty(childName))
        {
            return false;
        }
        if (otherChilds.ContainsKey(childName))
        {
            GameObject go = otherChilds[childName];
            GameObject.Destroy(go);
            otherChilds.Remove(childName);
        }
        Transform bone = GetChild(parentName);
        if (bone != null)
        {
            child.transform.SetParent(bone);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
            child.transform.localRotation = Quaternion.identity;
            otherChilds.Add(childName, child);
            return true;
        }
        return false;
    }

    private void ClearOtherChilds()
    {
        if (otherChilds.Count == 0) return;
        GameObject[] childs = new GameObject[otherChilds.Count];
        otherChilds.Values.CopyTo(childs, 0);
        otherChilds.Clear();
        for (int i = 0; i < childs.Length; ++i)
        {
            GameObject child = childs[i];
            child.transform.SetParent(null);
        }
    }


    public void RemoveChild(string parentName, string childName, bool destroy = false)
    {
        string key = parentName + "/" + childName;
        GameObject child;
        otherChilds.TryGetValue(key, out child);
        if (child != null)
        {
            otherChilds.Remove(key);
            child.transform.SetParent(null);
            if (destroy)
            {
                GameObject.Destroy(child);
                child = null;
            }
        }
    }



    public Transform GetChild(string childName)
    {
        if (body == null || body.transform == null || string.IsNullOrEmpty(childName))
        {
            return null;
        }
        Transform child;
        childrens.TryGetValue(childName, out child);
        if (child == null)
        {
            child = Util.FindInChildByName(body.transform, childName);
            if (child != null)
            {
                childrens.Add(childName, child);
            }
        }
        return child;
    }

    public void Show()
    {
        if (container != null)
        {
            container.SetActive(true);
        }
    }

    public void Hide()
    {
        if (container != null)
        {
            container.SetActive(false);
        }
    }

    public void ShowBody()
    {
        if (body != null)
        {
            body.SetActive(true);
        }
    }

    public void HideBody()
    {
        if (body != null)
        {
            body.SetActive(false);
        }
    }

    public void Update()
    {
        
    }

    public void Destroy()
    {
        ClearOtherChilds();
        childrens.Clear();
        childrens = null;
        GameObject.Destroy(container);
        GameObject.Destroy(body);
        container = null;
        body = null;
    }
}
