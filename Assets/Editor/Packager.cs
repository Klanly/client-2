using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Linq;
using System;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;

public class Packager 
{
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    private static string version = "1.0";
    
    private static string Android = "android";
    private static string IOS = "ios";

    public static string CurrentPlatform = Android;

    public static string AndroidWebUrl = "";
    public static string IosWebUrl = "";

    

    [MenuItem("GameTools/Build iPhone Resource", false, 1)]
    public static void BuildiPhoneResource()
    {
        CurrentPlatform = IOS;
        BuildAssetResource(BuildTarget.iOS, false);
    }

    [MenuItem("GameTools/Build Android Resource", false, 2)]
    public static void BuildAndroidResource()
    {
        CurrentPlatform = Android;
        BuildAssetResource(BuildTarget.Android, true);
    }

    [MenuItem("GameTools/Build Windows Resource", false, 3)]
    public static void BuildWindowsResource()
    {
        CurrentPlatform = Android;
        BuildAssetResource(BuildTarget.StandaloneWindows, true);
    }

    //重命名文件
    //[MenuItem("GameTools/Rename", false, 4)]
    //public static void ReName()
    //{
    //    string path = Application.dataPath + "/ArtAssets/prefabs/sounds/ui";
    //    DirectoryInfo fileDirInfo = new DirectoryInfo(path);
    //    FileInfo[] fileInfos = fileDirInfo.GetFiles();
    //    foreach (FileInfo fileInfo in fileInfos)
    //    {
    //        if (fileInfo.FullName.EndsWith(".mp3") || fileInfo.FullName.EndsWith(".ogg"))
    //        {
    //            string lowName = fileInfo.Name.ToLower();
    //            string fullPath = fileInfo.FullName.Replace(fileInfo.Name, lowName);
    //            UnityEngine.Debug.Log("fileInfo.FullName:" + fileInfo.Name);
    //            fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, fullPath));
    //        }
    //    }
    //}


    /// <summary>
    /// 生成绑定素材
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isWin"></param>
    /// <param name="isDeveloperMode">是否为开发者模式</param>
    public static void BuildAssetResource(BuildTarget target, bool isWin, bool isDeveloperMode = false)
    {
        AutoSetABName();
        string p = GameConst.DataPath;
        if (!Directory.Exists(p))
        {
            Directory.CreateDirectory(p);
        }
        
        BuildPipeline.BuildAssetBundles(GameConst.DataPath, BuildAssetBundleOptions.ChunkBasedCompression, target);
    }

    
    private static void AutoSetABName()
    {
        SetConfigABNameWithMeta();
        SetEffectABNameWithMeta();
        SetModelABNameWithMeta();
        SetFontABNameWithMeta();
        SetSoundsABNameWithMeta();
        SetUITexturesABName();
    }

