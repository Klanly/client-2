using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class CommonProperties
{

    
    public float aa;//生命
    public float ab;//物攻
    public float ac;//魔攻
    public float ad;//物防
    public float ae;//魔防
    public float af;//命中
    public float ag;//闪避
    public float ah;//暴击
    public float ai;//破击
    public float aj;//暴伤
    public float ak;//韧性
    public float al;//能量
    public float au;//霸体




    public float am;//发力回复
    public float an;//活力
    public float ao;//活力回复
    public float ap;// 生命回复
    public float ar;//吸血
    
    public float AS;//


    public float speed_scale = 1f;//行走速度倍率
    public float atk_speed_scale = 1f;//攻击速递倍率

    public Dictionary<string, float> protertys = new Dictionary<string, float>();


    
    public void AddProperty(string key, float value)
    {
        
        if (value > 0f)
            protertys.Add(key, value);
    }


    public void AddPropertys(string content)
    {
        string[] valus = content.Split(",".ToCharArray());
        for (int i = 0; i < valus.Length; i++)
        {
            string temp = valus[i];
            string[] temps = temp.Split(":".ToCharArray());

            string key = temps[0];
            string value = temps[1];

            float vv;
            float.TryParse(value, out vv);
            AddProperty(key, vv);
        }
    }

    public float GetProperty(string key)
    {
        float value;
        protertys.TryGetValue(key, out value);
        return value;
    }

    
    

    public virtual void Copy(CommonProperties commonProperties)
    {
        this.aa = commonProperties.aa;
        this.ab = commonProperties.ab;
        this.ac = commonProperties.ac;
        this.ad = commonProperties.ad;
        this.ae = commonProperties.ae;
        this.af = commonProperties.af;
        this.ag = commonProperties.ag;
        this.ah = commonProperties.ah;
        this.ai = commonProperties.ai;
        this.aj = commonProperties.aj;
        this.ak = commonProperties.ak;
        this.al = commonProperties.al;
        this.am = commonProperties.am;
        this.an = commonProperties.an;
        this.ao = commonProperties.ao;
        this.ap = commonProperties.ap;
        this.au = commonProperties.au;
        this.speed_scale = commonProperties.speed_scale;
        this.AS = commonProperties.AS;
    }
}
