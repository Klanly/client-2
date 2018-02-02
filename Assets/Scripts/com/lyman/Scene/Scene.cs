using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    private string name = string.Empty;
    private SceneInfo sceneInfo;
    private float distance;


    private Transform effectsContainer;
    private Transform terrainContainer;
    private Transform blocksContainer;
    private Transform nonblocksContainer;
    private Transform lightsContainer;

    private SceneThing terrainThing;

    public Scene()
    {
        effectsContainer = GameObject.Find("effects").transform;
        terrainContainer = GameObject.Find("terrain").transform;
        blocksContainer = GameObject.Find("blocks").transform;
        nonblocksContainer = GameObject.Find("nonblocks").transform;
        lightsContainer = GameObject.Find("lights").transform;
    }


    public void Init(SceneInfo sInfo)
    {
        sceneInfo = sInfo;
        name = sceneInfo.SceneName;
        terrainThing = new SceneThing();
        terrainThing.TerrainContainer = terrainContainer;
        terrainThing.Init(sceneInfo.TerrainInfo);
        terrainThing.Show();
    }

    public string Name
    {
        get { return name; }
    }
    public float Distance
    {
        set { distance = value; }
    }

    private Vector3 tempVector3 = Vector3.zero;

    private List<SceneThing> blocks = new List<SceneThing>();
    private List<SceneThing> nonBlocks = new List<SceneThing>();
    private List<SceneThing> effects = new List<SceneThing>();

    public void LoadRangeThings(Vector3 position)
    {

        SyncShowThing(sceneInfo.Blocks, position, blocks, distance);
        SyncShowThing(sceneInfo.NonBlocks, position, nonBlocks, distance);
        SyncShowThing(sceneInfo.Effects, position, effects, distance);
    }


    public void LoadAllThings()
    {
        SyncShowThing(sceneInfo.Blocks, Vector3.zero, blocks, 10000f);
        SyncShowThing(sceneInfo.NonBlocks, Vector3.zero, nonBlocks, 10000f);
        SyncShowThing(sceneInfo.Effects, Vector3.zero, effects, 10000f);
    }


    private void SyncShowThing(List<GameObjectInfo> list,Vector3 position, List<SceneThing> things,float range)
    {
        GameObjectInfo gameObjectInfo;
        SceneThing sceneThing = null;
        int i = 0;
        for (i = 0; i < list.Count; ++i)
        {
            gameObjectInfo = list[i];
            tempVector3.Set(gameObjectInfo.X, gameObjectInfo.Y, gameObjectInfo.Z);
            float tdistance = Vector3.Distance(position, tempVector3);
            if (things.Count > i)
            {
                sceneThing = things[i];
            }
            if (tdistance <= range)
            {
                if (sceneThing == null)
                {
                    sceneThing = new SceneThing();
                    sceneThing.BlocksContainer = blocksContainer;
                    sceneThing.NonblocksContainer = nonblocksContainer;
                    sceneThing.EffectContainer = effectsContainer;
                    sceneThing.LightsContainer = lightsContainer;
                    things.Add(sceneThing);
                }
                sceneThing.Init(gameObjectInfo);
                sceneThing.Show();
            }
            else if (sceneThing != null)
            {
                sceneThing.Hide();
            }
            sceneThing = null;
        }
    }

}

public class SceneThing
{
    private GameObjectInfo gameObjectInfo;
    private GameObject gameObject;
    private bool isHide = true;
    private string abPath;
    private bool isDestroy;

    private Transform effectContainer;
    private Transform terrainContainer;
    private Transform blocksContainer;
    private Transform nonblocksContainer;
    private Transform lightsContainer;

    private Vector3 position;
    private Vector3 scale;
    private Vector3 rotation;

    private SphereCollider sphereCollider;
    private CapsuleCollider capsuleCollider;

