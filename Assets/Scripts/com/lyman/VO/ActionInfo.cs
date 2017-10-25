using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
public class ActionInfo
{
    public string ActionName;//动作名称
    public float Length = 0f;
    public bool IsLangAttack = false;
    public float PlaySpeed = 1f;//播放速度
    public string SoundName = BindTypes.NONE;//音效名称
    public float SoundPlayDelayTime = 0f;//音效播放开始时间

    public float AttackRadius = 3f;
    public float AttackAngle = 90f;
    //

    //攻击自身是否位移
    public float SelfMoveDelayTime = 0f;
    public float SelfMoveDistance = 0f;
    public float SelfMoveTime = 0f;

    //受击是否位移
    public bool  IsHitMove = false;
    public float HitMoveDistance = 0f;
    public float HitMoveTime = 0f;


    //受击是否击飞
    public bool  IsHitFly = false;
    public float HitFlyDistance = 0f;
    public float HitFlyTime = 0f;

    public string ShakeScreen = BindTypes.NONE;//震屏类型
    public bool DefaultShakeScene = false;
    public bool IsLoop = false;
    public List<EffectInfo> ActionEffectInfos = new List<EffectInfo>();
    public List<EffectInfo> HitEffectInfos = new List<EffectInfo>();
    
    public string ToXMLString()
    {
        foreach (EffectInfo aeffectInfo in ActionEffectInfos)
        {
            aeffectInfo.Title = "ac";
        }
        foreach (EffectInfo heffectInfo in HitEffectInfos)
        {
            heffectInfo.Title = "hi";
        }
        StringBuilder stringbuilder = new StringBuilder();
        stringbuilder.Append("\t<as n='actioninfos'>\n");
        PushToStringBuilder(stringbuilder, "ActionName", ActionName);
        PushToStringBuilder(stringbuilder, "Length", Length.ToString());
        PushToStringBuilder(stringbuilder, "IsLangAttack", IsLangAttack ? "1":"0");
        
        PushToStringBuilder(stringbuilder, "IsLoop", (IsLoop ? 1 : 0).ToString());
        PushToStringBuilder(stringbuilder, "PlaySpeed", PlaySpeed.ToString());
        PushToStringBuilder(stringbuilder, "SoundName", SoundName);
        PushToStringBuilder(stringbuilder, "SoundPlayDelayTime", SoundPlayDelayTime.ToString());
        
        PushToStringBuilder(stringbuilder, "AttRange", AttackRadius.ToString());
        PushToStringBuilder(stringbuilder, "AttAngle", AttackAngle.ToString());
        
        PushToStringBuilder(stringbuilder, "SelfMoveDelayTime", SelfMoveDelayTime.ToString());
        PushToStringBuilder(stringbuilder, "SelfMoveDistance", SelfMoveDistance.ToString());
        PushToStringBuilder(stringbuilder, "SelfMoveTime", SelfMoveTime.ToString());

        PushToStringBuilder(stringbuilder, "IsHitMove", (IsHitMove ? 1 : 0).ToString());
        PushToStringBuilder(stringbuilder, "HitMoveDistance", HitMoveDistance.ToString());
        PushToStringBuilder(stringbuilder, "HitMoveTime", HitMoveTime.ToString());

        PushToStringBuilder(stringbuilder, "IsHitFly", (IsHitFly ? 1 : 0).ToString());
        PushToStringBuilder(stringbuilder, "HitFlyDistance", HitFlyDistance.ToString());
        PushToStringBuilder(stringbuilder, "HitFlyTime", HitFlyTime.ToString());

        PushToStringBuilder(stringbuilder, "ShakeScreen", ShakeScreen.ToString());
        PushToStringBuilder(stringbuilder, "DefaultShakeScene", (DefaultShakeScene ? "1" : "0").ToString());
        stringbuilder.Append("\t\t<es n='Effects'>\n");
        foreach (EffectInfo aeffectInfo in ActionEffectInfos)
        {
            stringbuilder.Append(aeffectInfo.ToXMLString());
        }
        foreach (EffectInfo heffectInfo in HitEffectInfos)
        {
            stringbuilder.Append(heffectInfo.ToXMLString());
        }
        stringbuilder.Append("\t\t</es>\n");
        stringbuilder.Append("\t</as>\n");
        return stringbuilder.ToString();
    }


    private void PushToStringBuilder(StringBuilder stringbuilder, string title, string content)
    {
        stringbuilder.Append("\t\t<a n='");
        stringbuilder.Append(title);
        stringbuilder.Append("'>");
        stringbuilder.Append(content);
        stringbuilder.Append("</a>\n");
    }
}
