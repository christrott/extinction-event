using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public Vector2 playerPos;
    public GameObject gameWonModal;

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
            if (playerIsInWinPosition(newPosition)) {
                gameWonModal.SetActive(true);
                return;
            }
            var currentTile = boardManager.GetTile(playerPos);
            var newTile = boardManager.GetTile(newPosition);
            var newTileEntityTypes = newTile.GetComponent<TileEntityContainer>().GetTileTypes();
            energyManager.DeductMoveCost(newTileEntityTypes);
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

    private bool playerIsInWinPosition(Vector2 position)
    {
        var tile = boardManager.GetTile(position);
        var baseTileEntity = tile.GetComponent<TileEntityContainer>().GetBaseTileEntity();
        return (baseTileEntity.type == TileTypes.Exit);
    }
}
