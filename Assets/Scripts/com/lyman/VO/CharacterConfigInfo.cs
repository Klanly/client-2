using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
public class CharacterConfigInfo
{
    public string ModelName;
    public float WalkSpeed = 1.0f;
    public float RunSpeed = 1.5f;
    public string LeftWeaponName = BindTypes.NONE;
    public string RightWeaponName = BindTypes.NONE;
    
    private Dictionary<string, ActionInfo> actionInfos = new Dictionary<string, ActionInfo>();

    public ActionInfo GetActionInfo(string actionName)
    {
        if (string.IsNullOrEmpty(actionName)) return null;
        ActionInfo actionInfo;
        actionInfos.TryGetValue(actionName, out actionInfo);
        if (actionInfo == null)
        {
            actionInfo = new ActionInfo();
            actionInfo.ActionName = actionName;
            actionInfos.Add(actionName, actionInfo);
        }
        return actionInfo;
    }
    public void AddActionInfo(ActionInfo actionInfo)
    {
        bool container = actionInfos.ContainsKey(actionInfo.ActionName);
        if (!container)
        {
            actionInfos.Add(actionInfo.ActionName, actionInfo);
        }
    }

    public string ToXMLString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table n='CharacterConfigInfo'>\n");
        PushToStringBuilder(stringBuilder, "ModelName", ModelName);
        PushToStringBuilder(stringBuilder, "WalkSpeed", WalkSpeed.ToString());
        PushToStringBuilder(stringBuilder, "RunSpeed", RunSpeed.ToString());
        PushToStringBuilder(stringBuilder, "LeftWeaponName", LeftWeaponName);
        PushToStringBuilder(stringBuilder, "RightWeaponName", RightWeaponName);
        foreach (string actionn in AnimationType.GetActionList())
        {
            ActionInfo actionInfo = GetActionInfo(actionn);
            stringBuilder.Append(actionInfo.ToXMLString());
        }
        stringBuilder.Append("</table>");
        return stringBuilder.ToString();
    }


    private void PushToStringBuilder(StringBuilder stringBuilder, string title, string content)
    {
        stringBuilder.Append("\t<p n='");
        stringBuilder.Append(title);
        stringBuilder.Append("'>");
        stringBuilder.Append(content);
        stringBuilder.Append("</p>\n");
    }

}
