using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
public class SceneInfo
{
    private bool saveGridsByBytes;

    public bool SaveGridsByBytes
    {
        get { return saveGridsByBytes; }
        set { saveGridsByBytes = value; }
    }


    private string gridsContent = string.Empty;
    private string sceneName = string.Empty;
    private GameObjectInfo terrainInfo;
    private List<GameObjectInfo> blocks = new List<GameObjectInfo>();
    private List<GameObjectInfo> nonblocks = new List<GameObjectInfo>();
    private List<GameObjectInfo> effects = new List<GameObjectInfo>();
    private List<GameObjectInfo> lights = new List<GameObjectInfo>();

    private List<GameObjectInfo> allGameObjectInfos = new List<GameObjectInfo>();


    public List<GameObjectInfo> AllGameObjectInfos
    {
        get { return allGameObjectInfos; }
    }

    private int xLength;
    private int zLength;
    private float harfXLength;
    private float harfZLength;

    private int offsetX;
    private int offsetZ;

    private byte[,] grids;

    public byte[,] Grids
    {
        get { return grids; }
    }

    private float[,] yPositions;

    private static float gridSize = 1f;
    private float harfGridSize = gridSize / 2f;


    public List<GameObjectInfo> NonBlocks
    {
        get { return nonblocks; }
    }

    public List<GameObjectInfo> Blocks
    {
        get { return blocks; }
    }

    public List<GameObjectInfo> Effects
    {
        get { return effects; }
    }
    public List<GameObjectInfo> Lights
    {
        get { return lights; }
    }

