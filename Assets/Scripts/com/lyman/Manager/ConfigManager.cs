using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
public class ConfigManager
{

    private static Dictionary<string, ShakeCameraConfigInfo> shakeDir = new Dictionary<string, ShakeCameraConfigInfo>();

    //角色信息配置
    private static Dictionary<string, CharacterConfigInfo> characterConfigInfos = new Dictionary<string, CharacterConfigInfo>();

    //场景配置
    private static Dictionary<string, SceneInfo> sceneConfigInfos = new Dictionary<string, SceneInfo>();

    private static void ParseSceneConfig(string sceneName,XmlNodeList nodeList)
    {
        if (nodeList == null)
        {
            return;
        }
        int count = nodeList.Count;
        SceneInfo sceneInfo = new SceneInfo();
        XmlElement node = nodeList[0] as XmlElement;
        XmlNodeList childList = node.GetElementsByTagName("a");
        for (int j = 0; j < childList.Count; j++)
        {
            GameObjectInfo gameObjectInfo = new GameObjectInfo();
            XmlElement child = childList[j] as XmlElement;
            string value = child.InnerXml;
            string[] values = value.Split(',');
            gameObjectInfo.GameObjectName = values[0];
            gameObjectInfo.IsTerrain = values[1] == "1" ? true : false;
            gameObjectInfo.X = float.Parse(values[2]);
            gameObjectInfo.Y = float.Parse(values[3]);
            gameObjectInfo.Z = float.Parse(values[4]);
            gameObjectInfo.RotationX = float.Parse(values[5]);
            gameObjectInfo.RotationY = float.Parse(values[6]);
            gameObjectInfo.RotationZ = float.Parse(values[7]);
            gameObjectInfo.ScaleX = float.Parse(values[8]);
            gameObjectInfo.ScaleY = float.Parse(values[9]);
            gameObjectInfo.ScaleZ = float.Parse(values[10]);
            sceneInfo.AddGameObjectInfo(gameObjectInfo);
        }
        sceneConfigInfos.Add(sceneName, sceneInfo);
    }

    //获取场景配置信息
    public static SceneInfo GetSceneConfigInfo(string sceneName)
    {
        string name = sceneName.ToLower();
        SceneInfo sceneInfo;
        sceneConfigInfos.TryGetValue(name, out sceneInfo);
        if (sceneInfo == null)
        {
            
            XmlNodeList xmlnode = GetXML(name);
            if (xmlnode != null)
            {
                ParseSceneConfig(name, xmlnode);
                sceneConfigInfos.TryGetValue(name, out sceneInfo);
            }
        }
        return sceneInfo;
    }




