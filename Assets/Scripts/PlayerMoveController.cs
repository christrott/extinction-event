using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public Vector2 playerPos;
    public GameObject gameWonModal;
    public VoidController voidController;

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
            // If entity is not consumable
            energyManager.DeductMoveCost(newTileEntityTypes);
            playerPos = newPosition;
            var currentTileContainer = currentTile.GetComponent<TileEntityContainer>();
            TileEntity playerEntity = currentTileContainer.GetPlayerTileEntity();
            currentTile.GetComponent<TileEntityContainer>().RemoveEntity(playerEntity);
            var destinationType = newTile.GetComponent<TileEntityContainer>();
            newTile.GetComponent<TileEntityContainer>().AddEntity(playerEntity);
            voidController.SpreadVoid();
        }
    }

    private bool playerIsInWinPosition(Vector2 position)
    {
        var tile = boardManager.GetTile(position);
        var baseTileEntity = tile.GetComponent<TileEntityContainer>().GetBaseTileEntity();
        return (baseTileEntity.type == TileTypes.Exit);
    }
}
