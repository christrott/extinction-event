using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{
    public int energy = 20;
    public int maxEnergy = 100;
    public GameObject energyProgressBar;
    EntityCostDirector costManager;
    EntityEnergyDirector gainManager;

    private void Start()
    {
        costManager = GetComponent<EntityCostDirector>();
        UpdateEnergy(energy);
    }

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
        var energyGain = gainManager.GetEnergyGainForTile(foodType);
        UpdateEnergy(energy + energyGain);
    }

    public int GetTileEnergyCost(TileTypes destinationTile)
    {
        GameObject playerTile = GetComponent<PlayerMoveAdvisor>().GetTileAtPlayerPosition();
        List<TileTypes> typesAtPlayer = playerTile.GetComponent<TileEntityContainer>().GetTileTypes();
        int highestCost = typesAtPlayer.Aggregate(0, (existingCost, newTile) => {
            var newCost = costManager.GetCostForTile(newTile);
            return existingCost > newCost ? existingCost : newCost;
        });
        return highestCost;
    }

    public void UpdateEnergy(int newEnergy)
    {
        energy = newEnergy;
        float energyFillRatio = (float)energy / (float)maxEnergy;
        energyProgressBar.GetComponent<ProgressBarController>().SetProgress(energyFillRatio);
    }
}
