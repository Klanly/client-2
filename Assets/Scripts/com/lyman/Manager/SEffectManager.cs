using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效管理
/// </summary>
public class SEffectManager
{
    private static uint CacheCount = 2;

    private static Dictionary<string, float> effectDurations = new Dictionary<string, float>();

    public static GameObject GetEffect(string abName, string prefabName)
    {
        if (string.IsNullOrEmpty(abName) || string.IsNullOrEmpty(prefabName)) return null;
        GameObject prefab = ResourceManager.GetGameObjectPrefab(abName, prefabName);
        GameObjectPool gameObjectPool = ObjectPoolManager.Instance.CreatePool(abName + "_" + prefabName, CacheCount, prefab);
        GameObject go = gameObjectPool.GetGameObject();
        return go;
    }

    public static void RecycleEffect(string abName, string prefabName, GameObject gameObject)
    {
        if (string.IsNullOrEmpty(abName) || string.IsNullOrEmpty(prefabName)) return;
        string poolName = abName + "_" + prefabName;
        GameObjectPool gameObjectPool = ObjectPoolManager.Instance.GetPool(poolName);
        if (gameObjectPool != null)
        {
            gameObjectPool.RecycleGameObject(poolName, gameObject);
        }
    }

    public static float GetEffectDuration(GameObject effect, string abName, string prefabName)
    {
        if (effect == null || string.IsNullOrEmpty(abName) || string.IsNullOrEmpty(prefabName)) return 0f;
        string key = abName + "_" + prefabName;
        if (effectDurations.ContainsKey(key))
        {
            return effectDurations[key];
        }
        else
        {   
            float duration = 0f;
            ParticleSystem[] particles = effect.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; ++i)
            {
                ParticleSystem particle = particles[i];
                if (particle.emission.enabled)
                {
                    float time = particle.main.startDelayMultiplier + Mathf.Max(particle.main.duration, particle.main.startLifetimeMultiplier);
                    if (time > duration)
                    {
                        duration = time;
                    }
                }
            }
            effectDurations.Add(key, duration);
            return duration;
        }
    }




}
