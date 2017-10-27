using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    private static Dictionary<int, Entity> entitys = new Dictionary<int, Entity>();
    public static Entity GetEntityByID(int id)
    {
        Entity entity = null;
        entitys.TryGetValue(id, out entity);
        return entity;
    }

    public static void AddEntity(Entity entity)
    {
        if(!entitys.ContainsKey(entity.ID))
            entitys.Add(entity.ID, entity);
    }
}
