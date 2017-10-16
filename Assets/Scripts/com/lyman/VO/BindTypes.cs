using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BindTypes
{

    
    public static string NONE = GameConst.NONE;
    
    public static int None = 0;
    public static int OrginePoint = 1;  //自身同坐标,随角色移动
    public static int Self_Point = 2;   //自身同坐标,不随角色移动
    public static int Target_Range = 3; //目标范围
    public static int Bone = 4;         //绑骨骼
    
    

    
    private static Dictionary<string, int> keyToValue = new Dictionary<string, int>();
    private static Dictionary<int, string> valueToKey = new Dictionary<int, string>();

    public static Dictionary<string, int>.KeyCollection Types()
    {
        if (keyToValue.Count == 0)
        {
            keyToValue.Add(NONE, None);
            valueToKey.Add(None, NONE);
            
            string key = OrginePoint + "_自身同坐标,随角色移动";
            keyToValue.Add(key, OrginePoint);
            valueToKey.Add(OrginePoint, key);
            
            key = Bone + "_骨骼或点";
            keyToValue.Add(key, Bone);
            valueToKey.Add(Bone, key);

            key = Target_Range + "_目标范围，不随目标移动";
            keyToValue.Add(key, Target_Range);
            valueToKey.Add(Target_Range, key);

            key = Self_Point + "_自身同坐标,不随角色移动";
            keyToValue.Add(key, Self_Point);
            valueToKey.Add(Self_Point, key);
        }
        return keyToValue.Keys;
    }
    public static int GetType(string key)
    {
        int ty;
        keyToValue.TryGetValue(key, out ty);
        return ty;
    }
    public static string GetKey(int type)
    {
        string cn;
        valueToKey.TryGetValue(type, out cn);
        return cn;
    }
}
