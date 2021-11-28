using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2 boardDimensions;
    public TileEntity startingPlayerEntity;
    public TileEntity tileExitEntity;
    public TileEntity voidFrontEntity;

    private const float xSpacing = 1.0f;
    private const float ySpacing = 0.8f;
    private const float alternateOffset = xSpacing / 2.0f;
    private GameObject board;
    private GameObject voidDirector;
    private TileComponents tileComponents;

    // Start is called before the first frame update
    void Awake()
    {
        tileComponents = GetComponent<TileComponents>();
        board = GameObject.Find("Board");
        voidDirector = GameObject.Find("VoidDirector");
        GenerateBoard(17, 17);
    }

    public void GenerateBoard(int xSize, int ySize)
    {
        board.GetComponent<BoardManager>().tileSet = new Dictionary<string, GameObject>();
        var playerPos = GeneratePlayerPosition(xSize, ySize);
        var voidPos = GenerateVoidPosition(xSize, ySize, playerPos);
        board.GetComponent<PlayerMoveController>().playerPos = playerPos;
        boardDimensions = new Vector2(xSize * xSpacing, ySize * ySpacing);
        Vector2 boardOffset = boardDimensions / 2;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                var newTile = Instantiate(tilePrefab, board.transform);
                string index = x + "," + y;
                newTile.name = index;
                newTile.GetComponent<TileInteractManager>().boardPosition = new Vector2(x, y);                
                bool isOddRow = y % 2 == 1;
                float xOffset = isOddRow ? boardOffset.x - alternateOffset : boardOffset.x;
                newTile.transform.position = new Vector2(
                    x * xSpacing - xOffset,
                    y * ySpacing - boardOffset.y
                );
                if (y == 0 || y == ySize - 1 || x == 0 || x == xSize - 1)
                {
                    Debug.Log("Place Exit tile at (" + x + "," + y + ")");
                    var component = tileExitEntity;
                    newTile.GetComponent<TileEntityContainer>().AddEntity(component);
                }
                else
                {
                    var position = new Vector2(x, y);
                    if (position == playerPos)
                    {
                        newTile.GetComponent<TileEntityContainer>().AddEntity(startingPlayerEntity);
                    }
                    else if (position == voidPos)
                    {
                        newTile.GetComponent<TileEntityContainer>().AddEntity(voidFrontEntity);
                        voidDirector.GetComponent<VoidController>().RegisterVoidFrontTile(newTile, false);
                    }
                    else
                    {
                        int tier = getTierForPosition(position, new Vector2(xSize, ySize));
                        TileEntity newEntity = getEntityForTier(tier, position);
                        newTile.GetComponent<TileEntityContainer>().AddEntity(newEntity);
                    }
                }
                board.GetComponent<BoardManager>().tileSet[index] = newTile;
            }
        }
        GameObject.Find("Main Camera").GetComponent<CameraControl>().CentreCameraOnTile(playerPos);
    }

    private Vector2 GeneratePlayerPosition(int xSize, int ySize)
    {
        Vector2 xBand = new Vector2(Mathf.FloorToInt(xSize * 0.4f), Mathf.FloorToInt(xSize * 0.6f));
        Vector2 yBand = new Vector2(Mathf.FloorToInt(ySize * 0.4f), Mathf.FloorToInt(ySize * 0.6f));
        int randomX = Mathf.FloorToInt(Random.Range(xBand.x, xBand.y));
        int randomY = Mathf.FloorToInt(Random.Range(yBand.x, yBand.y));
        return new Vector2(randomX, randomY);
    }

    private Vector2 GenerateVoidPosition(int xSize, int ySize, Vector2 playerPosition)
    {
        Vector2 xBand = new Vector2(Mathf.FloorToInt(xSize * 0.25f), Mathf.FloorToInt(xSize * 0.75f));
        Vector2 yBand = new Vector2(Mathf.FloorToInt(ySize * 0.25f), Mathf.FloorToInt(ySize * 0.75f));
        return new Vector2(xBand.y, yBand.y);
    }

    private int getTierForPosition(Vector2 position, Vector2 mapSize)
    {
        var halfMapSize = mapSize / 2;
        int xDistanceFromCentre = Mathf.FloorToInt(Mathf.Abs(halfMapSize.x - position.x));
        int yDistanceFromCentre = Mathf.FloorToInt(Mathf.Abs(halfMapSize.y - position.y));

        int xTier = Mathf.FloorToInt(xDistanceFromCentre / (halfMapSize.x / 3)) + 1;
        int yTier = Mathf.FloorToInt(yDistanceFromCentre / (halfMapSize.y / 3)) + 1;
        return xTier > yTier ? xTier : yTier;
    }

    private TileEntity getEntityForTier(int tier, Vector2 position)
    {
        var grassEntity = tileComponents.componentList[0];
        var bushEntity = tileComponents.componentList[1];
        var smallEntity = tileComponents.componentList[2];
        var mediumEntity = tileComponents.componentList[3];
        var largeEntity = tileComponents.componentList[4];

        int random = Random.Range(0, 100);
        if (random <= 80)
        {
            // Place tier appropriate entity
            switch(tier)
            {
                case 1:
                    return smallEntity;
                case 2:
                    return mediumEntity;
                case 3:
                    return largeEntity;
            }
        }
        else if (random <= 90)
        {
            // Plants
            if (random > 85)
            {
                return bushEntity;
            }
            else
            {
                return grassEntity;
            }
        }
        else
        {
            // Other tier animal
            switch (tier)
            {
                case 1:
                    return mediumEntity;
                case 2:
                    return smallEntity;
                case 3:
                    return mediumEntity;
            }
        }
        return grassEntity;
    }
}
