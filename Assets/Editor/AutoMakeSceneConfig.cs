﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using System.IO;
public class AutoMakeSceneConfig
{

    [MenuItem("GameTools/自动生成场景配置", false, 5)]
    public static void AutoMakeScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetActiveScene();
        Debug.Log("scene name:" + scene.name + " / scene path:" + scene.path);
        GameObject[] gos = scene.GetRootGameObjects();
        SceneInfo sceneInfo = new SceneInfo();
        int count = 0;
        int j;
        
        bool isHaveTerrain = false;
        GameObject nonblocks = null;
        GameObject terrainGo = null;
        for (int i = 0; i < gos.Length; ++i)
        {
            GameObject go = gos[i];
            Transform ts;
            GameObjectInfo gameObjectInfo;
            if (go.name == GameObjectTags.Terrain)
            {
                //地形
                count = go.transform.childCount;
                if (count > 1)
                {
                    throw new System.Exception("地形节点下只允许有一个地形GameObject!!!");
                }
                else if (count == 0)
                {
                    throw new System.Exception("地形节点没有一个地形GameObject，请添加!!!");
                }
                isHaveTerrain = true;
                ts = go.transform.GetChild(0);
                terrainGo = ts.gameObject;
                if (ts.tag != GameObjectTags.Terrain)
                {
                    throw new System.Exception("地形节点下有名称为:" + ts.name + "的GameObject的Tag不为terrain,请重新设置！！！");
                }

                Collider collider = ts.GetComponent<Collider>();
                if (collider == null)
                {
                    throw new System.Exception("地形节点下有名称为:" + ts.name + "的GameObject的不带有碰撞组件，请添加！！！");
                }
                Renderer render = ts.GetComponent<Renderer>();
                if (render == null || (render != null && !render.enabled))
                {
                    if (render == null)
                        throw new System.Exception("地形节点下有名称为:" + ts.name + "的GameObject不带有渲染组件，请添加渲染组件！！！");
                    else
                        throw new System.Exception("地形节点下有名称为:" + ts.name + "的GameObject的渲染组件未激活，请激活渲染组件！！！");
                }
                gameObjectInfo = GetGameObjectInfo(ts, true);
                sceneInfo.AddGameObjectInfo(gameObjectInfo);
            }
            else if (go.name == "nonblocks")
            {
                //非阻挡
                count = go.transform.childCount;
                nonblocks = go;
                for (j = 0; j < count; ++j)
                {
                    ts = go.transform.GetChild(j);
                    if (!ts.gameObject.activeSelf) continue;
                    if (ts.tag != GameObjectTags.NON_Block)
                    {
                        throw new System.Exception("非阻挡节点下有名称为:" + ts.name + "的GameObject的Tag不为nonblock,请重新设置！！！");
                    }
                    Collider collider = ts.GetComponent<Collider>();
                    if (collider != null)
                    {
                        throw new System.Exception("非阻挡节点下有名称为:" + ts.name + "的GameObject带有碰撞组件，请删除！！！");
                    }
                    Renderer render = ts.GetComponent<Renderer>();
                    if (render == null || (render != null && !render.enabled))
                    {
                        if (render == null)
                            throw new System.Exception("非阻挡节点下有名称为:" + ts.name + "的GameObject不带有渲染组件，请添加渲染组件！！！");
                        else
                            throw new System.Exception("非阻挡节点下有名称为:" + ts.name + "的GameObject的渲染组件未激活，请激活渲染组件！！！");
                    }
                    gameObjectInfo = GetGameObjectInfo(ts);
                    sceneInfo.AddGameObjectInfo(gameObjectInfo);
                }
            }
            else if (go.name == "blocks")
            {
                count = go.transform.childCount;
                for (j = 0; j < count; ++j)
                {
                    ts = go.transform.GetChild(j);
                    if (!ts.gameObject.activeSelf)
                    {
                        continue;
                    }
                    if (ts.tag != GameObjectTags.Block)
                    {
                        throw new System.Exception("阻挡节点下有名称为:" + ts.name + "的GameObject的Tag不为block,请重新设置！！！");
                    }
                    Collider collider = ts.GetComponent<Collider>();
                    if (collider == null || (collider != null && !collider.enabled))
                    {
                        if (collider == null)
                            throw new System.Exception("阻挡节点下有名称为:" + ts.name + "的GameObject没有碰撞组件，请添加！！！");
                        else
                            throw new System.Exception("阻挡节点下有名称为:" + ts.name + "的GameObject碰撞组件未激活，请激活！！！");
                    }
                    Renderer render = ts.GetComponent<Renderer>();
                    if (render != null)
                    {
                        throw new System.Exception("阻挡节点下有名称为:" + ts.name + "的GameObject带有渲染组件，请删除渲染组件！！！");
                    }
                }
            }
        }
        if (!isHaveTerrain)
            throw new System.Exception("该地图没有地形GameObject，请添加!!!");
        if (nonblocks != null)
        {
            nonblocks.SetActive(false);
        }
        if (terrainGo != null)
        {
            MeshRenderer meshRender = terrainGo.GetComponent<MeshRenderer>();
            Vector3 vector3 = meshRender.bounds.size;
            int xLenght = 0;
            int zLength = 0;
            if (vector3.x % 2f != 0f)
            {
                xLenght = (int)vector3.x + 1;
                if (xLenght % 2 != 0)
                {
                    xLenght++;
                }
            }
            if (vector3.z % 2f != 0f)
            {
                zLength = (int)vector3.z + 1;
                if (zLength % 2 != 0)
                {
                    zLength++;
                }
            }

            byte[,] grids = new byte[xLenght, zLength];

            for (int i = -xLenght / 2; i < xLenght; ++i)
            {
                for (j = -zLength / 2; j < zLength; ++j)
                {
                    int x = i + xLenght / 2;
                    float xx = i + 0.5f;

                    int y = j + zLength / 2;
                    float yy = j + 0.5f;

                    //todo 射线检测
                    
                    RaycastHit raycastHit;
                    Vector3 startPosition = new Vector3(xx, 10f, yy);
                    Ray ray = new Ray(startPosition, Vector3.down);
                    bool isHit = Physics.Raycast(ray, out raycastHit, 20f);
                    if (isHit)
                    {
                        if (raycastHit.transform.tag == GameObjectTags.Block)
                        {
                            grids[x, y] = 0;
                        }
                        else
                        {
                            grids[x, y] = 1;
                        }
                    }
                    else
                    {
                        grids[x, y] = 0;
                    }
                }
            }
            Debug.Log(grids);
        }
        


