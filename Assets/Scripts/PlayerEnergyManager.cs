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
    public AudioClip tierUpSfx;

    private EntityCostDirector costManager;
    private EntityEnergyDirector gainManager;
    private PlayerTierManager tierManager;
    private PlayerMoveController moveController;
    private BoardManager boardManager;
    private EntityPriorityManager priorityManager;

    private void Start()
    {
        costManager = GetComponent<EntityCostDirector>();
        gainManager = GetComponent<EntityEnergyDirector>();
        tierManager = GetComponent<PlayerTierManager>();
        moveController = GetComponent<PlayerMoveController>();
        boardManager = GetComponent<BoardManager>();
        priorityManager = GetComponent<EntityPriorityManager>();
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
        Debug.Log("Energy Gain " + energyGain);
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
            playerTile.GetComponent<AudioSource>().PlayOneShot(tierUpSfx);
            var entityContainer = playerTile.GetComponent<TileEntityContainer>();
            var entity = entityContainer.GetPlayerTileEntity();
            entityContainer.ReplaceEntity(entity, tierManager.playerTier2);
        }
        else
        {
            maxEnergy = TIER_3_MAX;
            var playerTile = boardManager.GetTile(moveController.playerPos);
            playerTile.GetComponent<AudioSource>().PlayOneShot(tierUpSfx);
            var entityContainer = playerTile.GetComponent<TileEntityContainer>();
            var entity = entityContainer.GetPlayerTileEntity();
            entityContainer.ReplaceEntity(entity, tierManager.playerTier3);
        }
        UpdateEnergy(energy);
    }

    public int GetTileEnergyCost(List<TileTypes> destinationTileTypes)
    {
        GameObject playerTile = GetComponent<PlayerMoveAdvisor>().GetTileAtPlayerPosition();
        TileEntity playerEntity = playerTile.GetComponent<TileEntityContainer>().GetPlayerTileEntity();
        int playerPriority = priorityManager.GetTileTypeRankIndex(playerEntity.type);
        Debug.Log(playerEntity + " - " + playerPriority);
        TileTypes entityType = TileTypes.Empty;
        int highestCost = destinationTileTypes.Aggregate(0, (existingCost, newType) =>
        {
            var newCost = costManager.GetCostForTile(newType);
            if (existingCost > newCost)
            {
                return existingCost;
            } else
            {
                entityType = newType;
                return newCost;
            }
        });

        int entityPriority = priorityManager.GetTileTypeRankIndex(entityType);
        Debug.Log(entityType + " - " + entityPriority);
        if (entityPriority < playerPriority)
        {
            return highestCost;
        }
        else
        {
            return 1;
        }
    }

    public void UpdateEnergy(int newEnergy)
    {
        Debug.Log("Update energy from " + energy + " to " + newEnergy);
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
