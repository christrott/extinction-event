using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntityContainer : MonoBehaviour
{
    public GameObject entityObject;
    private List<TileEntity> tileEntities;

    public void AddEntity(TileEntity newEntity)
    {
        if (tileEntities == null)
        {
            tileEntities = new List<TileEntity>();
        }
        tileEntities.Add(newEntity);
        if (tileEntities.Count > 1)
        {
            ResolveInteraction(tileEntities[0], newEntity);
        }
        else
        {
            UpdateEntity(newEntity);
        }
    }

    public void RemoveEntity(TileEntity entity)
    {
        tileEntities.Remove(entity);
    }

    private void ResolveInteraction(TileEntity entity1, TileEntity entity2)
    {
        Debug.Log("ResolveInteraction " + entity1.name + ", " + entity2.name);
    }

    private void UpdateEntity(TileEntity entity)
    {
        entityObject.GetComponent<SpriteRenderer>().sprite = entity.sprite;
    }
}
