using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{
    private static int TIER_1_MAX = 100;
    private static int TIER_2_MAX = 300;
    private static int TIER_3_MAX = 700;

    public int energy = 20;
    public int maxEnergy = TIER_1_MAX;
    public GameObject energyProgressBar;
    public GameObject gameOverPanel;
    private EntityCostDirector costManager;
    private EntityEnergyDirector gainManager;
    private PlayerTierManager tierManager;
    private PlayerMoveController moveController;
    private BoardManager boardManager;

    private void Start()
    {
        costManager = GetComponent<EntityCostDirector>();
        gainManager = GetComponent<EntityEnergyDirector>();
        tierManager = GetComponent<PlayerTierManager>();
        moveController = GetComponent<PlayerMoveController>();
        boardManager = GetComponent<BoardManager>();
        UpdateEnergy(energy);
    }

    public void DeductMoveCost(List<TileTypes> destinationTileTypes)
    {
        Debug.Log("DeductMoveCost(" + destinationTileTypes.Count + ")");
        var tileMoveCost = GetTileEnergyCost(destinationTileTypes);
        UpdateEnergy(energy - tileMoveCost);
        if (energy <= 0)
        {
            GameOver();
        }
    }

    public void AddEnergyFromFood(TileTypes foodType)
    {
        Debug.Log("AddEnergyFromFood(" + foodType + ")");
        var energyGain = gainManager.GetEnergyGainForTile(foodType);
        UpdateEnergy(energy + energyGain);

        if (energy >= maxEnergy)
        {
            TierUp();
        }
    }

    public void TierUp()
    {
        if (maxEnergy == TIER_1_MAX)
        {
            maxEnergy = TIER_2_MAX;
            var playerTile = boardManager.GetTile(moveController.playerPos);
            var entityContainer = playerTile.GetComponent<TileEntityContainer>();
            var entity = entityContainer.GetPlayerTileEntity();
            entityContainer.ReplaceEntity(entity, tierManager.playerTier2);
        } else
        {
            maxEnergy = TIER_3_MAX;
            var playerTile = boardManager.GetTile(moveController.playerPos);
            var entityContainer = playerTile.GetComponent<TileEntityContainer>();
            var entity = entityContainer.GetPlayerTileEntity();
            entityContainer.ReplaceEntity(entity, tierManager.playerTier3);
        }
        UpdateEnergy(energy);
    }

    public int GetTileEnergyCost(List<TileTypes> destinationTileTypes)
    {
        GameObject playerTile = GetComponent<PlayerMoveAdvisor>().GetTileAtPlayerPosition();
        //List<TileTypes> typesAtPlayer = playerTile.GetComponent<TileEntityContainer>().GetTileTypes();
        int highestCost = destinationTileTypes.Aggregate(0, (existingCost, newType) =>
        {
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

    private void GameOver()
    {
        Debug.Log("Game Over");
        gameOverPanel.SetActive(true);
    }
}