        //SaveConfig(sceneInfo, scene.path);
    }


    private static GameObjectInfo GetGameObjectInfo(Transform ts, bool isTerrain = false)
    {
        if (ts == null) return null;
        GameObjectInfo gameObjectInfo = new GameObjectInfo();
        gameObjectInfo.IsTerrain = isTerrain;
        gameObjectInfo.GameObjectName = ts.gameObject.name;
        gameObjectInfo.X = ts.position.x;
        gameObjectInfo.Y = ts.position.y;
        gameObjectInfo.Z = ts.position.z;
        gameObjectInfo.RotationX = ts.eulerAngles.x;
        gameObjectInfo.RotationY = ts.eulerAngles.y;
        gameObjectInfo.RotationZ = ts.eulerAngles.z;
        gameObjectInfo.ScaleX = ts.localScale.x;
        gameObjectInfo.ScaleY = ts.localScale.y;
        gameObjectInfo.ScaleZ = ts.localScale.z;
        return gameObjectInfo;
    }


    private static void SaveConfig(SceneInfo sceneInfo, string path)
    {
        string content = sceneInfo.ToXMLString();
        string name = Path.GetFileNameWithoutExtension(path);
        path = Application.dataPath + "/ArtAssets/prefabs/configs/" + name + ".xml";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        StreamWriter SW;
        SW = File.CreateText(path);
        SW.WriteLine(content);
        SW.Close();
    }
    
}




