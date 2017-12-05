using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIWidgets;
using System.IO;
using System.Text;
using System;
public class DataManager
{
    public static float WalkSpeedMax = 10f;
    public static float WalkSpeedMin = 0.1f;

    public static float RunSpeedMax = 10f;
    public static float RunSpeedMin = 0.1f;

    public static float ActionSpeedMax = 5f;
    public static float ActionSpeedMin = 0.1f;

    public static float SelfMoveDistanceMax = 10f;
    public static float SelfMoveDistanceMin = 0f;

    public static float SelfMoveTimeMax = 10f;
    public static float SelfMoveTimeMin = 0f;

    public static float AttackRadioMax = 50f;
    public static float AttackRadioMin = 1f;
    public static float AttackAngleMax = 360f;
    public static float AttackAngleMin = 60f;

    public static float HitMoveDistanceMax = 20f;
    public static float HitMoveDistanceMin = 0.1f;

    public static float HitMoveTimeMax = 20f;
    public static float HitMoveTimeMin = 0.1f;

    public static float HitFlyDistanceMin = 0.1f;
    public static float HitFlyDistanceMax = 20f;
    public static float HitFlyTimeMin = 0.1f;
    public static float HitFlyTimeMax = 20f;

    public static float Step = 0.01f;

    public static float ActionSoundDelayMin = 0f;
    public static float ActionSoundDelayMax = 5f;
    public static float ActionSoundStep = 0.01f;


    public static float FlySpeedMin = 0f;
    public static float FlySpeedMax = 50f;
    public static float FlySpeedStep = 0.01f;

