using UnityEngine;
using System.Collections;

public class test_two_uv : MonoBehaviour {

	public float x1speed = 0;
	public float y1speed = 0;

	public float x2speed = 0;
	public float y2speed = 0;
	
	private Vector2 v1;
	private Vector2 v2;
	Material mat1;
	Material mat2;

	void Start()
	{
		v1 = Vector2.zero;
		v2 = Vector2.zero;
		//AssetBundleManager.GetBundle("dataconfig", getdata);
		Material[] materials = this.transform.GetComponent<Renderer>().materials;
		if(materials.Length>=1) mat1 = GetComponent<Renderer>().materials[0];
		if(materials.Length>=2) mat2 = GetComponent<Renderer>().materials[1];
	}
	void Update()
	{	
		if (mat1 != null) {
			v1.x += Time.fixedDeltaTime * x1speed;
			v1.y += Time.fixedDeltaTime * y1speed;
            if(mat1.HasProperty("_MainTex"))
            {
                mat1.mainTextureOffset = v1;
            }			
		}
		if (mat2 != null) {
			v2.x += Time.fixedDeltaTime * x2speed;
			v2.y += Time.fixedDeltaTime * y2speed;
            if (mat2.HasProperty("_MainTex"))
            {
                mat2.mainTextureOffset = v2;
            }
		}
	}
}
