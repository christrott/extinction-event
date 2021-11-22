using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileEntityContainer : MonoBehaviour
{
    public GameObject entityDisplayObject;

    private TileEntity emptyTileEntity = new TileEntity {
        name = "",
        sprite = null,
        type = TileTypes.Empty,
    };
    private PlayerEnergyManager playerEnergyManager;
    private EntityPriorityManager priorityManager;
    private List<TileEntity> tileEntities;

    private void Start()
    {
        var board = GameObject.Find("Board");
        priorityManager = board.GetComponent<EntityPriorityManager>();
        playerEnergyManager = board.GetComponent<PlayerEnergyManager>();
    }

    public void AddEntity(TileEntity newEntity)
    {
        if (tileEntities == null)
        {
            tileEntities = new List<TileEntity>();
        }
        if (tileEntities.Count == 1 && tileEntities[0].type == TileTypes.Empty)
        {
            tileEntities.Remove(emptyTileEntity);
        }
        
        tileEntities.Add(newEntity);
        if (tileEntities.Count > 1)
        {
            ResolveInteraction(tileEntities[0], tileEntities[1]);
        }
        else
        {
            UpdateEntity(newEntity);
        }
    }

    public TileEntity GetBaseTileEntity()
    {
        return tileEntities[0];
    }

    public List<TileTypes> GetTileTypes()
    {
        return tileEntities.Select(entity => entity.type).ToList();
    }

    public void RemoveEntity(TileEntity entity)
    {
        tileEntities.Remove(entity);
        if (tileEntities.Count > 0)
        {
            UpdateEntity(tileEntities[0]);
        }
        else
        {
            AddEntity(emptyTileEntity);
            UpdateEntity(emptyTileEntity);
        }
    }

    public TileEntity GetTileEntityByType(TileTypes type)
    {
        return tileEntities.Find((entity) => entity.type == type);
    }

    public TileEntity GetTileEntityExcluding(TileTypes excludeType)
    {
        return tileEntities.Find((entity) => entity.type != excludeType);
    }

    private void ResolveInteraction(TileEntity entity1, TileEntity entity2)
    {
        //Debug.Log("ResolveInteraction " + entity1.name + ", " + entity2.name);
        TileTypes dominantType = priorityManager.GetTileTypePriority(entity1.type, entity2.type);

        if (dominantType == TileTypes.PlayerTier1)
        {
            var consumedEntity = (entity1.type == TileTypes.PlayerTier1) ? entity2 : entity1;
            playerEnergyManager.AddEnergyFromFood(consumedEntity.type);
        }

        if (entity1.type == dominantType)
        {
            RemoveEntity(entity2);
            UpdateEntity(entity1);
        } else
        {
            RemoveEntity(entity1);
            UpdateEntity(entity2);
        }
    }

    private void UpdateEntity(TileEntity entity)
    {
        entityDisplayObject.GetComponent<SpriteRenderer>().sprite = entity.sprite;
    }
}
