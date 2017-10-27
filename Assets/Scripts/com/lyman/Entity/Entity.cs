using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    private int id;
    private string tag;

    public int ID
    {
        set { id = value; }
        get { return id; }
    }

    public virtual string Tag
    {
        set { tag = value; }
        get { return tag; }
    }

    public virtual void Update(float deltaTime)
    {
        
    }

    public virtual void Destroy()
    {

    }


    private static int index;
    public static int UniqueID
    {
        get
        {
            index++;
            return index;
        }
    }

	
}
