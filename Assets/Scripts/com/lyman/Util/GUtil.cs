using UnityEngine;
using System.Collections;

//图形工具
public class GUtil
{
    public static float GetDistance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(GetDistancePower(x1, y1, x2, y2));
    }
    public static float GetDistancePower(float x1, float y1, float x2, float y2)
    {
        return Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2);
    }
    public static float GetDistance(Transform from, Transform to)
    {
        return GetDistance(from.position.x, to.position.x, from.position.z, to.position.z);
    }
    public static Vector3 GetVetor(Vector3 from, Vector3 to)
    {
        return new Vector3(to.x - from.x, from.y, to.z - from.z);
    }


    public static bool IsIn(Vector3 source,Vector3 forward,Vector3 target,float radius,float angle,bool offset = true)
    {
        if (offset && Vector3.Distance(target, source) > radius)
        {
            return false;
        }

        Vector3 direction = target - source;
        float an = Vector3.Angle(direction, forward);
        //Debug.LogError("配置角度:"+ angle+" 实际角度:"+ an);
        if (an > angle)
            return false;
        return true;
    }

    

    /// 
    /// 判定攻击目标是否在攻击者扇形攻击范围类
    /// 
    /// 攻击者
    /// 被击者
    /// 扇形半径
    /// 扇形角度
    /// 
    public static bool IsInSector(Vector3 source, Vector3 target, Quaternion rotation,float radius, float angle)
    {

        



        float STDistance = GetDistance(source.x, source.z, target.x, target.z);

        Vector3 _tVector = GetVetor(source, target);
        Vector3 _sVector = (source + (rotation * Vector3.forward) * radius);
        float targetAngle = Vector3.Angle(_tVector, _sVector);
        //Debug.LogError("targetAngle:"+targetAngle);
        if (STDistance <= radius && targetAngle <= angle)
            return false;
        return false;
    }


    public static float Angle(float radius, Quaternion rotation, Vector3 source, Vector3 target)
    {
        float STDistance = GetDistance(source.x, source.z, target.x, target.z);

        Vector3 _tVector = GetVetor(source, target);
        Vector3 _sVector = (source + (rotation * Vector3.forward) * radius);
        float targetAngle = Vector3.Angle(_tVector, _sVector);

        return targetAngle;
    }

    //  圆点坐标：(orignX,orignY)
    //  半径：r
    //  角度：angle       
    //  则圆上任一点为：（x1,y1）
    public static Vector3 GetPosition(float radius,float angle,Vector3 origin)
    {
        float orignX = origin.x;
        float orignY = origin.z;
        var x1 = orignX + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        var y1 = orignY + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(x1, origin.y, y1);
    }
    public static Vector3 ResetPoint(Vector3 pos)
    {
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(pos, Vector3.down * 5f, out hitInfo);
        if (isHit)
        {
            pos = hitInfo.point;
        }
        return pos;
    }



}
