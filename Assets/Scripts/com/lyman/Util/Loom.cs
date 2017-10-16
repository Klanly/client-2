using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class Loom : MonoBehaviour
{
    private List<DelayedQueueItem> delayed = new List<DelayedQueueItem>();

    private List<DelayedQueueItem> currentDelayed = new List<DelayedQueueItem>();

    private List<Action> currentActions = new List<Action>();

    private List<Action> actions = new List<Action>();

    private int count;

    public static int maxThreads = 8;
    private static int numThreads;
    private static bool initialized;
    private static Loom current;
    

    public static Loom Current
    {
        get
        {
            Initialize();
            return current;
        }
    }

    void Awake()
    {
        current = this;
        initialized = true;
    }

    

    private static void Initialize()
    {
        if (!initialized)
        {

            if (!Application.isPlaying)
                return;
            initialized = true;
            var g = new GameObject("Loom");
            current = g.AddComponent<Loom>();
        }

    }

    
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0f);
    }
    public static void QueueOnMainThread(Action action, float time)
    {
        if (time != 0)
        {
            lock (Current.delayed)
            {
                Current.delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else
        {
            lock (Current.actions)
            {
                Current.actions.Add(action);
            }
        }
    }

    public static Thread RunAsync(Action a)
    {
        Initialize();
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }

    }


   


    
    

    // Update is called once per frame
    void Update()
    {
        lock (actions)
        {
            currentActions.Clear();
            currentActions.AddRange(actions);
            actions.Clear();
        }
        for (int i = 0; i < currentActions.Count; ++i)
        {
            Action action = currentActions[i];
            if(action != null)
                action();
        }
        lock (delayed)
        {
            currentDelayed.Clear();
            currentDelayed.AddRange(delayed.Where(d => d.time <= Time.time));
            for (int i = 0; i < currentDelayed.Count; ++i)
            {
                DelayedQueueItem delayedQueneItem = currentDelayed[i];
                delayed.Remove(delayedQueneItem);
            }
        }
        for (int i = 0; i < currentDelayed.Count; ++i)
        {
            DelayedQueueItem delayedQueneItem = currentDelayed[i];
            delayedQueneItem.action();
        }
    }
}