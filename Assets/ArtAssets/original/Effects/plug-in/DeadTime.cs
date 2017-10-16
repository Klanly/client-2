using UnityEngine;
using System.Collections;

public class DeadTime : MonoBehaviour
{

    public float deadTime = 1.0f;

	void Start () {
        DestroyObject(gameObject,deadTime);
	
	}
	

}