    private static void ParseSingleCharacter(XmlNodeList nodeList)
    {
        if (nodeList == null)
        {
            return;
        }
        for (int i = 0; i < nodeList.Count; ++i)
        {
            XmlElement node = nodeList[i] as XmlElement;
            CharacterConfigInfo characterConfigInfo = new CharacterConfigInfo();
            XmlNodeList childList = node.GetElementsByTagName("p");
            for (int j = 0; j < childList.Count; j++)
            {
                XmlElement child = childList[j] as XmlElement;
                string key = child.GetAttribute("n");
                string value = child.InnerXml;
                switch (key)
                {
                    case "ModelName":
                        characterConfigInfo.ModelName = value;
                        break;
                    case "WalkSpeed":
                        float.TryParse(value, out characterConfigInfo.WalkSpeed);
                        break;
                    case "RunSpeed":
                        float.TryParse(value, out characterConfigInfo.RunSpeed);
                        break;
                    case "LeftWeaponName":
                        characterConfigInfo.LeftWeaponName = value;
                        break;
                    case "RightWeaponName":
                        characterConfigInfo.RightWeaponName = value;
                        break;
                }
            }
            childList = node.GetElementsByTagName("as");
            for (int j = 0; j < childList.Count; ++j)
            {
                XmlElement child = childList[j] as XmlElement;
                ActionInfo actionInfo = new ActionInfo();
                XmlNodeList acts = child.GetElementsByTagName("a");
                for (int t = 0; t < acts.Count; t++)
                {
                    XmlElement act = acts[t] as XmlElement;
                    string key = act.GetAttribute("n");
                    string value = act.InnerXml;
                    switch (key)
                    {
                        case "ActionName":
                            actionInfo.ActionName = value;
                            break;
                        case "Length":
                            float.TryParse(value, out actionInfo.Length);
                            break;
                        case "IsLangAttack":
                            actionInfo.IsLangAttack = (value == "1" ? true : false);
                            break;
                        case "PlaySpeed":
                            float.TryParse(value, out actionInfo.PlaySpeed);
                            break;
                        case "IsLoop":
                            actionInfo.IsLoop = (value == "1" ? true : false);
                            break;
                        case "SoundName":
                            actionInfo.SoundName = value;
                            break;
                        case "SoundPlayDelayTime":
                            float.TryParse(value, out actionInfo.SoundPlayDelayTime);
                            break;
                        case "AttRange":
                            float.TryParse(value, out actionInfo.AttackRadius);
                            break;
                        case "AttAngle":
                            float.TryParse(value, out actionInfo.AttackAngle);
                            break;
                        case "SelfMoveDelayTime":
                            float.TryParse(value, out actionInfo.SelfMoveDelayTime);
                            break;
                        case "SelfMoveDistance":
                            float.TryParse(value, out actionInfo.SelfMoveDistance);
                            break;
                        case "SelfMoveTime":
                            float.TryParse(value, out actionInfo.SelfMoveTime);
                            break;
                        case "IsHitMove":
                            actionInfo.IsHitMove = value == "1" ? true : false;
                            break;
                        case "HitMoveDistance":
                            float.TryParse(value, out actionInfo.HitMoveDistance);
                            break;
                        case "HitMoveTime":
                            float.TryParse(value, out actionInfo.HitMoveTime);
                            break;
                        case "IsHitFly":
                            actionInfo.IsHitFly = value == "1" ? true : false;
                            break;
                        case "HitFlyDistance":
                            float.TryParse(value, out actionInfo.HitFlyDistance);
                            break;
                        case "HitFlyTime":
                            float.TryParse(value, out actionInfo.HitFlyTime);
                            break;
                        case "ShakeScreen":
                            actionInfo.ShakeScreen = value;
                            break;
                        case "DefaultShakeScene":
                            actionInfo.DefaultShakeScene = (value == "1" ? true : false);
                            break;
                    }
                }
                ParseEffectInfo(child, "acs", "ac", actionInfo.ActionEffectInfos);
                ParseEffectInfo(child, "his", "hi", actionInfo.HitEffectInfos);
                characterConfigInfo.AddActionInfo(actionInfo);
            }
            characterConfigInfos.Add(characterConfigInfo.ModelName, characterConfigInfo);
        }
    }


    private static void ParseEffectInfo(XmlElement child,string pTitle, string cTitle, List<EffectInfo> list)
    {
        XmlNodeList effectInfos = child.GetElementsByTagName(pTitle);
        for (int y = 0; y < effectInfos.Count; ++y)
        {
            XmlElement xmlElement = effectInfos[y] as XmlElement;
            EffectInfo effectinfo = new EffectInfo();
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName(cTitle);
            for (int h = 0; h < xmlNodeList.Count; ++h)
            {
                XmlElement aei = xmlNodeList[h] as XmlElement;
                string key = aei.GetAttribute("n");
                string value = aei.InnerXml;
                switch (key)
                {
                    case "EffectName":
                        effectinfo.EffectName = value;
                        break;
                    case "EffectType":
                        int.TryParse(value, out effectinfo.EffectType);
                        break;
                    case "FlySpeed":
                        float.TryParse(value, out effectinfo.FlySpeed);
                        break;
                    case "SoundName":
                        effectinfo.SoundName = value;
                        break;
                    case "SoundPlayDelayTime":
                        float.TryParse(value, out effectinfo.SoundPlayDelayTime);
                        break;
                    case "BindType":
                        int.TryParse(value, out effectinfo.BindType);
                        break;
                    case "BindName":
                        effectinfo.BindName = value;
                        break;
                    case "IsLoop":
                        effectinfo.IsLoop = value == "1" ? true:false;
                        break;
                }
            }
            list.Add(effectinfo);
        }
    }


    //获取模型配置信息
    public static CharacterConfigInfo GetCharacterConfigInfo(string modelName, bool isEditor = false)
    {
        CharacterConfigInfo characterConfigInfo;
        characterConfigInfos.TryGetValue(modelName, out characterConfigInfo);
        if (characterConfigInfo == null)
        {
            string name = modelName.ToLower();
            XmlNodeList xmlnode = GetXML(name, isEditor);
            if (xmlnode != null)
            {
                ParseSingleCharacter(xmlnode);
                characterConfigInfos.TryGetValue(modelName, out characterConfigInfo);
            }
        }
        return characterConfigInfo;
    }



























