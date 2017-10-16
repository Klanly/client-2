using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
public class EffectInfo
{
    private static int index;
    public bool IsLoop;
    private int myIndex;
    public string Title = string.Empty;
    public string EffectName = BindTypes.NONE;//特效名称
    public int EffectType = EffectTypes.Normal;//特效类型
    public string SoundName = BindTypes.NONE;//对应音效名称
    public float SoundPlayDelayTime = 0f;   //音效播放延迟时间
    public int BindType = BindTypes.None;//绑定对象类型
    public string BindName = BindTypes.NONE;//绑定对象名称
    
    public bool ColliderDisappear = true;//碰撞后是否消失

    public EffectInfo()
    {
        index++;
        myIndex = index;
    }

    public int Index
    {
        get { return myIndex; }
    }

    public EffectInfo Clone()
    {
        EffectInfo effectInfo = new EffectInfo();
        effectInfo.Title = Title;
        effectInfo.EffectName = EffectName;
        effectInfo.EffectType = EffectType;
        effectInfo.SoundName = SoundName;
        effectInfo.SoundPlayDelayTime = SoundPlayDelayTime;
        effectInfo.BindType = BindType;
        effectInfo.BindName = BindName;
        effectInfo.IsLoop = IsLoop;
        effectInfo.ColliderDisappear = ColliderDisappear;
        return effectInfo;
    }

    public string ToXMLString()
    {
        string title = Title.ToLower();
        string titles = title + "s";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("\t\t\t<");
        stringBuilder.Append(titles);
        stringBuilder.Append(" n='");
        stringBuilder.Append(titles);
        stringBuilder.Append("'>\n");
        PushToStringBuilder(stringBuilder, title, "EffectName", EffectName);
        PushToStringBuilder(stringBuilder, title, "EffectType", EffectType.ToString());
        PushToStringBuilder(stringBuilder, title, "SoundName", SoundName);
        PushToStringBuilder(stringBuilder, title, "SoundPlayDelayTime", SoundPlayDelayTime.ToString());
        PushToStringBuilder(stringBuilder, title, "BindType", BindType.ToString());
        PushToStringBuilder(stringBuilder, title, "BindName", BindName);
        PushToStringBuilder(stringBuilder, title, "IsLoop", IsLoop ? "1" : "0");
        PushToStringBuilder(stringBuilder, title, "ColliderDisappear", (ColliderDisappear ? 1 : 0).ToString());
        stringBuilder.Append("\t\t\t</");
        stringBuilder.Append(titles);
        stringBuilder.Append(">\n");
        return stringBuilder.ToString();
    }

    private void PushToStringBuilder(StringBuilder stringBuilder,string title,string n,string content)
    {
        stringBuilder.Append("\t\t\t\t<");
        stringBuilder.Append(title);
        stringBuilder.Append(" n='");
        stringBuilder.Append(n);
        stringBuilder.Append("'>");
        stringBuilder.Append(content);
        stringBuilder.Append("</");
        stringBuilder.Append(title);
        stringBuilder.Append(">\n");
    }

}
