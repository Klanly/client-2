using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConst
{
    public const string GameName = "mygame";
    public const string XMLExtensionName = ".xml";
    public const string ABExtensionName = ".assetbundle";
    public const string MetaExtensionn = ".meta";
    public const string StreamingAssets = "StreamingAssets";
    public const string NONE = "none";
    private static string assetBundlerPath = string.Empty;

    //配置表 ab path
    public const string ConfigABPath = "configs/configs.assetbundle";
    //字体 ab path
    public const string FontABDirectory = "fonts";
    //声音 ab path
    public const string SoundABDirectory = "sounds/";
    //角色模型ab path
    public const string CharacterModelABDirectory = "models/characters/";
    //场景模型ab path
    public const string SceneModelABDirectory = "models/scene/";

    //装备模型ab path
    public const string EquipmentModelABDirectory = "models/equipments/";
    //战斗特效ab path
    public const string FightEffectABDirectory = "effects/fight/";
    //ui特效ab path
    public const string UIEffectABDirectory = "effects/ui/";
    //场景特效ab path
    public const string SceneEffectABDirectory = "effects/scene/";



    /// <summary>
    /// 获取ab存放路径 在移动平台ab会存放在可读写目录便于热更新
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = GameConst.GameName.ToLower();
            if (Application.isMobilePlatform)
            {
                if (assetBundlerPath == string.Empty)
                {
                    assetBundlerPath = Application.persistentDataPath + "/" + game + "/";
                }
            }
            else
            {
                if (assetBundlerPath == string.Empty)
                {
                    assetBundlerPath = Application.streamingAssetsPath+"/";
                }
            }
            return assetBundlerPath;
        }
    }
}
