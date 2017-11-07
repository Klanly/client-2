using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    private string name = string.Empty;
    private SceneInfo sceneInfo;
    private float distance;
    public void Init(SceneInfo sInfo)
    {
        sceneInfo = sInfo;
        name = sceneInfo.SceneName;
    }

    public string Name
    {
        get { return name; }
    }
    public float Distance
    {
        set { distance = value; }
    }

    public void LoadRangeThings(Vector3 position)
    {

    }
	
}

public class SceneThing
{
    private GameObjectInfo gameObjectInfo;
    private GameObject gameObject;
    private bool isHide = true;
    public void Init(GameObjectInfo goInfo)
    {
        gameObjectInfo = goInfo;
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
            
            //ResourceManager.CreateAssetBundleAsync()
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


    public void Clear()
    {
        Hide();
    }

}
