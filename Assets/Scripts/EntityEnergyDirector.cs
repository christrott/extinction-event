using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnergyDirector : MonoBehaviour
{
    private Dictionary<TileTypes, int> playerEnergyGains;

    private void Start()
    {
        playerEnergyGains = new Dictionary<TileTypes, int>();
        playerEnergyGains[TileTypes.Grass] = 1;
        playerEnergyGains[TileTypes.Bush] = 2;
        playerEnergyGains[TileTypes.Tree] = 3;
        playerEnergyGains[TileTypes.SmallAnimal] = 5;
        playerEnergyGains[TileTypes.MediumAnimal] = 10;
        playerEnergyGains[TileTypes.LargeAnimal] = 20;
    }

    public int GetEnergyGainForTile(TileTypes tileType)
    {
        try
        {
            return playerEnergyGains[tileType];
        }
        catch
        {
            return 0;
        }
    }
}