    private static List<string> importSetting = new List<string>();


    
    private static void SetUITexturesABName()
    {
        if (importSetting.Count == 0)
        {
            importSetting.Add("TextureImporter:");
            importSetting.Add("  fileIDToRecycleName: {}");
            importSetting.Add("  serializedVersion: 4");
            importSetting.Add("  mipmaps:");
            importSetting.Add("    mipMapMode: 0");
            importSetting.Add("    enableMipMap: 0");
            importSetting.Add("    sRGBTexture: 1");
            importSetting.Add("    linearTexture: 0");
            importSetting.Add("    fadeOut: 0");
            importSetting.Add("    borderMipMap: 0");
            importSetting.Add("    mipMapFadeDistanceStart: 1");
            importSetting.Add("    mipMapFadeDistanceEnd: 3");
            importSetting.Add("  bumpmap:");
            importSetting.Add("    convertToNormalMap: 0");
            importSetting.Add("    externalNormalMap: 0");
            importSetting.Add("    heightScale: 0.25");
            importSetting.Add("    normalMapFilter: 0");
            importSetting.Add("  isReadable: 0");
            importSetting.Add("  grayScaleToAlpha: 0");
            importSetting.Add("  generateCubemap: 6");
            importSetting.Add("  cubemapConvolution: 0");
            importSetting.Add("  seamlessCubemap: 0");
            importSetting.Add("  textureFormat: 1");
            importSetting.Add("  maxTextureSize: 2048");
            importSetting.Add("  textureSettings:");
            importSetting.Add("    filterMode: 1");
            importSetting.Add("    aniso: -1");
            importSetting.Add("    mipBias: -1");
            importSetting.Add("    wrapMode: -1");
            importSetting.Add("  nPOTScale: 0");
            importSetting.Add("  lightmap: 0");
            importSetting.Add("  compressionQuality: 50");
            importSetting.Add("  spriteMode: 1");
            importSetting.Add("  spriteExtrude: 1");
            importSetting.Add("  spriteMeshType: 1");
            importSetting.Add("  alignment: 0");
            importSetting.Add("  spritePivot: {x: 0.5, y: 0.5}");
            importSetting.Add("  spriteBorder: {x: 0, y: 0, z: 0, w: 0}");
            importSetting.Add("  spritePixelsToUnits: 100");
            importSetting.Add("  alphaUsage: 1");
            importSetting.Add("  alphaIsTransparency: 0");
            importSetting.Add("  spriteTessellationDetail: -1");
            importSetting.Add("  textureType: 8");
            importSetting.Add("  textureShape: 1");
            importSetting.Add("  maxTextureSizeSet: 0");
            importSetting.Add("  compressionQualitySet: 0");
            importSetting.Add("  textureFormatSet: 0");
            importSetting.Add("  platformSettings:");
            importSetting.Add("  - buildTarget: DefaultTexturePlatform");
            importSetting.Add("    maxTextureSize: 2048");
            importSetting.Add("    textureFormat: -1");
            importSetting.Add("    textureCompression: 1");
            importSetting.Add("    compressionQuality: 50");
            importSetting.Add("    crunchedCompression: 0");
            importSetting.Add("    allowsAlphaSplitting: 0");
            importSetting.Add("    overridden: 0");
            importSetting.Add("  - buildTarget: Standalone");
            importSetting.Add("    maxTextureSize: 2048");
            importSetting.Add("    textureFormat: -1");
            importSetting.Add("    textureCompression: 1");
            importSetting.Add("    compressionQuality: 50");
            importSetting.Add("    crunchedCompression: 0");
            importSetting.Add("    allowsAlphaSplitting: 0");
            importSetting.Add("    overridden: 0");
            importSetting.Add("  - buildTarget: Android");
            importSetting.Add("    maxTextureSize: 128");
            importSetting.Add("    textureFormat: 34");
            importSetting.Add("    textureCompression: 1");
            importSetting.Add("    compressionQuality: 50");
            importSetting.Add("    crunchedCompression: 0");
            importSetting.Add("    allowsAlphaSplitting: 0");
            importSetting.Add("    overridden: 1");
            importSetting.Add("  spriteSheet:");
            importSetting.Add("    serializedVersion: 2");
            importSetting.Add("    sprites: []");
            importSetting.Add("    outline: []");
            importSetting.Add("  spritePackingTag: ");
            importSetting.Add("  userData: ");
            importSetting.Add("  assetBundleName: ");
            importSetting.Add("  assetBundleVariant: ");
        }
        string dic = Application.dataPath + "/ArtAssets/original/UI/";
        string[] dirs = Directory.GetDirectories(dic);
        for (int i = 0; i < dirs.Length; ++i)
        {
            string dirPath = dirs[i];
            string[] files = Directory.GetFiles(dirPath);
            string dirName = Path.GetFileName(dirPath);
            for (int j = 0; j < files.Length; ++j)
            {
                string file = files[j];
                if (file.EndsWith(".meta"))
                {
                    string[] hLines = File.ReadAllLines(file);
                    List<string> allLine = new List<string>();
                    allLine.Add(hLines[0]);
                    allLine.Add(hLines[1]);
                    allLine.Add(hLines[2]);
                    allLine.Add(hLines[3]);
                    allLine.AddRange(importSetting);
                    allLine[80] = "  spritePackingTag: " + dirName.ToLower();
                    allLine[82] = "  assetBundleName: ui/textures/" + dirName.ToLower() + GameConst.ABExtensionName;
                    SaveMeta(file, allLine);
                }
            }
        }
        dic = Application.dataPath + "/ArtAssets/prefabs/ui/prefabs";
        dirs = Directory.GetDirectories(dic);
        for (int i = 0; i < dirs.Length; ++i)
        {
            string dirPath = dirs[i];
            string[] files = Directory.GetFiles(dirPath);
            for (int j = 0; j < files.Length; ++j)
            {
                string file = files[j];
                if (file.EndsWith(".meta"))
                {
                    string[] allLines = File.ReadAllLines(file); 
                    string fileN = file.Replace(".meta", "");
                    fileN = Path.GetFileNameWithoutExtension(fileN).ToLower();
                    allLines[7] = "  assetBundleName: ui/prefabs/" + fileN + GameConst.ABExtensionName;
                    SaveMeta(file, allLines);
                }
            }
        }

        
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 设置配置文件的ab name
    /// </summary>
    private static void SetConfigABNameWithMeta()
    {
        string path = Application.dataPath + "/ArtAssets/prefabs/configs/";
        
        string[] dirs = Directory.GetDirectories(path);
        foreach (string dir in dirs)
        {
            string[] fs = Directory.GetFiles(dir);
            foreach (string f in fs)
            {
                if (!f.EndsWith(".meta")) continue;
                string[] allLines = File.ReadAllLines(f);
                allLines[6] = "  assetBundleName: configs/configs" + GameConst.ABExtensionName;
                SaveMeta(f, allLines);
            }
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".meta"))
            {
                string[] allLines = File.ReadAllLines(file);
                allLines[6] = "  assetBundleName: configs/configs" + GameConst.ABExtensionName;
                SaveMeta(file, allLines);
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 设置特效的ab name
    /// </summary>
    private static void SetEffectABNameWithMeta()
    {
        string path = Application.dataPath + "/ArtAssets/prefabs/effects/fight/";
        SetEffectABName(path,"fight");
        path = Application.dataPath + "/ArtAssets/prefabs/effects/ui/";
        SetEffectABName(path,"ui");
        AssetDatabase.Refresh();
    }
    private static void SetEffectABName(string path,string d)
    {
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".meta"))
            {
                string[] allLines = File.ReadAllLines(file);
                string name = Path.GetFileNameWithoutExtension(file.Replace(".meta", ""));
                allLines[6] = "  assetBundleName: effects/" + d + "/" + name.ToLower() + GameConst.ABExtensionName;
                SaveMeta(file, allLines);
            }
        }
    }

    
    /// <summary>
    /// 设置模型的ab name
    /// </summary>
    private static void SetModelABNameWithMeta()
    {
        string path = Application.dataPath + "/ArtAssets/prefabs/models/characters/";
        SetModelABName(path, "characters");
        path = Application.dataPath + "/ArtAssets/prefabs/models/equipments/";
        SetModelABName(path, "equipments");
        path = Application.dataPath + "/ArtAssets/prefabs/models/scene/";
        SetModelABName(path, "scene");
        AssetDatabase.Refresh();
    }

    private static void SetModelABName(string path, string d)
    {
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".meta"))
            {
                string[] allLines = File.ReadAllLines(file);
                string name = Path.GetFileNameWithoutExtension(file.Replace(".meta", ""));
                allLines[6] = "  assetBundleName: models/" + d + "/" + name.ToLower() + GameConst.ABExtensionName;
                SaveMeta(file, allLines);
            }
        }
    }


    /// <summary>
    /// 设置font ab name
    /// </summary>
    private static void SetFontABNameWithMeta()
    {
        string path = Application.dataPath + "/ArtAssets/prefabs/fonts/";
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".fontsettings.meta"))
            {
                string[] allLines = File.ReadAllLines(file);
                allLines[7] = "  assetBundleName: fonts/fonts" + GameConst.ABExtensionName;
                SaveMeta(file, allLines);
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 设置声音文件的ab name
    /// </summary>
    private static void SetSoundsABNameWithMeta()
    {
        string pPath = Application.dataPath + "/ArtAssets/prefabs/sounds/";
        string bgPath = pPath + "bg";
        SetSoundABName(bgPath);

        string fightPath = pPath + "fight";
        SetSoundABName(fightPath);

        string uiPath = pPath + "ui";
        SetSoundABName(uiPath);
    }

    private static void SetSoundABName(string path)
    {
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".meta"))
            {
                string[] allLines = File.ReadAllLines(file);
                string name = Path.GetFileNameWithoutExtension(file.Replace(".meta", ""));
                allLines[20] = "  assetBundleName: sounds/" + name + GameConst.ABExtensionName;
                SaveMeta(file, allLines);
            }
        }
    }


    private static void SaveMeta(string file, List<string> allLines)
    {
        if (File.Exists(file))
            File.Delete(file);
        FileStream fs = new FileStream(file, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        foreach (string line in allLines)
        {
            sw.WriteLine(line);
        }
        sw.Close(); fs.Close();
    }

    private static void SaveMeta(string file, string[] allLines)
    {
        if(File.Exists(file))
            File.Delete(file);
        FileStream fs = new FileStream(file, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        foreach (string line in allLines)
        {
            sw.WriteLine(line);
        }
        sw.Close(); fs.Close();
    }

    //private static void SetCTextures(string path)
    //{
    //    string[] textures = Directory.GetFiles(path);
    //    foreach (string file in textures)
    //    {
    //        if (file.EndsWith(".png") || file.EndsWith(".jpg"))
    //        {
    //            string abName = Path.GetFileName(file).Split(".".ToCharArray())[0] + "_texture";
    //            AssetImporter asset = AssetImporter.GetAtPath(file);
    //            string fName;
    //            fName = "scene_textures/" + abName + AppConst.ExtName;
    //            asset.assetBundleName = "";
    //            asset.SaveAndReimport();
    //            //if (asset.assetBundleName != fName)
    //            //{
    //            //    asset.assetBundleName = fName;
    //            //    asset.SaveAndReimport();
    //            //}
    //        }
    //    }
    //}




    //设置单个UI图片的assetbundle名称
    //private static void SetSingleTexture(string head, string file, string tagName, TextureImporterType type = TextureImporterType.Sprite)
    //{
    //    TextureImporter asset = (TextureImporter)AssetImporter.GetAtPath(file);

    //    string fName = head + "/" + tagName + AppConst.ExtName;
    //    asset.textureType = type;
    //    asset.mipmapEnabled = false;
    //    asset.spritePackingTag = tagName;
    //    asset.assetBundleName = fName;
    //    asset.filterMode = FilterMode.Bilinear;
    //    if (CurrentPlat == IOS)
    //    {
    //       // asset.SetPlatformTextureSettings("iPhone", 1024, TextureImporterFormat.AutomaticTruecolor);
    //    }
    //    asset.SaveAndReimport();
    //}

    //private static void SetSingleSoundABName(string file,string head = null)
    //{
    //    if (Path.GetExtension(file) == AppConst.WAV || Path.GetExtension(file) == AppConst.MP3 || Path.GetExtension(file) == AppConst.OGG)
    //    {
    //        string abName = Path.GetFileName(file).Split(".".ToCharArray())[0];
    //        AssetImporter asset = AssetImporter.GetAtPath(file);
    //        string fName;
    //        if (head != null && head != string.Empty)
    //        {
    //            fName = head + "/"+ abName + AppConst.ExtName;
    //        } else
    //        {
    //            fName = abName + AppConst.ExtName;
    //        }
    //        if (asset.assetBundleName != fName)
    //        {
    //            asset.assetBundleName = fName;
    //            asset.SaveAndReimport();
    //        }
    //    }
    //}

    //private static void SetSinglePrefabABName(string file,string head = null)
    //{
    //    string abName = Path.GetFileName(file).Split(".".ToCharArray())[0];
    //    AssetImporter asset = AssetImporter.GetAtPath(file);
    //    string fName;
    //    if (head != null && head != string.Empty)
    //    {
    //        fName = head + "/" + abName + AppConst.ExtName;
    //    }
    //    else
    //    {
    //        fName = abName + AppConst.ExtName;
    //    }

    //    if (file.Contains("Builds") && (fName.Contains("chengqiang_") || fName.Contains("kebaota_")))
    //    {
    //        asset.assetBundleName = "";
    //        asset.SaveAndReimport();
    //    }
    //    else
    //    {
    //        if (asset.assetBundleName != fName)
    //        {
    //            asset.assetBundleName = fName;
    //            asset.SaveAndReimport();
    //        }
    //    }

    //}

    //自动设置需要打包成aasetbundle文件的assetbundle 名称
    //[MenuItem("Game/Auto set assetbundle name", false, 115)]
    //public static void AutoSetAssetBundleName()
    //{
    //    string fName;


    //    //设置所有模型的assetbundle 名称
    //    string modelsPath = "Assets/ArtResources/Prefabs/Model";
    //    string[] models = Directory.GetDirectories(modelsPath);
    //    foreach (string model in models)
    //    {
    //        string[] modelfiles = Directory.GetFiles(model);
    //        foreach (string e in modelfiles)
    //        {
    //            if (e.EndsWith(".prefab"))
    //            {
    //                SetSinglePrefabABName(e, "models");
    //            }
    //        }
    //    }



    //    //设置UI prefab assetbundle name
    //    string uiprefabsPath = "Assets/ArtResources/Prefabs/UI";
    //    string[] uis = Directory.GetDirectories(uiprefabsPath);
    //    foreach (string ui in uis)
    //    {
    //        string[] uifiles = Directory.GetFiles(ui);
    //        foreach (string e in uifiles)
    //        {
    //            if (e.EndsWith(".prefab"))
    //            {
    //                SetSinglePrefabABName(e, "ui_prefab");
    //            }
    //        }
    //    }



    //    //string modelsPath1 = "Assets/ArtResources/Prefabs/Model/Builds/";
    //    //string[] builds = Directory.GetFiles(modelsPath1);
    //    //foreach (string e in builds)
    //    //{
    //    //    if (e.EndsWith(".prefab"))
    //    //    {

    //    //        SetSinglePrefabABName(e, "models");
    //    //    }
    //    //}





    //    //设置游戏音效assetbundle name
    //    string soundPath = "Assets/ArtResources/Prefabs/Sound/";
    //    string[] sounds = Directory.GetDirectories(soundPath);
    //    foreach (string sound in sounds)
    //    {
    //        string[] soundfiles = Directory.GetFiles(sound);
    //        foreach (string s in soundfiles)
    //        {
    //            SetSingleSoundABName(s,"sounds");
    //        }
    //    }

    //    //设置地表属性文件ab name
    //    string mdPath = "Assets/MapData/";
    //    string[] mds = Directory.GetFiles(mdPath);
    //    foreach (string md in mds)
    //    {
    //        if (md.EndsWith(".txt"))
    //        {
    //            string abName = Path.GetFileName(md).Split(".".ToCharArray())[0];
    //            AssetImporter asset = AssetImporter.GetAtPath(md);
    //            string abn = "mapdatas/"+abName + AppConst.ExtName;
    //            if (asset.assetBundleName != abn)
    //            {
    //                asset.assetBundleName = abn;
    //                asset.SaveAndReimport();
    //            }
    //        }
    //    }


    //    //设置游戏特效assetbundle name
    //    string effectPath = "Assets/ArtResources/Prefabs/Effects/";
    //    string[] effects = Directory.GetDirectories(effectPath);
    //    foreach (string effect in effects)
    //    {
    //        string[] effectfiles = Directory.GetFiles(effect);
    //        foreach (string e in effectfiles)
    //        {
    //            if (e.EndsWith(".prefab"))
    //            {
    //                SetSinglePrefabABName(e,"effects");
    //            }
    //        }
    //    }

    //    //设置场景文件的assetbundle名称
    //    string scenesPath = "Assets/ArtResources/Original/Scenes";
    //    string[] directorys = Directory.GetDirectories(scenesPath);
    //    foreach (string directory in directorys)
    //    {
    //        string[] files = Directory.GetFiles(directory);
    //        foreach (string file in files)
    //        {
    //            if (Path.GetExtension(file) == AppConst.LevelExtensionName)
    //            {
    //                string abName = Path.GetFileName(file).Split(".".ToCharArray())[0];

    //                AssetImporter asset = AssetImporter.GetAtPath(file);
    //                fName = "scenes/"+abName + AppConst.ExtName;
    //                asset.assetBundleName = "";
    //                asset.SaveAndReimport();
    //                //if (asset.assetBundleName != fName)
    //                //{
    //                //    asset.assetBundleName = fName;
    //                //    asset.SaveAndReimport();
    //                //}

    //                //string lNames = file.Replace(".unity", "");
    //                //string[] fiels = Directory.GetFiles(lNames);
    //                //fName = lNames + "/";
    //                //if (Directory.Exists(fName))
    //                //{
    //                //    foreach (string light in fiels)
    //                //    {
    //                //        if (!light.EndsWith(".meta"))
    //                //        {
    //                //            string ffName = Path.GetFileName(light).Split(".".ToCharArray())[0];
    //                //            asset = AssetImporter.GetAtPath(light);
    //                //            if (asset.assetBundleName != "scenes/"+ abName + "/" + ffName+ AppConst.ExtName)
    //                //            {
    //                //                asset.assetBundleName = "scenes/" + abName + "/" + ffName + AppConst.ExtName;
    //                //                asset.SaveAndReimport();
    //                //            }
    //                //        }
    //                //    }
    //                //}
    //            }
    //        }
    //    }
    //    //设置场景建筑地形共用贴图
    //    string sceneTextures = "Assets/ArtResources/Original/Scenes/common/textures";
    //    SetCTextures(sceneTextures + "/c");
    //    SetCTextures(sceneTextures + "/d");
    //    SetCTextures(sceneTextures + "/m");
    //    //SetCTextures(sceneTextures + "/n");
    //    SetCTextures(sceneTextures + "/t");

    //    //设置xml文件的prefab名称
    //    string configPath = "Assets/ConfigFiles/xml";
    //    string[] xmlfiles = Directory.GetFiles(configPath);
    //    foreach (string file in xmlfiles)
    //    {
    //        if ((Path.GetExtension(file) == ".xml" || Path.GetExtension(file) == ".txt") && !file.EndsWith("hell_angels.xml") && !file.EndsWith("goods.xml") && !file.EndsWith("army.xml") && !file.EndsWith("skill_ext.xml") && !file.EndsWith("monster_list.xml") && !file.EndsWith("sensitive_words.xml") && !file.EndsWith("characterconfig.xml"))
    //        {
    //            string abName = Path.GetFileName(file).Split(".".ToCharArray())[0];
    //            AssetImporter asset = AssetImporter.GetAtPath(file);
    //            fName = "configs/allconfigs" + AppConst.ExtName;
    //            if (asset.assetBundleName != fName)
    //            {
    //                asset.assetBundleName = fName;
    //                asset.SaveAndReimport();
    //            }
    //        }
    //        else if (file.EndsWith("hell_boss_random.xml") || file.EndsWith("floating_words.xml") || file.EndsWith("robot.xml") || file.EndsWith("hero_ext.xml") || file.EndsWith("hell_angels.xml") || file.EndsWith("goods.xml") || file.EndsWith("army.xml") || file.EndsWith("skill_ext.xml") || file.EndsWith("monster_list.xml") || file.EndsWith("sensitive_words.xml") || file.EndsWith("characterconfig.xml"))
    //        {
    //            AssetImporter asset = AssetImporter.GetAtPath(file);
    //            asset.assetBundleName = "";
    //            asset.SaveAndReimport();
    //        }
    //    }


    //    //设置UI图片的assetbundle name 以及texture type
    //    string uiTexturePath = "Assets/ArtResources/Original/UI";
    //    directorys = Directory.GetDirectories(uiTexturePath);
    //    foreach(string directory in directorys)
    //    {
    //        string[] files = Directory.GetFiles(directory);
    //        string disName = Path.GetFileName(directory);
    //        if (disName.ToLower() == "yg")
    //        {
    //            foreach (string file in files)
    //            {
    //                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
    //                {
    //                    SetSingleTexture("ui_textures",file, disName+"_ui", TextureImporterType.Default);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            foreach (string file in files)
    //            {
    //                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
    //                {
    //                    SetSingleTexture("ui_textures",file, disName + "_ui");
    //                }
    //            }
    //        }
    //    }

    //    //设置字体的assetbundle name
    //    string fontPath = "Assets/ArtResources/Fonts";
    //    string[] fonts = Directory.GetFiles(fontPath);
    //    foreach (string font in fonts)
    //    {
    //        string abName;
    //        if (font.EndsWith(".fontsettings"))
    //        {
    //            abName = Path.GetFileName(font).Split(".".ToCharArray())[0];
    //            AssetImporter asset = AssetImporter.GetAtPath(font);
    //            fName = "fonts/"+"bitmapfonts" + AppConst.ExtName;
    //            if (asset.assetBundleName != fName)
    //            {
    //                asset.assetBundleName = fName;
    //                asset.SaveAndReimport();
    //            }
    //        }
    //        else if (font.EndsWith(".TTF"))
    //        {
    //            abName = Path.GetFileName(font).Split(".".ToCharArray())[0];
    //            AssetImporter asset = AssetImporter.GetAtPath(font);
    //            fName = "fonts/"+abName.ToLower() + AppConst.ExtName;
    //            if (asset.assetBundleName != fName)
    //            {
    //                asset.assetBundleName = fName;
    //                asset.SaveAndReimport();
    //            }
    //        }
    //    }

    //    AssetDatabase.Refresh();
    //}





    //private static void SetAllPrefabAssetbundleName1(string pathname)
    //{
    //    Stack<string> skDir = new Stack<string>();
    //    skDir.Push(pathname);
    //    List<string> fileList = new List<string>();
    //    while (skDir.Count > 0)
    //    {
    //        pathname = skDir.Pop();
    //        string[] subDirs = Directory.GetDirectories(pathname);
    //        string[] subFiles = Directory.GetFiles(pathname);
    //        if (subDirs != null)
    //        {
    //            for (int i = 0; i < subDirs.Length; i++)
    //            {
    //                skDir.Push(subDirs[i]);
    //            }
    //        }
    //        if (subFiles != null)
    //        {
    //            for (int i = 0; i < subFiles.Length; i++)
    //            {
    //                string file = subFiles[i];
    //               // string fileName = Path.GetFileName(file);
    //                if (file.EndsWith(".prefab"))
    //                {
    //                    // 处理文件
    //                    fileList.Add(file);
    //                }
    //            }
    //        }
    //    }
    //    foreach (string file in fileList)
    //    {
    //        SetSingleFileAssetbundleName(file);
    //    }
    //}

    //private static void SetAllPrefabAssetbundleName(string directory)
    //{
    //    string[] files = Directory.GetFiles(directory);
    //    foreach (string file in files)
    //    {
    //        if(file.EndsWith(AppConst.PrefabExtensionName))
    //        {
    //            SetSingleFileAssetbundleName(file);
    //        }
    //    }
    //    string[] directorys = Directory.GetDirectories(directory);
    //    foreach (string dir in directorys)
    //    {
    //        SetAllPrefabAssetbundleName(dir);
    //    }
    //}

    //设置单个prefab的assetbundle名称
    //private static void SetSingleFileAssetbundleName(string file)
    //{
    //    string abName = Path.GetFileName(file).Split(".".ToCharArray())[0];

    //    AssetImporter asset = AssetImporter.GetAtPath(file);
    //    string fName;
    //    if (file.Contains(Path.DirectorySeparatorChar+"UI"+ Path.DirectorySeparatorChar))
    //    {

    //        fName = abName+"_ui_prefab" + AppConst.ExtName;
    //        UnityEngine.Debug.LogError("fName:"+ fName);
    //    }
    //    else
    //    {
    //        fName = abName + AppConst.ExtName;
    //    }
    //    if (asset.assetBundleName != fName)
    //    {
    //        asset.assetBundleName = fName;
    //        asset.SaveAndReimport();
    //    }
    //}




    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path) 
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names) 
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs) 
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    static void UpdateProgress(int progress, int progressMax, string desc) {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
}