    public Transform EffectContainer
    {
        get { return effectContainer; }
        set { effectContainer = value; }
    }
    public Transform TerrainContainer
    {
        get { return terrainContainer; }
        set { terrainContainer = value; }
    }
    public Transform BlocksContainer
    {
        get { return blocksContainer; }
        set { blocksContainer = value; }
    }
    public Transform NonblocksContainer
    {
        get { return nonblocksContainer; }
        set { nonblocksContainer = value; }
    }
    public Transform LightsContainer
    {
        get { return lightsContainer; }
        set { lightsContainer = value; }
    }


    private Transform GetParent()
    {
        if (gameObjectInfo.Type == GameObjectTypes.Effect)
        {
            return EffectContainer;
        }
        else if (gameObjectInfo.Type == GameObjectTypes.Block)
        {
            return BlocksContainer;
        }
        else if (gameObjectInfo.Type == GameObjectTypes.NonBlock)
        {
            return NonblocksContainer;
        }
        else if (gameObjectInfo.Type == GameObjectTypes.Terrain)
        {
            return TerrainContainer;
        }
        else if (gameObjectInfo.Type == GameObjectTypes.Light)
        {
            return LightsContainer;
        }
        return null;
    }

    public void Init(GameObjectInfo goInfo)
    {
        gameObjectInfo = goInfo;
        position = new Vector3(gameObjectInfo.X, gameObjectInfo.Y, gameObjectInfo.Z);
        scale = new Vector3(gameObjectInfo.ScaleX, gameObjectInfo.ScaleY, gameObjectInfo.ScaleZ);
        rotation = new Vector3(gameObjectInfo.RotationX, gameObjectInfo.RotationY, gameObjectInfo.RotationZ);
        string head = gameObjectInfo.Type == GameObjectTypes.Effect ? GameConst.SceneEffectABDirectory : GameConst.SceneModelABDirectory;
        abPath = string.Format("{0}{1}", head, gameObjectInfo.PrefabName);
    }

    public void Show()
    {
        isHide = false;
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            ResourceManager.CreateAssetBundleAsync(abPath, OnCreateCompleteHandler);
        }
    }
    

    private void OnCreateCompleteHandler()
    {
        if (isDestroy) return;
        gameObject = ResourceManager.GetGameObjectInstance(abPath, gameObjectInfo.PrefabName);
        if (gameObjectInfo.ColliderType == ColliderTypes.SphereCollider && sphereCollider == null)
        {
            sphereCollider = gameObject.GetComponent<SphereCollider>();
        }
        else if (gameObjectInfo.ColliderType == ColliderTypes.CapsuleCollider && capsuleCollider == null)
        {
            capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        }
        if (isHide)
        {
            Hide();
        }
        else
        {
            //显示出来
            if (gameObject.transform.parent == GetParent()) return;
            gameObject.transform.SetParent(GetParent());
            if (gameObjectInfo.Type == GameObjectTypes.Block)
            {
                if(gameObjectInfo.ColliderType == ColliderTypes.SphereCollider)
                {   
                    if (sphereCollider)
                    {
                        sphereCollider.radius = gameObjectInfo.Radius;
                    }
                }
                else if (gameObjectInfo.ColliderType == ColliderTypes.CapsuleCollider)
                {
                    if (capsuleCollider)
                    {
                        capsuleCollider.radius = gameObjectInfo.Radius;
                        capsuleCollider.height = gameObjectInfo.Height;
                    }
                }
            }
            gameObject.transform.localPosition = position;
            gameObject.transform.localScale = scale;
            gameObject.transform.eulerAngles = rotation;
        }
    }


    public void Hide()
    {
        isHide = true;
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }


    
    public void Destroy()
    {
        isDestroy = true;
        isHide = true;
        sphereCollider = null;
        capsuleCollider = null;
        if (gameObject)
        {
            gameObject.transform.parent = null;
        }
        GameObject.Destroy(gameObject);
        gameObject = null;
    }

}