    public void AddGameObjectInfo(GameObjectInfo gameObjectInfo)
    {
        if (gameObjectInfo == null) return;
        if (gameObjectInfo.Type != GameObjectTypes.Terrain)
        {
            allGameObjectInfos.Add(gameObjectInfo);
            if (gameObjectInfo.Type == GameObjectTypes.Effect)
            {
                effects.Add(gameObjectInfo);
            }
            else if (gameObjectInfo.Type == GameObjectTypes.Block)
            {
                blocks.Add(gameObjectInfo);
            }
            else if (gameObjectInfo.Type == GameObjectTypes.NonBlock)
            {
                nonblocks.Add(gameObjectInfo);
            }
            else if (gameObjectInfo.Type == GameObjectTypes.Light)
            {
                lights.Add(gameObjectInfo);
            }
        }
        else
        {
            terrainInfo = gameObjectInfo;
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
        get { return gridsContent; }
    }
    private byte[] gridsBytes;
    public byte[] GridsBytes
    {
        set
        {
            gridsBytes = value;
        }
        get { return gridsBytes; }
    }


    private bool isParsed = false;
    private string[] splits;
    public void ParseGrids()
    {
        if (isParsed)
            return;
        isParsed = true;
        //System.Diagnostics.Stopwatch stopWatch;
        splits = gridsContent.Split('/');

        string[] temp = splits[0].Split(',');
        xLength = int.Parse(temp[0]);
        harfXLength = (float)xLength / 2;
        zLength = int.Parse(temp[1]);
        harfZLength = (float)zLength / 2;
        offsetX = int.Parse(temp[2]);
        offsetZ = int.Parse(temp[3]);

        int length = splits.Length;
        //Debug.LogError("length:"+ length + " / xLength:" + xLength+ " / zLength:"+ zLength);

        grids = new byte[xLength, zLength];
        yPositions = new float[xLength, zLength];
        //stopWatch = new System.Diagnostics.Stopwatch();
        //stopWatch.Start();
        int xIndex = 0;
        int yIndex = 0;
        for (int i = 1; i < length; ++i)
        {
            temp = splits[i].Split(',');
            grids[xIndex, yIndex] = byte.Parse(temp[0]);
            float y = float.Parse(temp[1]);
            yPositions[xIndex, yIndex] = y;
            yIndex++;
            if (yIndex == zLength)
            {
                yIndex = 0;
                xIndex++;
            }
        }
        //stopWatch.Stop();
        //Debug.LogError("解析" + SceneName + ".xml用时:" + stopWatch.ElapsedMilliseconds);
    }


    private bool parseed = false;

    public bool Parsed
    {
        get { return parseed; }
    }

    public void ParseGridsByBytes()
    {
        if (parseed) return;
        parseed = true;
        ByteBuffer byteBuffer = new ByteBuffer(gridsBytes);
        xLength = byteBuffer.ReadUShort();
        zLength = byteBuffer.ReadUShort();
        harfXLength = (float)xLength / 2;
        harfZLength = (float)zLength / 2;
        offsetX = byteBuffer.ReadUShort();
        offsetZ = byteBuffer.ReadUShort();
        grids = new byte[xLength, zLength];
        yPositions = new float[xLength, zLength];
        for (int i = 0; i < xLength; ++i)
        {
            for (int j = 0; j < zLength; ++j)
            {
                grids[i, j] = (byte)(byteBuffer.ReadBoolean() ? 1 : 0);
                yPositions[i, j] = (float)byteBuffer.ReadInt32() / 100f;
            }
        }
    }



    private Vector3 tempVector3 = Vector3.zero;
    public Vector3 GridToPixel(int xIndex,int zIndex)
    {
        float x = -harfXLength + offsetX + xIndex * gridSize + harfGridSize; 
        float y = yPositions[xIndex, zIndex];
        float z = -harfZLength + offsetZ + zIndex * gridSize + harfGridSize;
        tempVector3.Set(x,y,z);
        return tempVector3;
    }

    private Vector2 tempVector2 = Vector2.zero;
    public Vector2 PixelToGrid(float x,float z)
    {

        bool xIsI = x - (int)x == 0f;
        if(!xIsI)
            x = (int)x + (x >= 0f ? harfGridSize : -harfGridSize);

        bool zIsI = z - (int)z == 0f;
        if(!zIsI)
            z = (int)z + (z >= 0f ? harfGridSize : -harfGridSize);

        int xIndex = (int)((x + harfXLength - offsetX - (xIsI ? 0 : harfGridSize)) / gridSize);


        int zIndex = (int)((z + harfZLength - offsetZ - (zIsI ? 0 : harfGridSize)) / gridSize);
        tempVector2.Set(xIndex, zIndex);
        return tempVector2;
    }


    public string ToXMLString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<?xml version='1.0' encoding='utf-8'?>\n");
        stringBuilder.Append("<table n='SceneConfig'>\n");
        allGameObjectInfos.Add(terrainInfo);
        GameObjectInfo gameObjectInfo = null;
        int length = allGameObjectInfos.Count;
        for (int i = 0; i < length; ++i)
        {
            stringBuilder.Append("\t");
            gameObjectInfo = allGameObjectInfos[i];
            stringBuilder.Append(gameObjectInfo.ToXMLString());
            stringBuilder.Append("\n");
        }
        allGameObjectInfos.Remove(terrainInfo);

        //stringBuilder.Append("\t");
        //stringBuilder.Append("<a n='GS'>");
        //stringBuilder.Append(gridsContent);
        //stringBuilder.Append("</a>");
        //stringBuilder.Append("\n");

        stringBuilder.Append("</table>");
        return stringBuilder.ToString();
    }

    public byte[] GameObjectsToBytes()
    {
        ByteBuffer byteBuffer = new ByteBuffer();
        if(!allGameObjectInfos.Contains(terrainInfo))
            allGameObjectInfos.Add(terrainInfo);
        GameObjectInfo gameObjectInfo = null;
        Byte[] bytes;
        int length = allGameObjectInfos.Count;
        byteBuffer.WriteUShort((ushort)(length));

        for (int i = 0; i < length; ++i)
        {
            gameObjectInfo = allGameObjectInfos[i];
            bytes = gameObjectInfo.ToBytes();
            byteBuffer.WriteInt(bytes.Length);
            byteBuffer.WriteBytes(bytes);
        }
        allGameObjectInfos.Remove(terrainInfo);
        return byteBuffer.ToBytes();
    } 

}

public class GameObjectInfo
{

    private static int index = 0;

    public static int Index
    {
        get
        {
            index++;
            return index;
        }
    }

