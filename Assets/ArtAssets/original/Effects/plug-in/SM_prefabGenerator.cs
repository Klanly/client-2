using UnityEngine;
using System.Collections;

public class SM_prefabGenerator : MonoBehaviour {

    public GameObject[] createThis; // 实例化目标对象的数组
    private int rndNr; //随机控制实例化数组中的对象
    public int thisManyTimes = 60;//产生的总数量
    public float overThisTime = 3f;//持续产生的时间
    private float thisLifeTime = 1.0f;

    public float xWidth = 5;
    public float yWidth;
    public float zWidth = 5;

    public float xRotMax;
    public float yRotMax;
    public float zRotMax;

    public bool allUseSameRotation = false;
    public bool detachToWorld = false;

    private bool allRotationDecided = false;

    private float x_cur;
    private float y_cur;
    private float z_cur;

    private float xRotCur;
    private float yRotCur;
    private float zRotCur;

    private float timeCounter;
    private int effectCounter;
    private float trigger;


	void Start ()
    {
        if (thisManyTimes < 1) thisManyTimes = 1;
        trigger = overThisTime / thisManyTimes; 
	}

    private void ClearChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    void OnEnable()
    {
        trigger = overThisTime / thisManyTimes; 
        ClearChildren();
        timeCounter = 0;//重置时间计时器
        effectCounter = 0;//重置特效计数器
    }

    void Update () 
    {
	    timeCounter += Time.deltaTime;

        if(effectCounter > thisManyTimes && transform.childCount > 0) 
        {
            Invoke("ClearChildren", thisLifeTime);
        }

	    if(timeCounter > trigger && effectCounter <= thisManyTimes)
		{
             rndNr = Mathf.FloorToInt(Random.value * createThis.Length);//Random.value生成的随机数范围[0, 1]
             x_cur = transform.position.x + (Random.value * xWidth) - xWidth * 0.5f; 
             y_cur = transform.position.y + (Random.value * yWidth) - yWidth * 0.5f;
             z_cur = transform.position.z + (Random.value * zWidth) - zWidth * 0.5f;

            if (allUseSameRotation == false || allRotationDecided == false)  //allUseSameRotation == true的话则所有实例化的对象的Rotation一样
		    {
		        xRotCur = transform.rotation.x + (Random.value * xRotMax * 2) - xRotMax;
		        yRotCur = transform.rotation.y + (Random.value * yRotMax * 2) - yRotMax;  
		        zRotCur = transform.rotation.z + (Random.value * zRotMax * 2) - zRotMax;  
		        allRotationDecided = true;
		    }
            GameObject obj = Instantiate(createThis[rndNr], new Vector3(x_cur, y_cur, z_cur), transform.rotation) as GameObject;  //创建预设
		    obj.transform.Rotate(xRotCur, yRotCur, zRotCur);

            if (detachToWorld == false)  // detachToWorld == true的话则挂在世界坐标下
		    {
			    obj.transform.parent = transform;
		    }
            timeCounter = 0;
		    effectCounter ++;
		}
	}
}
