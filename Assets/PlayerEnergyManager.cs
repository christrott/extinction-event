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
        gainManager = GetComponent<EntityEnergyDirector>();
        UpdateEnergy(energy);
    }

    public void DeductMoveCost(List<TileTypes> destinationTileTypes)
    {
        Debug.Log("DeductMoveCost(" + destinationTileTypes.Count + ")");
        var tileMoveCost = GetTileEnergyCost(destinationTileTypes);
        UpdateEnergy(energy - tileMoveCost);
    }

    public void AddEnergyFromFood(TileTypes foodType)
    {
        Debug.Log("AddEnergyFromFood(" + foodType + ")");
        var energyGain = gainManager.GetEnergyGainForTile(foodType);
        UpdateEnergy(energy + energyGain);
    }

    public int GetTileEnergyCost(List<TileTypes> destinationTileTypes)
    {
        GameObject playerTile = GetComponent<PlayerMoveAdvisor>().GetTileAtPlayerPosition();
        //List<TileTypes> typesAtPlayer = playerTile.GetComponent<TileEntityContainer>().GetTileTypes();
        int highestCost = destinationTileTypes.Aggregate(0, (existingCost, newType) => {
            var newCost = costManager.GetCostForTile(newType);
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
