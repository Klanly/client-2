using UnityEngine;
using System;
using System.IO;
using System.Text;
public class SceneEntity : Entity
{
    
    private CommonModel model;
    protected string tag;
    protected string layer;
    protected Vector3 position = Vector3.zero;
    protected Vector3 rotation = Vector3.zero;
    private Vector3 scale = Vector3.one;
    private string name;
    protected bool isActive;

    public SceneEntity()
    {

    }
    
    public CommonModel Model
    {
        get { return model; }
        set { model = value; }
    }

    public override string Tag
    {
        get
        {
            return base.Tag;
        }
        set
        {
            base.Tag = value;
            if (model != null)
                model.Tag = tag;
        }
    }
    
    
    public string Name
    {
        set
        {
            name = value;
            if (model != null)
                model.Name = name;
        }
        get
        {
            return name;
        }
    }

    

    public void SetLayer(string layerValue)
    {
        layer = layerValue;
        if (Model != null)
        {
            Model.Layer = LayerMask.NameToLayer(layer);
        }
    }

    
    public virtual void SetScale(float x, float y, float z)
    {
        scale.Set(x,y,z);
        if (Model != null)
        {
            Model.LocalScale = scale;
        }
    }

    public void SetPosition(float x, float y, float z)
    {
        position.Set(x, y, z);
        if (Model != null)
        {
            Model.Position = position;
        }
    }
    public Vector3 GetPosition()
    {
        if (Model != null)
            position.Set(Model.Position.x, Model.Position.y, Model.Position.z);
        return position;
    }

    public void SetRotation(float x, float y, float z)
    {
        rotation.Set(x, y, z);
        if (Model != null )
        {
            Model.Rotation = Quaternion.Euler(rotation);
        }
    }
    
    public void FaceTo(Vector3 position, bool isLockXZ = true)
    {
        if (Model != null)
        {
            Model.LookAt(position);
            if (isLockXZ)
            {
                SetRotation(0f, Model.Euler.y, 0f);
            }
        }
    }
    
    public virtual void Hide()
    {
        isActive = false;
        if (Model != null)
        {
            Model.Hide();
        }
    }

    public virtual void Show()
    {
        isActive = true;
        if (Model != null)
        {
            Model.Show();
        }

    }
    public override void Destroy()
    {
        base.Destroy();
        if (Model != null)
        {
            Model.Destroy();
            Model = null;
        }
    }
}