    private static ObservableList<string> copyList = new ObservableList<string>();
    public static ObservableList<string> GetCopyList()
    {
        if (copyList.Count > 0)
        {
            return copyList;
        }
        string path = Application.dataPath + "/ArtAssets/prefabs/configs/copy/";
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(GameConst.XMLExtensionName))
            {
                string n = Path.GetFileNameWithoutExtension(file);
                copyList.Add(n);
            }
        }
        return copyList;
    }


    private static ObservableList<string> soundList = new ObservableList<string>();
    public static ObservableList<string> GetSoundList()
    {
        if (soundList.Count > 0) return soundList;

        soundList.Add(BindTypes.NONE);
        string path = GameConst.DataPath + "/sounds/";
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(GameConst.ABExtensionName))
            {
                string n = Path.GetFileNameWithoutExtension(file);
                soundList.Add(n);
            }
        }
        return soundList;
    }
    public static int GetSoundIndex(string soundn)
    {
        return soundList.IndexOf(soundn);
    }


    private static ObservableList<string> waeponList = new ObservableList<string>();
    public static ObservableList<string> GetWeaponList()
    {
        if (waeponList.Count > 0) return waeponList;

        waeponList.Add(BindTypes.NONE);
        string path = GameConst.DataPath + "/models/equipments/";
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(GameConst.ABExtensionName))
            {
                string n = Path.GetFileNameWithoutExtension(file);
                waeponList.Add(n);
            }
        }
        return waeponList;
    }
    public static int GetWeaponIndex(string weaponn)
    {
        return waeponList.IndexOf(weaponn);
    }


    private static ObservableList<string> characterList = new ObservableList<string>();
    public static ObservableList<string> GetCharacterList()
    {
        if (characterList.Count > 0) return characterList;
        string path = GameConst.DataPath + "/models/characters/";
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(GameConst.ABExtensionName))
            {
                string n = Path.GetFileNameWithoutExtension(file);
                characterList.Add(n);
            }
        }
        return characterList;
    }
    public static int GetCharacterIndex(string charactern)
    {
        return characterList.IndexOf(charactern);
    }



    private static ObservableList<string> effectList = new ObservableList<string>();
    public static ObservableList<string> GetEffectList()
    {
        if (effectList.Count > 0) return effectList;
        string path = GameConst.DataPath + "/effects/fight/";
        effectList.Add(BindTypes.NONE);
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(GameConst.ABExtensionName))
            {
                string n = Path.GetFileNameWithoutExtension(file);
                effectList.Add(n);
            }
        }
        return effectList;
    }
    public static int GetEffectIndex(string effectn)
    {
        return effectList.IndexOf(effectn);
    }


    private static ObservableList<string> actionList = new ObservableList<string>();
    public static ObservableList<string> GetActionList()
    {
        if (actionList.Count > 0) return actionList;
        List<string> aList = AnimationType.GetActionList();
        foreach (string action in aList)
        {
            actionList.Add(action);
        }
        return actionList;
    }
    public static int GetActionIndex(string actionn)
    {
        return actionList.IndexOf(actionn);
    }



    private static ObservableList<string> effecttypeList = new ObservableList<string>();
    public static ObservableList<string> GetEffectTypeList()
    {
        if (effecttypeList.Count == 0)
        {
            foreach (string key in EffectTypes.Types())
            {
                effecttypeList.Add(key);
            }
        }
        return effecttypeList;
    }
    public static int GetEffectTypeIndex(string effectType)
    {
        return effecttypeList.IndexOf(effectType);
    }




    private static ObservableList<string> effectBindList = new ObservableList<string>();
    public static ObservableList<string> GetEffectBindList()
    {
        if (effectBindList.Count == 0)
        {
            foreach (string key in BindTypes.Types())
            {
                effectBindList.Add(key);
            }
        }
        return effectBindList;
    }
    public static int GetEffectBindIndex(string bindType)
    {
        return effectBindList.IndexOf(bindType);
    }


    private static ObservableList<string> effectBindnList = new ObservableList<string>();
    public static ObservableList<string> GetEffectBindNameList()
    {
        if (effectBindnList.Count == 0)
        {

            foreach (string key in BoneTypes.Types())
            {
                effectBindnList.Add(key);
            }
        }
        return effectBindnList;
    }

    public static int GetEffectBindnIndex(string n)
    {
        return effectBindnList.IndexOf(n);
    }


    private static Dictionary<string, CharacterConfigInfo> characterInfos = new Dictionary<string, CharacterConfigInfo>();
    public static CharacterConfigInfo GetCharacterConfigInfo(string modelName)
    {
        CharacterConfigInfo characterConfigInfo;
        characterInfos.TryGetValue(modelName, out characterConfigInfo);
        if (characterConfigInfo == null)
        {
            characterConfigInfo = ConfigManager.GetCharacterConfigInfo(modelName,true);
            if (characterConfigInfo != null)
            {
                characterInfos.Add(modelName, characterConfigInfo);
            }
        }
        if (characterConfigInfo == null)
        {
            characterConfigInfo = new CharacterConfigInfo();
            characterConfigInfo.ModelName = modelName;
            characterInfos.Add(modelName, characterConfigInfo);
        }
        return characterConfigInfo;
    }

    private static List<CharacterConfigInfo> tempList = new List<CharacterConfigInfo>();

    private static TimerInfo timerInfo;
    private static Action OnCompleteHandler;
    public static void Save(Action OnCltHandler)
    {
        //to do 提示保存中
        OnCompleteHandler = OnCltHandler;
        if (characterInfos.Count == 0 && OnCompleteHandler != null)
        {
            OnCompleteHandler();
            return;
        }


        tempList.Clear();



        foreach (CharacterConfigInfo characterConfigInfo in characterInfos.Values)
        {
            tempList.Add(characterConfigInfo);
        }
        timerInfo = TimerManager.AddDelayHandler(OnDelHandler, 0.02f, (uint)tempList.Count + 1);
    }


    private static void OnDelHandler(float del)
    {
        if (tempList.Count > 0)
        {
            int index = tempList.Count - 1;
            CharacterConfigInfo characterConfigInfo = tempList[index];
            tempList.RemoveAt(index);
            SaveSingle(characterConfigInfo);
        }
        else
        {
            TimerManager.RemoveHandler(timerInfo);
            timerInfo = null;
            // to do 保存完成
            if (OnCompleteHandler != null)
            {
                OnCompleteHandler();
            }
        }

    }


    private static void SaveSingle(CharacterConfigInfo characterConfigInfo)
    {
        string content = characterConfigInfo.ToXMLString();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<?xml version='1.0' encoding='utf-8'?>\n");
        stringBuilder.Append(content);
        string path = Application.dataPath + "/ArtAssets/prefabs/configs/characters/" + characterConfigInfo.ModelName.ToLower() + ".xml";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        StreamWriter SW;
        SW = File.CreateText(path);
        SW.WriteLine(stringBuilder.ToString());
        SW.Close();
    }


    private static ObservableList<string> shakeCameraList = new ObservableList<string>();
    public static ObservableList<string> GetShakeCameraList()
    {
        if (shakeCameraList.Count > 0) return shakeCameraList;
        shakeCameraList.Add(BindTypes.NONE);
        Dictionary<string, ShakeCameraConfigInfo> tDic = ConfigManager.GetShakeCameraList();
        foreach (ShakeCameraConfigInfo shakeCameraConfigInfo in tDic.Values)
        {
            shakeCameraList.Add(shakeCameraConfigInfo.id);
        }
        return shakeCameraList;
    }


    public static int GetShakeCameraIndex(string id)
    {
        return shakeCameraList.IndexOf(id);
    }


}
