using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    public BoardManager boardManager;
    public TileEntity voidFrontEntity;
    public TileEntity voidEntity;
    private List<GameObject> voidTiles;
    private List<Vector2> possiblePositionsOdd;
    private List<Vector2> possiblePositionsEven;

    private void Start()
    {
        possiblePositionsOdd = new List<Vector2>
        {
            new Vector2(-1,0),  // Left
            new Vector2(1,0),   // Right
            new Vector2(0,-1),  // Top Left
            new Vector2(0,1),   // Bottom Left
            new Vector2(1,-1),  // Top Right
            new Vector2(1,1),   // Bottom Right
        };

        possiblePositionsEven = new List<Vector2>
        {
            new Vector2(-1,0),  // Left
            new Vector2(1,0),   // Right
            new Vector2(-1,-1),  // Top Left
            new Vector2(-1,1),   // Bottom Left
            new Vector2(0,-1),  // Top Right
            new Vector2(0,1),   // Bottom Right
        };
    }

    public void RegisterVoidFrontTile(GameObject voidTile, bool checkSurrounds)
    {
        if (voidTiles == null)
        {
            voidTiles = new List<GameObject>();
        }
        voidTiles.Add(voidTile);

        if (checkSurrounds)
        {
            // Look at surrounding tiles to check for void spaces
            var boardPosition = voidTile.GetComponent<TileInteractManager>().boardPosition;
            if (checkIfVoidFrontSurrounded(boardPosition))
            {
                voidTile.GetComponent<TileEntityContainer>().AddEntity(voidEntity);
                DeregisterVoidFrontTile(voidTile);
            }
            var possiblePositions = (boardPosition.y % 2 == 0) ? possiblePositionsEven : possiblePositionsOdd;
            possiblePositions.ForEach(relativePosition =>
            {
                var position = boardPosition + relativePosition;
                // Debug.Log("Check Position (" + position.x + "," + position.y + ")");
                if (checkIfVoidFrontSurrounded(position))
                {
                    var surroundedTile = boardManager.GetTile(position);
                    surroundedTile.GetComponent<TileEntityContainer>().AddEntity(voidEntity);
                    DeregisterVoidFrontTile(surroundedTile);
                }
            });
        }
    }

    public void DeregisterVoidFrontTile(GameObject voidTile)
    {
        voidTiles.Remove(voidTile);
    }

    public void SpreadVoid()
    {
        StartCoroutine(expandVoidAsync());
    }

    private IEnumerator expandVoidAsync()
    {
        expandRandomVoidTile();
        yield return null;
        expandRandomVoidTile();
        yield return null;
        expandRandomVoidTile();
    }

    private void expandRandomVoidTile()
    {
        int voidTileIndex = Random.Range(0, voidTiles.Count);
        var expandTile = voidTiles[voidTileIndex];
        Vector2 tilePosition = expandTile.GetComponent<TileInteractManager>().boardPosition;
        List<Vector2> validPositions = getValidVoidFrontExpansionPoints(tilePosition);
        int tileModIndex = Random.Range(0, validPositions.Count);
        Vector2 tileMod = validPositions[tileModIndex];
        Vector2 newPosition = tilePosition + tileMod;
        var tileToMod = boardManager.GetTile(newPosition);
        tileToMod.GetComponent<TileEntityContainer>().AddEntity(voidFrontEntity);
        RegisterVoidFrontTile(tileToMod, true);
        Debug.Log("Expanded Tile (" + tilePosition.x + "," + tilePosition.y + ") to (" + newPosition.x + "," + newPosition.y + ")");
    }

    private bool checkIfVoidFrontSurrounded(Vector2 tilePosition)
    {
        List<Vector2> validPositions = getValidVoidFrontExpansionPoints(tilePosition);
        return validPositions.Count == 0;
    }

    private List<Vector2> getValidVoidFrontExpansionPoints(Vector2 position)
    {
        var validPoints = new List<Vector2>();
        var possiblePositions = (position.y % 2 == 0) ? possiblePositionsEven : possiblePositionsOdd;
        possiblePositions.ForEach(point =>
        {
            var tile = boardManager.GetTile(position + point);
            var type = tile.GetComponent<TileEntityContainer>().GetBaseTileEntity().type;
            if (type != TileTypes.Exit && type != TileTypes.EventFront && type != TileTypes.EventVoid)
            {
                validPoints.Add(point);
            }
        });
        return validPoints;
    }
}
