using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
public class SceneInfo
{
    private string gridsContent = string.Empty;
    private string sceneName = string.Empty;
    private GameObjectInfo terrainInfo;
    private List<GameObjectInfo> GameObjectInfos = new List<GameObjectInfo>();

    private int xLength;
    private int zLength;

    private int offsetX;
    private int offsetZ;

    private byte[,] grids;
    private float[,] yPositions;

    private float gridSize = 1f;

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
        set
        {
            gridsContent = value;
        }
    }

    public void ParseGrids()
    {
        System.Diagnostics.Stopwatch stopWatch;
        
        string[] splits = gridsContent.Split('/');
        
        string[] temp = splits[0].Split(',');
        xLength = int.Parse(temp[0]);
        zLength = int.Parse(temp[1]);
        offsetX = int.Parse(temp[2]);
        offsetZ = int.Parse(temp[3]);
        
        int length = splits.Length;
        Debug.LogError("length:" + length);
        
        grids = new byte[xLength, zLength];
        yPositions = new float[xLength, zLength];
        stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        for (int i = 1; i < length; ++i)
        {
            temp = splits[i].Split(',');
            int xIndex = int.Parse(temp[0]);
            int yIndex = int.Parse(temp[1]);
            grids[xIndex, yIndex] = byte.Parse(temp[2]);
            float y = float.Parse(temp[3]);
            yPositions[xIndex, yIndex] = y;
        }
        stopWatch.Stop();
        Debug.LogError("4:" + stopWatch.ElapsedMilliseconds);
    }

    private Vector3 tempVector3 = Vector3.zero;
    public Vector3 GridToPixel(int xIndex,int zIndex)
    {
        float x = -(float)xLength/2f + offsetX + xIndex * gridSize + gridSize/2f; 
        float y = yPositions[xIndex, zIndex];
        float z = -(float)zLength/2f + offsetZ + zIndex * gridSize + gridSize/2f;
        tempVector3.Set(x,y,z);
        return tempVector3;
    }

    private Vector2 tempVector2 = Vector2.zero;
    public Vector2 PixelToGrid(float x,float z)
    {
        x = (int)x + gridSize / 2f;
        z = (int)z + gridSize / 2f;
        int xIndex = (int)((x + (float)xLength/2f - offsetX - gridSize/2f) / gridSize);
        int zIndex = (int)((z + (float)zLength/2f - offsetZ - gridSize/2f) / gridSize);
        return tempVector2;
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
        stringBuilder.Append("<a n='GS'>");
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
        stringBuilder.Append("<a n='G'>");
        stringBuilder.Append(GameObjectName);
        stringBuilder.Append(",");
        stringBuilder.Append(IsTerrain ? "1" : "0");
        stringBuilder.Append(",");
        stringBuilder.Append(X.ToString("0.00"));
        stringBuilder.Append(",");
        stringBuilder.Append(Y.ToString("0.00"));
        stringBuilder.Append(",");
        stringBuilder.Append(Z.ToString("0.00"));
        stringBuilder.Append(",");

        stringBuilder.Append(RotationX.ToString("0.00"));
        stringBuilder.Append(",");
        stringBuilder.Append(RotationY.ToString("0.00"));
        stringBuilder.Append(",");
        stringBuilder.Append(RotationZ.ToString("0.00"));
        stringBuilder.Append(",");

        stringBuilder.Append(ScaleX.ToString("0.00"));
        stringBuilder.Append(",");
        stringBuilder.Append(ScaleY.ToString("0.00"));
        stringBuilder.Append(",");
        stringBuilder.Append(ScaleZ.ToString("0.00"));

        stringBuilder.Append("</a>");
        return stringBuilder.ToString();
    }

}
