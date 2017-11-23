using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using System.IO;
using System.Text;
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
                gameObjectInfo = GetGameObjectInfo(ts, GameObjectTypes.Terrain);
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

                    //Debug.LogError("render.lightmapIndex:" + render.lightmapIndex);
                    //Debug.LogError("render.lightmapScaleOffset:" + render.lightmapScaleOffset);
                    //    render.lightmapScaleOffset
                    //    render.lightProbeAnchor
                    //    render.lightProbeProxyVolumeOverride
                    //    render.lightProbeUsage
                    //    render.realtimeLightmapIndex
                    //    render.realtimeLightmapScaleOffset
                    //    render.useLightProbes

                    gameObjectInfo = GetGameObjectInfo(ts, GameObjectTypes.NonBlock);
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
                    gameObjectInfo = GetGameObjectInfo(ts, GameObjectTypes.Block);
                    sceneInfo.AddGameObjectInfo(gameObjectInfo);
                }
            }
            else if (go.name == "effects")
            {
                count = go.transform.childCount;
                for (j = 0; j < count; ++j)
                {
                    ts = go.transform.GetChild(j);
                    if (!ts.gameObject.activeSelf) continue;
                    if (ts.tag != GameObjectTags.Scene_Effect)
                    {
                        throw new System.Exception("场景特效节点下有名称为:" + ts.name + "的GameObject的Tag不为scene_effect,请重新设置！！！");
                    }
                    gameObjectInfo = GetGameObjectInfo(ts, GameObjectTypes.Effect);
                    sceneInfo.AddGameObjectInfo(gameObjectInfo);
                }
            }
            else if (go.name == "lights")
            {
                count = go.transform.childCount;
                for (j = 0; j < count; ++j)
                {
                    ts = go.transform.GetChild(j);
                    if (!ts.gameObject.activeSelf) continue;
                    gameObjectInfo = GetGameObjectInfo(ts, GameObjectTypes.Light);
                    sceneInfo.AddGameObjectInfo(gameObjectInfo);
                }
            }
        }
        if (!isHaveTerrain)
            throw new System.Exception("该地图没有地形GameObject，请添加!!!");
        if (nonblocks != null)
        {
            nonblocks.SetActive(false);
        }
        StringBuilder strinBuilder = new StringBuilder();
        if (terrainGo != null)
        {
            MeshRenderer meshRender = terrainGo.GetComponent<MeshRenderer>();
            Vector3 size = meshRender.bounds.size;
            Vector3 center = meshRender.bounds.center;


            float allX = size.x;// + Mathf.Abs(center.x)*2f;
            float allZ = size.z;// + Mathf.Abs(center.z)*2f;

            int xLength = 0;
            int zLength = 0;
            if (allX % 2f != 0f)
            {
                xLength = (int)allX + 1;
                if (xLength % 2 != 0)
                {
                    xLength++;
                }
            }
            else
            {
                xLength = (int)allX;
            }
            if (allZ % 2f != 0f)
            {
                zLength = (int)allZ + 1;
                if (zLength % 2 != 0)
                {
                    zLength++;
                }
            }
            else
            {
                zLength = (int)allZ;
            }

            byte[,] grids = new byte[xLength, zLength];
            

            strinBuilder.Append(xLength.ToString());
            strinBuilder.Append(",");
            strinBuilder.Append(zLength.ToString());
            strinBuilder.Append(",");


            Debug.Log("xLength:"+xLength+" / zLength:"+zLength);

            int offsetZ = 0;
            if (center.z != 0f)
            {
                offsetZ = center.z - (int)center.z != 0f ? (int)center.z + (center.z > 0f ? 1 : -1) : (int)center.z;
            }
            int offsetX = 0;
            if(center.x != 0f)
                offsetX = center.x - (int)center.x != 0f ? (int)center.x + (center.x > 0f ? 1:-1) : (int)center.x;

            strinBuilder.Append(offsetX.ToString());
            strinBuilder.Append(",");
            strinBuilder.Append(offsetZ.ToString());
            strinBuilder.Append("/");

            

            //Debug.Log("offsetZ:" + offsetZ + " / offsetX:" + offsetX);

            int startX = -xLength / 2 + offsetX;
            int endX = xLength / 2 + offsetX;
            //Debug.Log("startX:" + startX + " / endX:" + endX);

            int startZ = -zLength / 2 + offsetZ;
            int endZ = zLength / 2 + offsetZ;

            //Debug.Log("startZ:" + startZ + " / endZ:" + endZ);
            
            int x = 0;
            
            for (int i = startX; i < endX; ++i)
            {
                float xx = i + 0.5f;
                int y = 0;
                for (j = startZ; j < endZ; ++j)
                {
                    float yy = j + 0.5f;
                    //todo 射线检测
                    //strinBuilder.Append(x.ToString());
                    //strinBuilder.Append(",");
                    //strinBuilder.Append(y.ToString());
                    //strinBuilder.Append(",");
                    Vector3 position = Vector3.zero;
                    RaycastHit raycastHit;
                    Vector3 startPosition = new Vector3(xx, 30f, yy);
                    bool isHit = Physics.Raycast(startPosition, Vector3.down*50f, out raycastHit);
                    if (isHit)
                    {
                        position = raycastHit.point;
                        if (raycastHit.transform.tag == GameObjectTags.Block)
                        {
                            grids[x, y] = 0;
                        }
                        else if(raycastHit.transform.tag == GameObjectTags.Terrain)
                        {
                            //Debug.Log(raycastHit.point);
                            grids[x, y] = 1;
                        }
                    }
                    else
                    {
                        grids[x, y] = 0;
                    }
                    strinBuilder.Append(grids[x, y].ToString());
                    strinBuilder.Append(",");
                   // strinBuilder.Append(position.x.ToString("0.00"));
                   // strinBuilder.Append(",");
                    strinBuilder.Append(position.y.ToString("0.00"));
                    //strinBuilder.Append(",");
                    //strinBuilder.Append(position.z.ToString("0.00"));
                    if (x == xLength - 1 && y == zLength - 1)
                    {

                    }
                    else
                    {
                        strinBuilder.Append("/");
                    }
                    //Debug.Log(x + "_" + y + ": " + grids[x, y] + "/" + xx + "/" + yy);
                    y++;
                }
                //Debug.Log("y:" + y);
                x++;

            }
            Debug.Log("x:"+x);
        }
        sceneInfo.GridsContent = strinBuilder.ToString();
        if (nonblocks != null)
        {
            nonblocks.SetActive(true);
        }

        SaveConfig(sceneInfo, scene.path);

        AssetDatabase.Refresh();
    }


    private static GameObjectInfo GetGameObjectInfo(Transform ts, int type)
    {
        if (ts == null) return null;
        GameObjectInfo gameObjectInfo = new GameObjectInfo();
        gameObjectInfo.Type = type;
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
        if (type == GameObjectTypes.Block)
        {

            CapsuleCollider capsuleCollider = ts.GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                //对于CapsuleCollider类型的限定美术把CapsuleCollider的Center属性设置成0,0,0;Direction设置成Y-Axis,这样可以减小配置表大小
                gameObjectInfo.ColliderType = ColliderTypes.CapsuleCollider;
                gameObjectInfo.Radius = capsuleCollider.radius;
                gameObjectInfo.Height = capsuleCollider.height;
            }
            else
            {
                BoxCollider boxCollider = ts.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    //对于BoxCollider类型的限定美术把BoxCollider的Center属性设置成0,0,0  Size属性设置成1,1,1这样可以减小配置表大小
                    gameObjectInfo.ColliderType = ColliderTypes.BoxCollider;
                }
                else
                {
                    //对于SphereCollider类型的限定美术把SphereCollider的Center属性设置成0,0,0;这样可以减小配置表大小
                    SphereCollider sphereCollider = ts.GetComponent<SphereCollider>();
                    if (sphereCollider)
                    {
                        gameObjectInfo.ColliderType = ColliderTypes.SphereCollider;
                        gameObjectInfo.Radius = sphereCollider.radius;
                    }
                }
            }
        }
        else if (type == GameObjectTypes.Light)
        {

            Light light = ts.GetComponent<Light>();
            gameObjectInfo.LightType = (int)light.type;
            //if (light.type == LightType.Area)
            //{
            //    gameObjectInfo.LightType = 3;
            //}
            //else if (light.type == LightType.Directional)
            //{
            //    gameObjectInfo.LightType = 1;
            //}
            //else if (light.type == LightType.Point)
            //{
            //    gameObjectInfo.LightType = 2;
            //}
            //else if (light.type == LightType.Spot)
            //{
            //    gameObjectInfo.LightType = 0;
            //}
            gameObjectInfo.color = light.color.r + "_" + light.color.g + "_" + light.color.b + "_" + light.color.a;
            //gameObjectInfo.Mode = light.renderMode


        }
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

    [MenuItem("GameTools/保存该场景预制件的烘焙信息", false, 6)]
    public static void SaveLightmapInfoByGameObject()
    {
        GameObject go = Selection.activeGameObject;

        if (null == go) return;

        PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
        if (data == null)
        {
            data = go.AddComponent<PrefabLightmapData>();
        }
        //save lightmapdata info by mesh.render
        data.SaveLightmap();

        EditorUtility.SetDirty(go);
        //applay prefab
        PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
    }

}




