using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


//摄像机控制
public class EditorCameraController : MonoBehaviour
{
    public bool IsGameEye;
    //摄像机偏移量
    public Vector3 TargetOffset = new Vector3(0f, 2f, 0f);

    //观察目标
    public GameObject Target;

    //观察距离
    public float Distance = 5f;
    
    //鼠标缩放距离最值
    private float MaxDistance = 100f;
    private float MinDistance = 5F;

    //旋转速度
    public float SpeedX = 150f;
    public float SpeedY = 120f;
    //角度限制
    private float MinLimitY = 10f;
    private float MaxLimitY = 45f;

    //旋转角度
    private float mX = 0F;
    private float mY = 0F;
    
    //鼠标缩放速率
    private float ZoomSpeed = 2F;

    //是否启用差值
    public bool isNeedDamping = true;
    //速度
    public float Damping = 8F;

    public bool isFirst = true;
    
    private Camera myCamera;


    private Vector3 eulerAngles = Vector3.zero;
    private Vector3 position = Vector3.zero;
    private Vector3 tposition = Vector3.zero;

    private static EditorCameraController instance;

    public static EditorCameraController Instance
    {
        get
        {
            return instance;
        }
    }


    void Start()
    {
        instance = this;
        //初始化旋转角度
        myCamera = Camera.main;
        mX = myCamera.transform.eulerAngles.x;
        mY = myCamera.transform.eulerAngles.y;
        myCamera.nearClipPlane = 1f;
    }

    

    //public void Flex(bool flex, float range = 0f, float firstTime = 0f, float secondTime=  0f, bool isDiminishing = true)
    //{
    //    if (flexCamera != null)
    //    {
    //        flexCamera.FlexRange = range;
    //        flexCamera.FirstHalfTime = firstTime;
    //        flexCamera.SecondHalfTime = secondTime;
    //        flexCamera.IsDiminishing = isDiminishing;
    //        flexCamera.IsStart = flex;
    //    }
    //}


    void FixedUpdate()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0))
        {
            return;
        }
        if (Target == null)
            return;
        if (isFirst)
        {
            mY = MaxLimitY;
        }
        //鼠标左键旋转
        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButton(0))
            {
                //获取鼠标输入
                mX += Input.GetAxis("Mouse X") * SpeedX * 0.02F;
                mY -= Input.GetAxis("Mouse Y") * SpeedY * 0.02F;
                //范围限制
                mY = ClampAngle(mY, MinLimitY, MaxLimitY);
            }
            else if (Target != null && Input.GetMouseButton(0) == false && isFirst == true)
            {
                mY = MaxLimitY;
                mX = Target.transform.localEulerAngles.y;
            }
            //鼠标滚轮缩放
            if (!IsGameEye)
            {
                Distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
                Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
            }
        }
        //重新计算位置和角度
        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        tposition.Set(0F, 0F, -Distance);
        Vector3 mPosition = mRotation * tposition + Target.transform.position;

        //设置相机的角度和位置
        if (isNeedDamping && isFirst == false)
        {
            //球形插值
            myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, mRotation, Time.deltaTime * Damping);
            //线性插值
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            myCamera.transform.rotation = mRotation;
            myCamera.transform.position = mPosition;
        }
        eulerAngles.Set(0f, mX, 0f);
        //将玩家转到和相机对应的位置上
        if (IsChangeEuler)
        {
            Target.transform.eulerAngles = eulerAngles;
        }
        if (IsGameEye)
        {
            Distance = 5f;
            Target.transform.eulerAngles = eulerAngles;
            position.Set(Target.transform.position.x + TargetOffset.x, Target.transform.position.y + TargetOffset.y, Target.transform.position.z + TargetOffset.z);
            transform.LookAt(position);
        }
        isFirst = false;
    }

    
    //public void ShakeCamera(ShakeCameraConfigInfo shakeCameraConfigInfo)
    //{
    //    if (shakeCameraConfigInfo != null)
    //    {
    //        //  stop = true;
    //        if (shakeCameraConfigInfo.type == 0)
    //            CameraShake.Shake(shakeCameraConfigInfo.number_of_shakes, shakeCameraConfigInfo.shake_amount, shakeCameraConfigInfo.rotation_amount, shakeCameraConfigInfo.distance, shakeCameraConfigInfo.speed, shakeCameraConfigInfo.decay, OnShakeCompleteHandler);
    //        else
    //            Flex(shakeCameraConfigInfo);
    //    }
    //}

    //private ShakeCameraConfigInfo shakeCameraConfigInfo;
    //private float startValue;
    //private float time;
    //public void Flex(ShakeCameraConfigInfo cInfo)
    //{
    //    shakeCameraConfigInfo = cInfo;
    //    startValue = myCamera.fieldOfView;
    //    time = 0f;
    //    TimerManager.AddCsHandler(OnDelHandler1);
    //}
    //private void OnDelHandler1(float del)
    //{
    //    time += del;
    //    if (time <= shakeCameraConfigInfo.firsttime)
    //    {
    //        float precent = shakeCameraConfigInfo.range * (time / shakeCameraConfigInfo.firsttime);
    //        if (shakeCameraConfigInfo.isdiminishing)
    //        {
    //            myCamera.fieldOfView = startValue + precent;
    //        }
    //        else
    //        {
    //            myCamera.fieldOfView = startValue - precent;
    //        }

    //    }
    //    else
    //    {
    //        TimerManager.RemoveCsHandler(OnDelHandler1);
    //        startValue = myCamera.fieldOfView;
    //        time = 0f;
    //        TimerManager.AddCsHandler(OnDelHandler2);
    //    }

    //}

    //private void OnDelHandler2(float del)
    //{
    //    time += del;
    //    if (time <= shakeCameraConfigInfo.secondtime)
    //    {
    //        float precent = shakeCameraConfigInfo.range * (time / shakeCameraConfigInfo.secondtime);
    //        if (shakeCameraConfigInfo.isdiminishing)
    //        {
    //            myCamera.fieldOfView = startValue - precent;
    //        }
    //        else
    //        {
    //            myCamera.fieldOfView = startValue + precent;
    //        }
    //    }
    //    else
    //    {
    //        TimerManager.RemoveCsHandler(OnDelHandler2);
    //    }
    //}
    //public bool stop;
    //private void OnShakeCompleteHandler()
    //{
    //    stop = false;
    //}

    public bool IsChangeEuler = true;
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
