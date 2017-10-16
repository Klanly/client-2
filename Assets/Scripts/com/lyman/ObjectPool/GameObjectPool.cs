using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GameObject 对象池
/// </summary>
public class GameObjectPool
{
    private uint maxSize;
	private string poolName;
    private Transform poolRoot;
    private GameObject poolObjectPrefab;
    private Stack<GameObject> availableObjStack = new Stack<GameObject>();

    private Stack<GameObject> redundantObjStack = new Stack<GameObject>();

    public GameObjectPool(string poolName, GameObject poolObjectPrefab, uint maxSize, Transform pool)
    {
		this.poolName = poolName;
        this.maxSize = maxSize;
        this.poolRoot = pool;
        this.poolObjectPrefab = poolObjectPrefab;
	}

    /// <summary>
    /// 获取实例
    /// </summary>
    /// <returns></returns>
	public GameObject GetGameObject()
    {
        GameObject go = null;
        if (redundantObjStack.Count > 0)
        {
            go = redundantObjStack.Pop();
            go.SetActive(true);
        }
        else if (availableObjStack.Count > 0)
        {
			go = availableObjStack.Pop();
            go.SetActive(true);
        }
        else
        {
            go = NewObjectInstance();
		}
        return go;
	} 
	
	/// <summary>
    /// 回收实例
    /// </summary>
    /// <param name="poolname"></param>
    /// <param name="gameObject"></param>
    public void RecycleGameObject(string poolname, GameObject gameObject)
    {
        if (poolName.Equals(poolname))
        {
            AddGameObjectToPool(gameObject);
		}
        else
        {
			Debug.LogError(string.Format("Trying to add object to incorrect pool {0} ", poolName));
		}
	}

    /// <summary>
    /// 销毁超过缓存数量的GameObject,在切换地图前调用
    /// </summary>
    public void DestroyRedundantGameObject()
    {
        DestroyGameObject(redundantObjStack);
    }

    /// <summary>
    /// 销毁该池的所有实例
    /// </summary>
    public void DestroyAllGameObject()
    {
        DestroyRedundantGameObject();
        DestroyGameObject(availableObjStack);
    }

    private void DestroyGameObject(Stack<GameObject> list)
    {
        int length = list.Count;
        for (int i = 0; i < length; ++i)
        {
            GameObject go = list.Pop();
            GameObject.Destroy(go);
            go = null;
        }
    }

    private void AddGameObjectToPool(GameObject gameObject)
    {
        if (gameObject == null) return;
        //add to pool
        gameObject.SetActive(false);
        gameObject.transform.SetParent(poolRoot, false);
        if (availableObjStack.Count < this.maxSize)
        {
            availableObjStack.Push(gameObject);
        }
        else
        {
            redundantObjStack.Push(gameObject);
        }
    }

    private GameObject NewObjectInstance()
    {
        return GameObject.Instantiate(poolObjectPrefab) as GameObject;
    }

}