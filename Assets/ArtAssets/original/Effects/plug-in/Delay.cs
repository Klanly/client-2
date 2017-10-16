using UnityEngine;
using System.Collections;

public class Delay : MonoBehaviour {
	
	public float delayTimer = 1.0f;
	
	// Use this for initialization
	void Start () {		
		gameObject.SetActive(false);
		Invoke("DelayFunc", delayTimer);
	}
	
	void DelayFunc()
	{
		gameObject.SetActive(true);
	}
	
}
