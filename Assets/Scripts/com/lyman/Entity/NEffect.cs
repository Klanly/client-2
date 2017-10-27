using UnityEngine;
using System.Collections;
using System.Text;
using System;
public class NEffect
{
    private float duration = 0f;
    private int key;
    private GameObject effect;
    private bool isLoop;
    private float totalTime;
    private string soundName;

    private string abName;
    private string prefabName;
    private TimerInfo timerInfo;
    private AudioSource audioSource;
    private float soundDelayTime;
    
    public void Init(string effectABName, string effectName, string audioClipName = "",float delayTime = 0f,bool loop = false)
    {
        effect = SEffectManager.GetEffect(effectABName, effectName);
        soundName = audioClipName;
        soundDelayTime = delayTime;
        isLoop = loop;
        abName = effectABName;
        prefabName = effectName;
    }

    public void Play()
    {
        if (effect == null) return;
        effect.SetActive(true);
        if (!string.IsNullOrEmpty(soundName) && soundName != BindTypes.NONE)
        {
            AudioClip audioClip = ResourceManager.GetAudioClip(GameConst.SoundABDirectory + soundName, soundName);
            audioSource = effect.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = effect.AddComponent<AudioSource>();
            }
            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.PlayDelayed(soundDelayTime);
        }
        if (!isLoop)
        {
            totalTime = 0f;
            duration = SEffectManager.GetEffectDuration(effect, abName, prefabName);
            timerInfo = TimerManager.AddHandler(OnDelayHandler);
        }
    }
    
    public void Stop()
    {
        TimerManager.RemoveHandler(timerInfo);
        timerInfo = null;
        SEffectManager.RecycleEffect(abName, prefabName, effect);
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
    
    public void OnDelayHandler(float delaytime)
    {
        totalTime += delaytime;
        if (totalTime >= duration)
        {
            Stop();
        }
    }
    
    public GameObject GetEffect()
    {
        return effect;
    }
}
