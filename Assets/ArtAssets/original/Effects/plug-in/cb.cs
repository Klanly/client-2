using UnityEngine;
using System.Collections;

public class cb : MonoBehaviour
{

	public float delayTime = 1.0f;
	
	// Use this for initialization
	void Start () {		
		Invoke("DestroyFunc", delayTime);
	}
	
	void DestroyFunc()
	{
		GameObject.Destroy(this.gameObject);
	}
}
