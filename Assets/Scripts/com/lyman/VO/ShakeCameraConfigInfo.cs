using UnityEngine;
using System.Collections;

public class ShakeCameraConfigInfo
{
    public string id;
    public int type;
    public bool isdiminishing;
    public float range;
    public float firsttime;
    public float secondtime;


    
    private int _number_of_shakes;
    public int number_of_shakes
    {
        get { return _number_of_shakes; }
        set
        {
            _number_of_shakes = value;
            if (_number_of_shakes < 1)
            {
                _number_of_shakes = 1;
            }
        }
    }

    private float _shake_amount_x;
    public float shake_amount_x
    {
        get { return _shake_amount_x; }
        set
        {
            _shake_amount_x = value;
            if (_shake_amount_x < 0f)
            {
                _shake_amount_x = 0f;
            }
        }
    }


    private float _shake_amount_y;
    public float shake_amount_y
    {
        get { return _shake_amount_y; }
        set
        {
            _shake_amount_y = value;
            if (_shake_amount_y < 0f)
            {
                _shake_amount_y = 0f;
            }
        }
    }

    private float _shake_amount_z;
    public float shake_amount_z
    {
        get { return _shake_amount_z; }
        set
        {
            _shake_amount_z = value;
            if (_shake_amount_z < 0f)
            {
                _shake_amount_z = 0f;
            }
        }
    }

    private Vector3 _shake_amount = Vector3.zero;
    public Vector3 shake_amount
    {
        get
        {
            _shake_amount.Set(this._shake_amount_x, this._shake_amount_y, this._shake_amount_z);
            return _shake_amount;
        }
    }

    private Vector3 _rotation_amount = Vector3.zero;
    public Vector3 rotation_amount
    {
        get
        {
            _rotation_amount.Set(this._rotation_amount_x, this._rotation_amount_y, this._rotation_amount_z);
            return _rotation_amount;
        }
    }

    

    private float _rotation_amount_x;
    public float rotation_amount_x
    {
        get { return _rotation_amount_x; }
        set
        {
            _rotation_amount_x = value;
            if (_rotation_amount_x < 0f)
            {
                _rotation_amount_x = 0f;
            }
            else if (_rotation_amount_x > 10f)
            {
                _rotation_amount_x = 10f;
            }
        }
    }

    private float _rotation_amount_y;
    public float rotation_amount_y
    {
        get { return _rotation_amount_y; }
        set
        {
            _rotation_amount_y = value;
            if (_rotation_amount_y < 0f)
            {
                _rotation_amount_y = 0f;
            }
            else if (_rotation_amount_y > 10f)
            {
                _rotation_amount_y = 10f;
            }
        }
    }

    private float _rotation_amount_z;
    public float rotation_amount_z
    {
        get { return _rotation_amount_z; }
        set
        {
            _rotation_amount_z = value;
            if (_rotation_amount_z < 0f)
            {
                _rotation_amount_z = 0f;
            }
            else if (_rotation_amount_z > 10f)
            {
                _rotation_amount_z = 10f;
            }
        }
    }
    


    private float _distance;

    public float distance
    {
        get { return _distance/10f; }
        set
        {
            _distance = value;
            if (_distance < 0f)
            {
                _distance = 0f;
            }
            else if (_distance > 1f)
            {
                _distance = 1f;
            }
        }
    }

    private float _speed;

    public float speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
            if (_speed < 0f)
            {
                _speed = 0f;
            }
            else if (_speed > 100f)
            {
                _speed = 100f;
            }
        }
    }


    private float _decay;

    public float decay
    {
        get { return _decay; }
        set
        {
            _decay = value;
            if (_decay < 0f)
            {
                _decay = 0f;
            }
            else if(_decay > 1f)
            {
                _decay = 1f;
            }
        }
    }
}
