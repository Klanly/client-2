using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BoneTypes
{
    public static string LHandPoint = "LHandPoint";// 左手 LHandPoint(Z朝上，X超前)
    public static string RHandPoint = "RHandPoint";// 右手 RHandPoint(Z朝上，X超前)
    public static string LFootPoint = "LFootPoint";//左脚 LFootPoint(Y朝上，Z超前)
    public static string RFootPoint = "RFootPoint";//右脚 RFootPoint(Y朝上，Z超前)
    public static string FootsPoint = "FootsPoint";//零点 FootsPoint(Y朝上，Z超前)
    public static string CenterPoint = "CenterPoint";//受击点 CenterPoint(Y朝上，Z超前)
    public static string BackPoint = "BackPoint";//翅膀 BackPoint(Y朝上，Z超前)
    public static string HeadPoint = "HeadPoint";//头 HeadPoint(Y朝上，Z超前)
    public static string Headbar = "headbar";      //血条 headbar

    public static Dictionary<string, string> KeyToValueBoneList = new Dictionary<string, string>();
    public static Dictionary<string, string> ValueToKeyBoneList = new Dictionary<string, string>();


    public static string Bone_Rwq = "Bone_Rwq";    //右手 Bone_Rwq
    public static string Bone_Lwq = "Bone_Lwq";    //左手 Bone_Lwq
    //public static string Bone_y_Rwq = "Bone_y_Rwq";//右腰 Bone_y_Rwq
    //public static string Bone_y_Lwq = "Bone_y_Lwq";//左腰 Bone_y_Lwq
    //public static string Bone_b_Rwq = "Bone_b_Rwq";//背右 Bone_b_Rwq
    //public static string Bone_b_Lwq = "Bone_b_Lwq";//背左 Bone_b_Lwq


    //public static string Weaponpoint = "weaponpoint";
    //public static string Weaponpoint2 = "weaponpoint2";
    //public static string Weaponpoint3 = "weaponpoint3";

    //public static Dictionary<string, string> WeaponBoneList = new Dictionary<string, string>();



    //挂点列表
    private static void Init()
    {
        if (KeyToValueBoneList.Count == 0)
        {

            KeyToValueBoneList.Add("none", "none");
            ValueToKeyBoneList.Add("none", "none");


            string key = LHandPoint + "_左手";
            KeyToValueBoneList.Add(LHandPoint, key);
            ValueToKeyBoneList.Add(key, LHandPoint);

            key = RHandPoint + "_右手";
            KeyToValueBoneList.Add(RHandPoint, key);
            ValueToKeyBoneList.Add(key, RHandPoint);

            key = LFootPoint + "_左脚";
            KeyToValueBoneList.Add(LFootPoint, key);
            ValueToKeyBoneList.Add(key, LFootPoint);

            key = RFootPoint + "_右脚";
            KeyToValueBoneList.Add(RFootPoint, key);
            ValueToKeyBoneList.Add(key, RFootPoint);

            key = FootsPoint + "_零点";
            KeyToValueBoneList.Add(FootsPoint, key);
            ValueToKeyBoneList.Add(key, FootsPoint);

            key = CenterPoint + "_中心点";
            KeyToValueBoneList.Add(CenterPoint, key);
            ValueToKeyBoneList.Add(key, CenterPoint);

            key = BackPoint + "_翅膀";
            KeyToValueBoneList.Add(BackPoint, key);
            ValueToKeyBoneList.Add(key, BackPoint);

            key = HeadPoint + "_头";
            KeyToValueBoneList.Add(HeadPoint, key);
            ValueToKeyBoneList.Add(key, HeadPoint);

            key = Headbar + "_血条";
            KeyToValueBoneList.Add(Headbar, key);
            ValueToKeyBoneList.Add(key, Headbar);


            //BoneList.Add(Weaponpoint, Weaponpoint + "_武器挂点1");
            //BoneList.Add(Weaponpoint2, Weaponpoint2 + "_武器挂点2");
            //BoneList.Add(Weaponpoint3, Weaponpoint3 + "_武器挂点3");
        }
        
    }

    public static Dictionary<string, string>.KeyCollection Types()
    {
        Init();
        return ValueToKeyBoneList.Keys;
    }

    public static string KeyToValue(string key)
    {
        Init();
        string value;
        ValueToKeyBoneList.TryGetValue(key, out value);
        return value;
    }

    public static string ValueToKey(string value)
    {
        Init();
        string key;
        KeyToValueBoneList.TryGetValue(value, out key);
        return key;
    }


    //public static string GetBonen(string bn)
    //{
    //    string n;
    //    BoneList.TryGetValue(bn, out n);
    //    return n;
    //}

    //private static List<string> bons = new List<string>();
    //public static List<string> Bons()
    //{
    //    if (bons.Count == 0)
    //    {
    //        bons.Add(BindTypes.NONE);
    //        foreach (string v in GetBoneList().Values)
    //        {
    //            bons.Add(v);
    //        }
    //    }
    //    return bons;
    //}
    //public static string GetBonDes(string key)
    //{
    //    string n;
    //    GetBoneList().TryGetValue(key, out n);
    //    return n;
    //}

    //public static string GetBonB(string v)
    //{
    //    foreach (string key in GetBoneList().Keys)
    //    {
    //        string v1 = GetBonDes(key);
    //        if (v1 == v) return key;
    //    }
    //    return BindTypes.NONE;
    //}


    //武器挂点列表
    //private static Dictionary<string, string> GetWeaponBoneList()
    //{
    //    if (WeaponBoneList.Count == 0)
    //    {
    //        WeaponBoneList.Add(Bone_Rwq + "_右手",Bone_Rwq);
    //        WeaponBoneList.Add(Bone_Lwq + "_左手",Bone_Lwq);
    //        WeaponBoneList.Add(Bone_y_Rwq + "_右腰",Bone_y_Rwq);
    //        WeaponBoneList.Add(Bone_y_Lwq + "_左腰",Bone_y_Lwq);
    //        WeaponBoneList.Add(Bone_b_Rwq + "_右背",Bone_b_Rwq);
    //        WeaponBoneList.Add(Bone_b_Lwq + "_左背",Bone_b_Lwq);
    //    }
    //    return WeaponBoneList;
    //}


    //private static List<string> weaponBons = new List<string>();
    //public static List<string> WeaponBons()
    //{
    //    if (weaponBons.Count == 0)
    //    {
    //        foreach (string key in GetWeaponBoneList().Keys)
    //        {
    //            weaponBons.Add(key);
    //        }
    //    }
    //    return weaponBons;
    //}

    //武器自身挂点列表
    //private static Dictionary<string, string> WeaponSelfBonsList = new Dictionary<string, string>();
    //public static Dictionary<string, string> GetWeaponSelfBonsList()
    //{
    //    //右翅 weaponpoint （以及默认右手武器）
    //    //左翅 weaponpoint2（以及左手武器）
    //    //尾巴 weaponpoint3
    //    if (WeaponSelfBonsList.Count == 0)
    //    {

    //        WeaponSelfBonsList.Add(Weaponpoint, Weaponpoint);
    //        WeaponSelfBonsList.Add(Weaponpoint2, Weaponpoint2);
    //        WeaponSelfBonsList.Add(Weaponpoint3, Weaponpoint3);
    //    }
    //    return WeaponSelfBonsList;
    //}
    //private static List<string> weaponSelfBones = new List<string>();
    //public static List<string> WeaponSelfBones()
    //{
    //    if (weaponSelfBones.Count == 0)
    //    {
    //        foreach (string key in GetWeaponSelfBonsList().Keys)
    //        {
    //            weaponSelfBones.Add(key);
    //        }
    //    }
    //    return weaponSelfBones;
    //}

}
