using UnityEngine;
using System.Collections;

public class test_uv : MonoBehaviour
{
    public float x1speed = 0;
    public float y1speed = 0;

    
    private Vector2 v1;

    void Start()
    {
        v1 = Vector2.zero;
	
        //AssetBundleManager.GetBundle("dataconfig", getdata);
    }
    void Update()
    {	
	    v1.x += Time.fixedDeltaTime*x1speed;
        v1.y += Time.fixedDeltaTime*y1speed;

        if (GetComponent<Renderer>().materials.Length > 0)
        {
            Material ma = GetComponent<Renderer>().materials[0];
            if (ma != null)
            {
                if (ma.HasProperty("_MainTex"))
                {
                    ma.mainTextureOffset = v1;
                }
            }
        }       
    }
 

}
