using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.IO;
using System.Collections.Generic;
public class AutoMakeAnimatorController : Editor
{
    public static string AssetsExtName = ".FBX";
    public static string AnimatorControllerExtName = ".controller";
    
    [MenuItem("GameTools/自动生成AnimatorController",false,4)]
    public static void DoCreateAnimationAssets()
    {
        List<UnityEditor.Animations.AnimatorState> states = new List<UnityEditor.Animations.AnimatorState>();
        List<string> actions = AnimationType.GetActionList();
        
        UnityEngine.Object select = Selection.activeObject;
        string p = AssetDatabase.GetAssetPath(select);
        if (!Directory.Exists(p))
        {
            return;
        }
        string dirName = Path.GetFileName(p);
        string fp = p + "/" + dirName.ToLower() + AnimatorControllerExtName;
        if (File.Exists(fp))
        {
            File.Delete(fp);
        }
        //创建animationController文件，保存在选择的目录下
        UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(fp);
        //得到它的Layer， 默认layer为base
        UnityEditor.Animations.AnimatorControllerLayer layer = animatorController.layers[0];

        string[] files = Directory.GetFiles(p);

        //List<float> ePrecent = new List<float>();
        //ePrecent.Add(0.95f);

        //List<string> events = new List<string>();
        //events.Add("endevent");

        //List<string> eventPs = new List<string>();
        //eventPs.Add("");

        for (int i = 0; i < actions.Count; i++)
        {
            string action = actions[i];
            string file = p + "/" + action + AssetsExtName;

            //if (action == AnimationType.Hit || action == AnimationType.Dead || action == AnimationType.Dodge)
            //{
            //    AddEventToAction(file, ePrecent, events, eventPs);
            //}
            //else if (AnimationType.IsAttackAction(action))
            //{
            //    ePrecent.Add(0.55f);
            //    events.Add("hitevent");
            //    eventPs.Add("");
            //    AddEventToAction(file, ePrecent, events, eventPs);
            //}

            if (File.Exists(file))
            {
                UnityEditor.Animations.AnimatorState state = AddStateTransition(animatorController, file, layer);
                states.Add(state);
            }
        }

       
        foreach (UnityEditor.Animations.AnimatorState fState in states)
        {
            if (fState.name == AnimationType.Idle)
            {
                layer.stateMachine.defaultState = fState;
            }
            foreach (UnityEditor.Animations.AnimatorState tState in states)
            {
                if (fState != tState)
                {
                    UnityEditor.Animations.AnimatorStateTransition at = fState.AddTransition(tState);
                    at.hasExitTime = false;
                    at.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 1f, tState.name);
                }
            }
        }

    }

    

    private static UnityEditor.Animations.AnimatorState AddStateTransition(UnityEditor.Animations.AnimatorController animatorController, string path, UnityEditor.Animations.AnimatorControllerLayer layer)
    {
        ////根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = null;
        if (File.Exists(path))
        {
            newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        }
        string[] sL = Path.GetFileName(path).Split(".".ToCharArray());
        string aName = sL[0];

        UnityEngine.AnimatorControllerParameter p = new UnityEngine.AnimatorControllerParameter();
        p.type = UnityEngine.AnimatorControllerParameterType.Bool;
        p.name = aName;
        p.defaultBool = false;
        animatorController.AddParameter(p);

        
        ////取出动画名子 添加到state里面
        UnityEditor.Animations.AnimatorState state = layer.stateMachine.AddState(aName);
        if(newClip != null)
            state.motion = newClip;
        
        return state;

        //把state添加在layer里面
        //UnityEditor.Animations.AnimatorStateTransition ts = sm.AddAnyStateTransition(state);
    }


    /// <summary>
    /// 通过脚本给动作添加事件
    /// </summary>
    /// <param name="fbxPath"></param>
    /// <param name="times"></param>
    /// <param name="functionNames"></param>
    /// <param name="stringParameters"></param>
    private static void AddEventToAction(string fbxPath,List<float> times,List<string> functionNames, List<string> stringParameters)
    {
        if (fbxPath == null || times == null || functionNames == null || stringParameters == null) return;
        if (!File.Exists(fbxPath)) return;
        if (!fbxPath.EndsWith(AssetsExtName)) return;
        if (times.Count == 0 || functionNames.Count == 0 || stringParameters.Count == 0) return;
        if (times.Count != functionNames.Count || times.Count != stringParameters.Count || functionNames.Count != stringParameters.Count) return;
        UnityEngine.Object[] objects = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        for (int m = 0; m < objects.Length; ++m)
        {
            if (objects[m].GetType() == typeof(AnimationClip))
            {
                AnimationClip clip = (AnimationClip)objects[m];
                List<AnimationEvent> list = new List<AnimationEvent>();
                for (int i = 0; i < functionNames.Count; ++i)
                {
                    AnimationEvent ae = new AnimationEvent();
                    ae.time = times[i] * clip.length;
                    ae.functionName = functionNames[i];
                    ae.stringParameter = stringParameters[i];
                    list.Add(ae);
                }
                AnimationUtility.SetAnimationEvents(clip, list.ToArray());
            }
        }
        AssetDatabase.Refresh();
    }

}
