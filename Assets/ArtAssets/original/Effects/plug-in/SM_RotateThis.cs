using UnityEngine;
using System.Collections;

public class SM_RotateThis : MonoBehaviour {
    public float rotationSpeedX = 90;
    public float rotationSpeedY = 0;
    public float rotationSpeedZ = 0;

    public float delayTime = 0;

    private Vector3 rotationVector = Vector3.zero;

    #region Instance
    private static SM_RotateThis _instance;
    public static SM_RotateThis Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<SM_RotateThis>() as SM_RotateThis;
                if (_instance == null) {
                    _instance = new GameObject(typeof(SM_RotateThis).ToString()).AddComponent<SM_RotateThis>() as SM_RotateThis;
                }
            }
            return _instance;
        }
    }
    #endregion

    // Use this for initialization
    void Start() {
        rotationVector = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
    }

    // Update is called once per frame
    void Update() {

        if (delayTime <= 0) {
            delayTime = 0;
            transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
        } else
            delayTime -= Time.deltaTime;
    }
}
