using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCostDirector : MonoBehaviour
{
    public Dictionary<TileTypes, int> playerTierCosts;

    private void Start()
    {
        playerTierCosts = new Dictionary<TileTypes, int>();
        playerTierCosts.Add(TileTypes.Grass, 0);
        playerTierCosts.Add(TileTypes.Bush, 1);
        playerTierCosts.Add(TileTypes.Tree, 1);
        playerTierCosts.Add(TileTypes.EventFront, 5);
        playerTierCosts.Add(TileTypes.EventVoid, 10);
        playerTierCosts.Add(TileTypes.SmallAnimal, 1);
        playerTierCosts.Add(TileTypes.MediumAnimal, 10);
        playerTierCosts.Add(TileTypes.LargeAnimal, 15);
    }

    public int GetCostForTile(TileTypes tileType)
    {
        Debug.Log("GetCostForTile(" + tileType + ")");
        try
        {
            return playerTierCosts[tileType];
        }
        catch
        {
            return 0;
        }
    }
}