    public  GameObjectInfo()
    {
        myIndex = Index;
    }

    
    public string PrefabName;
    public int myIndex;
    public float X;
    public float Y;
    public float Z;
    public float RotationX;
    public float RotationY;
    public float RotationZ;
    public float ScaleX;
    public float ScaleY;
    public float ScaleZ;
    private int type;

    //阻挡物才有的属性
    public int ColliderType;
    public float Radius;
    public float Height;

    //灯光才有的属性
    public int LightType;
    public string color;
    public int Mode;
    public float Intensity;
    public float IndirectMultiplier;
    public int ShadowType;
    public float BakeShadowAngle;
    public int DrawHalo;
    public int RenderMode;
    public int CollingMask;
    


    public int Type
    {
        get { return type; }
        set
        {
            type = value;
        }
    }

    public string ToXMLString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<a n='G'>");
        stringBuilder.Append(PrefabName);
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
        stringBuilder.Append(",");

        stringBuilder.Append(type.ToString());
        if (type == GameObjectTypes.Block)
        {
            stringBuilder.Append(",");
            stringBuilder.Append(ColliderType.ToString());

            if (ColliderType == ColliderTypes.SphereCollider || ColliderType == ColliderTypes.CapsuleCollider)
            {
                stringBuilder.Append(",");
                stringBuilder.Append(Radius.ToString("0.0"));
            }
            if (ColliderType == ColliderTypes.CapsuleCollider)
            {
                stringBuilder.Append(",");
                stringBuilder.Append(Height.ToString("0.0"));
            }
        }
        stringBuilder.Append("</a>");
        return stringBuilder.ToString();
    }

    public byte[] ToBytes()
    {
        ByteBuffer byteBuffer = new ByteBuffer();
        byteBuffer.WriteByte((Byte)PrefabName.Length);
        byteBuffer.WriteString(PrefabName);

        byteBuffer.WriteInt((int)(X * 100f));
        byteBuffer.WriteInt((int)(Y * 100f));
        byteBuffer.WriteInt((int)(Z * 100f));

        byteBuffer.WriteShort((short)(RotationX * 100f));
        byteBuffer.WriteShort((short)(RotationX * 100f));
        byteBuffer.WriteShort((short)(RotationX * 100f));


        byteBuffer.WriteShort((short)(ScaleX * 100f));
        byteBuffer.WriteShort((short)(ScaleY * 100f));
        byteBuffer.WriteShort((short)(ScaleZ * 100f));

        byteBuffer.WriteByte((Byte)(type));
        if (type != GameObjectTypes.Block) return byteBuffer.ToBytes();
        byteBuffer.WriteByte((Byte)(ColliderType));
        byteBuffer.WriteShort((Byte)(Radius*100f));
        if (ColliderType == ColliderTypes.CapsuleCollider)
            byteBuffer.WriteShort((Byte)(Height*100f));

        return byteBuffer.ToBytes();
    }

    public void ParseByBytes(byte[] bytes)
    {
        ByteBuffer byteBuffer = new ByteBuffer(bytes);
        byte length = byteBuffer.ReadByte();
        PrefabName = byteBuffer.ReadString((int)length);
        X = (float)byteBuffer.ReadInt32() / 100f;
        Y = (float)byteBuffer.ReadInt32() / 100f;
        Z = (float)byteBuffer.ReadInt32() / 100f;

        RotationX = (float)byteBuffer.ReadShort() / 100f;
        RotationY = (float)byteBuffer.ReadShort() / 100f;
        RotationZ = (float)byteBuffer.ReadShort() / 100f;

        ScaleX = (float)byteBuffer.ReadShort() / 100f;
        ScaleY = (float)byteBuffer.ReadShort() / 100f;
        ScaleZ = (float)byteBuffer.ReadShort() / 100f;

        type = byteBuffer.ReadByte();
        if (type != GameObjectTypes.Block) return;
        ColliderType = byteBuffer.ReadByte();
        Radius = (float)byteBuffer.ReadShort() / 100f;
        if (ColliderType == ColliderTypes.CapsuleCollider)
            Height = (float)byteBuffer.ReadShort() / 100f;
    }

}
