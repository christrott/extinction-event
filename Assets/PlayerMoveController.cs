using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public Vector2 playerPos;

    private BoardManager boardManager;
    private PlayerEnergyManager energyManager;

    // Start is called before the first frame update
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
        energyManager = GetComponent<PlayerEnergyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO Add keyboard controls for player movement
    }

    public void MovePlayer(Vector2 newPosition)
    {
        if (GetComponent<PlayerMoveAdvisor>().IsValidMovePosition(newPosition))
        {
            var currentTile = boardManager.GetTile(playerPos);
            var newTile = boardManager.GetTile(newPosition);
            // TODO Update tiles with new status
            energyManager.DeductMoveCost(newTile.GetComponent<TileEntityContainer>().GetTileTypes());
            var currentTileContainer = currentTile.GetComponent<TileEntityContainer>();
            TileEntity playerEntity = getPlayerEntityFromTile(currentTileContainer);
            currentTile.GetComponent<TileEntityContainer>().RemoveEntity(playerEntity);
            var destinationType = newTile.GetComponent<TileEntityContainer>();
            newTile.GetComponent<TileEntityContainer>().AddEntity(playerEntity);
            playerPos = newPosition;
        }
    }

    private TileEntity getPlayerEntityFromTile(TileEntityContainer tile)
    {
        return tile.GetTileEntityByType(TileTypes.PlayerTier1);
    }
}
