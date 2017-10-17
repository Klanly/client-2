using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 动作类型
/// </summary>
public class AnimationType
{
    //待机
    public static string Idle = "Idle";
    //行走
    public static string Walk = "Walk";
    //跑动
    public static string Run = "Run";
    //死亡
    public static string Dead = "Die";
    //受击
    public static string Hit = "Hit";

    //飞1
    public static string Fly1 = "Fly_1";
    //飞2
    public static string Fly2 = "Fly_2";
    //飞3
    public static string Fly3 = "Fly_3";


    //攻击1
    public static string Attack1 = "Att_1";
    //攻击2
    public static string Attack2 = "Att_2";
    //攻击3
    public static string Attack3 = "Att_3";
    //攻击3
    public static string Attack4 = "Att_4";
    //攻击4
    public static string Attack5 = "Att_5";
    //攻击6
    public static string Attack6 = "Att_6";
    //攻击7
    public static string Attack7 = "Att_7";
    //攻击8
    public static string Skill = "Skill";
    
    private static List<string> actionList = new List<string>();

    public static List<string> GetActionList()
    {
        if (actionList.Count == 0)
        {
            actionList.Add(Idle);
            actionList.Add(Walk);
            actionList.Add(Run);
            actionList.Add(Dead);
            actionList.Add(Hit);
            actionList.Add(Attack1);
            actionList.Add(Attack2);
            actionList.Add(Attack3);
            actionList.Add(Attack4);
            actionList.Add(Attack5);
            actionList.Add(Attack6);
            actionList.Add(Attack7);
            actionList.Add(Skill);
        }
        return actionList;
    }

    private static List<string> loopActionList = new List<string>();
    public static bool IsLoopAction(string actionName)
    {
        if (loopActionList.Count == 0)
        {
            loopActionList.Add(AnimationType.Walk);
            loopActionList.Add(AnimationType.Idle);
            loopActionList.Add(AnimationType.Run);
        }
        return loopActionList.Contains(actionName);
    }

    private static List<string> attackList = new List<string>();
    public static bool IsAttackAction(string actionName)
    {
        if (attackList.Count == 0)
        {
            attackList.Add(Attack1);
            attackList.Add(Attack2);
            attackList.Add(Attack3);
            attackList.Add(Attack4);
            attackList.Add(Attack5);
            attackList.Add(Attack6);
            attackList.Add(Attack7);
            attackList.Add(Skill);
        }
        return attackList.Contains(actionName);
    }
}