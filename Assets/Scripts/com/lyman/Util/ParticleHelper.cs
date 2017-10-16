using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ParticleHelper
{
    
    public float scale = 1.0f;
    private List<float> initialSizes = new List<float>();
    private ParticleSystem[] particles;

    private Dictionary<ParticleSystem, ParticleSystemRenderer> maping = new Dictionary<ParticleSystem, ParticleSystemRenderer>();

    private float duration;

    private Vector3 scaleV3 = Vector3.zero;

    private GameObject gameObject;

    public void Init(GameObject go)
    {
        if (particles == null)
        {
            gameObject = go;
            particles = gameObject.GetComponentsInChildren<ParticleSystem>();
            duration = 0f;
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
                initialSizes.Add(particle.main.startSizeMultiplier);
                ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
                if (renderer)
                {
                    maping.Add(particle, renderer);
                    initialSizes.Add(renderer.lengthScale);
                    initialSizes.Add(renderer.velocityScale);
                }
            }
        }
    }

    public float Duration
    {
        get { return duration; }
    }

    

    public float Scale
    {
        set
        {
            scale = value;
            scaleV3.Set(scale, scale, scale);
            gameObject.transform.localScale = scaleV3;
            float magnitude = gameObject.transform.localScale.magnitude;
            int arrayIndex = 0;
            int length = particles.Length;
            for (int i = 0; i < length; ++i)
            {
                ParticleSystem particle = particles[i];
                ParticleSystem.MainModule main = particle.main;
                main.startSizeMultiplier = initialSizes[arrayIndex++] * magnitude;
                ParticleSystemRenderer renderer;
                maping.TryGetValue(particle, out renderer);
                if (renderer != null)
                {
                    renderer.lengthScale = initialSizes[arrayIndex++] * magnitude;
                    renderer.velocityScale = initialSizes[arrayIndex++] * magnitude;
                }
            }
        }
    }

    
    /// <summary>
    /// value必须是幅度值
    /// </summary>
    public float Rotation
    {
        set
        {
            int length = particles.Length;
            for (int i = 0; i < length; ++i)
            {
                ParticleSystem particle = particles[i];
                ParticleSystem.MainModule main = particle.main;
                main.startRotation = value;
            }
        }
    }
   

    public void Destroy()
    {
        particles = null;
        initialSizes.Clear();
        maping.Clear();
    }
}