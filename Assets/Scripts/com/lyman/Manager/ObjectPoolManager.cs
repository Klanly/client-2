using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 对象池管理器，分普通类对象池+资源游戏对象池
/// </summary>
public class ObjectPoolManager
{
    private static ObjectPoolManager instance;

    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ObjectPoolManager();
            return instance;
        }
    }



    private Transform poolRootTransform = null;
    private Dictionary<string, object> objectPools = new Dictionary<string, object>();
    private Dictionary<string, GameObjectPool> gameObjectPools = new Dictionary<string, GameObjectPool>();



    private Transform PoolRootObject
    {
        get
        {
            if (poolRootTransform == null)
            {
                GameObject objectPool = new GameObject("ObjectPool");
                GameObject.DontDestroyOnLoad(objectPool);
                objectPool.transform.localScale = Vector3.one;
                objectPool.transform.localPosition = Vector3.zero;
                poolRootTransform = objectPool.transform;
            }
            return poolRootTransform;
        }
    }

    public GameObjectPool CreatePool(string poolName, uint maxSize, GameObject prefab)
    {
        GameObjectPool pool = GetPool(poolName);
        if (pool == null)
        {
            pool = new GameObjectPool(poolName, prefab, maxSize, PoolRootObject);
            gameObjectPools[poolName] = pool;
        }
        return pool;
    }

    public GameObjectPool GetPool(string poolName)
    {
        if (gameObjectPools.ContainsKey(poolName))
        {
            return gameObjectPools[poolName];
        }
        return null;
    }

    public GameObject Get(string poolName)
    {
        GameObject result = null;
        if (gameObjectPools.ContainsKey(poolName))
        {
            GameObjectPool pool = gameObjectPools[poolName];
            result = pool.GetGameObject();
            if (result == null)
            {
                Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
            }
        }
        else
        {
            Debug.LogError("Invalid pool name specified: " + poolName);
        }
        return result;
    }

    public void Recycle(string poolName, GameObject gameObject)
    {
        if (gameObjectPools.ContainsKey(poolName))
        {
            GameObjectPool pool = gameObjectPools[poolName];
            pool.RecycleGameObject(poolName, gameObject);
        } else
        {
            Debug.LogWarning("No pool available with name: " + poolName);
        }
    }

    /// <summary>
    /// 销毁超过缓存数量的GameObject,在切换地图前调用
    /// </summary>
    public void DestroyRedundantGameObject()
    {
        GameObjectPool[] pools = new GameObjectPool[gameObjectPools.Count];
        gameObjectPools.Values.CopyTo(pools, 0);
        for (int i = 0; i < pools.Length; ++i)
        {
            GameObjectPool gameObjectPool = pools[i];
            gameObjectPool.DestroyRedundantGameObject();
        }
    }

    /// <summary>
    /// 销毁所有实例 一般不调用，一定要调用，请在切换地图的时候调用
    /// </summary>
    public void DestroyAllGameObject()
    {
        GameObjectPool[] pools = new GameObjectPool[gameObjectPools.Count];
        gameObjectPools.Values.CopyTo(pools, 0);
        for (int i = 0; i < pools.Length; ++i)
        {
            GameObjectPool gameObjectPool = pools[i];
            gameObjectPool.DestroyAllGameObject();
        }
    }

    ///=======================================非GameObject对象池===================================================///
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionOnGet"></param>
    /// <param name="actionOnRelease"></param>
    /// <returns></returns>
    public ObjectPool<T> CreatePool<T>(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease) where T : class {
        var type = typeof(T);
        var pool = new ObjectPool<T>(actionOnGet, actionOnRelease);
        objectPools[type.Name] = pool;
        return pool;
    }

    public ObjectPool<T> GetPool<T>() where T : class {
        var type = typeof(T);
        ObjectPool<T> pool = null;
        if (objectPools.ContainsKey(type.Name)) {
            pool = objectPools[type.Name] as ObjectPool<T>;
        }
        return pool;
    }

    public T Get<T>() where T : class {
        var pool = GetPool<T>();
        if (pool != null) {
            return pool.Get();
        }
        return default(T);
    }

    public void Release<T>(T obj) where T : class {
        var pool = GetPool<T>();
        if (pool != null) {
            pool.Release(obj);
        }
    }
}