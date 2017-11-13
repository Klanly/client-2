using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
public class SceneInfo
{
    private string gridsContent;
    private string sceneName = string.Empty;
    private GameObjectInfo terrainInfo;
    private List<GameObjectInfo> GameObjectInfos = new List<GameObjectInfo>();

    public void AddGameObjectInfo(GameObjectInfo gameObjectInfo)
    {
        if (gameObjectInfo == null) return;
        if (gameObjectInfo.IsTerrain)
        {
            terrainInfo = gameObjectInfo;
        }
        else
        {
            GameObjectInfos.Add(gameObjectInfo);
        }
    }

    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; }
    }


    public GameObjectInfo TerrainInfo
    {
        get { return terrainInfo; }
    }

    public string GridsContent
    {
        set { gridsContent = value; }
    }


    public string ToXMLString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<?xml version='1.0' encoding='utf-8'?>\n");
        stringBuilder.Append("<table n='SceneConfig'>\n");
        GameObjectInfos.Add(terrainInfo);
        int length = GameObjectInfos.Count;
        for (int i = 0; i < length; ++i)
        {
            stringBuilder.Append("\t");
            GameObjectInfo gameObjectInfo = GameObjectInfos[i];
            stringBuilder.Append(gameObjectInfo.ToXMLString());
            stringBuilder.Append("\n");
        }

        stringBuilder.Append("\t");
        stringBuilder.Append("<a n='Grids'>");
        stringBuilder.Append(gridsContent);
        stringBuilder.Append("</a>");
        stringBuilder.Append("\n");

        stringBuilder.Append("</table>");
        return stringBuilder.ToString();
    }


}

public class GameObjectInfo
{
    public bool IsTerrain = false;
    public string GameObjectName;
    public float X;
    public float Y;
    public float Z;
    public float RotationX;
    public float RotationY;
    public float RotationZ;
    public float ScaleX;
    public float ScaleY;
    public float ScaleZ;

    public string ToXMLString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<a n='GameObjectInfo'>");
        stringBuilder.Append(GameObjectName);
        stringBuilder.Append(",");
        stringBuilder.Append(IsTerrain ? "1" : "0");
        stringBuilder.Append(",");
        stringBuilder.Append(X.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(Y.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(Z.ToString("0.000"));
        stringBuilder.Append(",");

        stringBuilder.Append(RotationX.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(RotationY.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(RotationZ.ToString("0.000"));
        stringBuilder.Append(",");

        stringBuilder.Append(ScaleX.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(ScaleY.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(ScaleZ.ToString("0.000"));

        stringBuilder.Append("</a>");
        return stringBuilder.ToString();
    }

}