    public static ShakeCameraConfigInfo GetShakeCameraConfigInfo(string id)
    {
        GetShakeCameraList();
        ShakeCameraConfigInfo shakeCameraConfigInfo;
        shakeDir.TryGetValue(id, out shakeCameraConfigInfo);
        return shakeCameraConfigInfo;
    }

    public static Dictionary<string, ShakeCameraConfigInfo> GetShakeCameraList()
    {
        if (shakeDir.Count == 0)
            ParseShake(GetXML("shakecamera"));
        return shakeDir;
    }



    private static Dictionary<string, ShakeCameraConfigInfo> ParseShake(XmlNodeList nodeList)
    {
        if (shakeDir.Count > 0)
            return shakeDir;
        for (int i = 0; i < nodeList.Count; ++i)
        {
            XmlElement node = nodeList[i] as XmlElement;
            ShakeCameraConfigInfo scCInfo = new ShakeCameraConfigInfo();
            XmlNodeList childList = node.GetElementsByTagName("c");
            for (int j = 0; j < childList.Count; j++)
            {
                XmlElement child = childList[j] as XmlElement;
                string key = child.GetAttribute("n");
                string value = child.InnerXml;
                switch (key)
                {
                    case "id":
                        scCInfo.id = value;
                        break;
                    case "type":
                        int.TryParse(value, out scCInfo.type);
                        break;
                    case "isdiminishing":
                        scCInfo.isdiminishing = value == "1" ? true : false;
                        break;
                    case "range":
                        float.TryParse(value, out scCInfo.range);
                        break;
                    case "firsttime":
                        float.TryParse(value, out scCInfo.firsttime);
                        break;
                    case "secondtime":
                        float.TryParse(value, out scCInfo.secondtime);
                        break;
                    case "number_of_shakes":
                        int number_of_shakes;
                        int.TryParse(value, out number_of_shakes);
                        scCInfo.number_of_shakes = number_of_shakes;
                        break;
                    case "shake_amount_x":
                        float shake_amount_x;
                        float.TryParse(value, out shake_amount_x);
                        scCInfo.shake_amount_x = shake_amount_x;
                        break;
                    case "shake_amount_y":
                        float shake_amount_y;
                        float.TryParse(value, out shake_amount_y);
                        scCInfo.shake_amount_y = shake_amount_y;
                        break;
                    case "shake_amount_z":
                        float shake_amount_z;
                        float.TryParse(value, out shake_amount_z);
                        scCInfo.shake_amount_z = shake_amount_z;
                        break;
                    case "rotation_amount_x":
                        float rotation_amount_x;
                        float.TryParse(value, out rotation_amount_x);
                        scCInfo.rotation_amount_x = rotation_amount_x;
                        break;
                    case "rotation_amount_y":
                        float rotation_amount_y;
                        float.TryParse(value, out rotation_amount_y);
                        scCInfo.rotation_amount_y = rotation_amount_y;
                        break;
                    case "rotation_amount_z":
                        float rotation_amount_z;
                        float.TryParse(value, out rotation_amount_z);
                        scCInfo.rotation_amount_z = rotation_amount_z;
                        break;
                    case "distance":
                        float distance;
                        float.TryParse(value, out distance);
                        scCInfo.distance = distance;
                        break;
                    case "speed":
                        float speed;
                        float.TryParse(value, out speed);
                        scCInfo.speed = speed;
                        break;
                    case "decay":
                        float decay;
                        float.TryParse(value, out decay);
                        scCInfo.decay = decay;
                        break;

                }
            }
            shakeDir.Add(scCInfo.id, scCInfo);
        }
        return shakeDir;
    }



    

    public static XmlNodeList GetXML(string abName,bool getByFile = false)
    {
        XmlNodeList nodeList = null;
        XmlDocument xmlDoc = null;
        if (!getByFile)
        {
            string content = ResourceManager.GetText("configs/configs", abName);
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
        }
        else
        {
            
            string path = Application.dataPath + "/ArtAssets/prefabs/configs/" + abName + ".xml";
            if (File.Exists(path))
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
            }
            
        }
        if (xmlDoc != null)
        {
            nodeList = xmlDoc.GetElementsByTagName("table");
            xmlDoc.RemoveAll();
            xmlDoc = null;
        }
        return nodeList;
    }


}
