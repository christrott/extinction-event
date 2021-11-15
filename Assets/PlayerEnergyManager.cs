using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{
    public int energy = 20;
    public int maxEnergy = 100;
    public GameObject energyProgressBar;

    public void DeductMoveCost(List<TileTypes> destinationTileTypes)
    {
        Debug.Log("DeductMoveCost()");
        Debug.Log(destinationTileTypes);
        // TODO Don't just use the first, get the highest cost type
        var destinationTile = destinationTileTypes[0];
        var tileMoveCost = GetTileEnergyCost(destinationTile);
        UpdateEnergy(energy - tileMoveCost);
    }

    public void AddEnergyFromFood(TileTypes foodType)
    {
        // TODO
        Debug.Log("AddEnergyFromFood(" + foodType + ")");
    }

    public int GetTileEnergyCost(TileTypes destinationTile)
    {
        GameObject playerTile = GetComponent<PlayerMoveAdvisor>().GetTileAtPlayerPosition();
        TileTypes playerTileType = playerTile.GetComponent<TileEntityContainer>().GetTileTypes()[0];
        if (playerTileType == TileTypes.EventFront)
        {
            return 5;
        }
        else
        {
            // TODO Handle cost based on moving away from stronger enemies
            return 2;
        }
    }

    public void UpdateEnergy(int newEnergy)
    {
        energy = newEnergy;
        float energyFillRatio = (float)energy / (float)maxEnergy;
        energyProgressBar.GetComponent<ProgressBarController>().SetProgress(energyFillRatio);
    }
}
