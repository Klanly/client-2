using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TT : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RaycastHit raycastHit;
        Vector3 startPosition = new Vector3(0.5f, 30f, 73.5f);// 73.5f);

        bool isHit = Physics.Raycast(startPosition, Vector3.down * 100f, out raycastHit);
        if (isHit)
        {
            Debug.Log("isHit");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
