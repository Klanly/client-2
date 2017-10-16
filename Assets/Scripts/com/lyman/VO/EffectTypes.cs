using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTypes
{
    public static int Normal = 0;//普通
    public static int Bullet = 1;//子弹类(飞行)

    private static Dictionary<int, string> keyToValue = new Dictionary<int, string>();
    private static Dictionary<string, int> valueToKey = new Dictionary<string, int>();
    public static void GetTypes()
    {
        if (keyToValue.Count == 0)
        {
            string des = Normal + "_普通";
            keyToValue.Add(Normal, des);
            valueToKey.Add(des, Normal);

            des = Bullet + "_子弹类(飞行)";
            keyToValue.Add(Bullet, des);
            valueToKey.Add(des, Bullet);
        }
    }


    public static Dictionary<string, int>.KeyCollection Types()
    {
        GetTypes();
        return valueToKey.Keys;
    }

    public static string GetValue(int key)
    {
        GetTypes();
        string value;
        keyToValue.TryGetValue(key, out value);
        return value;
    }

    public static int GetKey(string value)
    {
        GetTypes();
        int key;
        valueToKey.TryGetValue(value, out key);
        return key;
    }

}
