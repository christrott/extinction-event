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

    // Start is called before the first frame update
    void Awake()
    {
        board = GameObject.Find("Board");
        voidDirector = GameObject.Find("VoidDirector");
        GenerateBoard(17, 17);
    }

    public void GenerateBoard(int xSize, int ySize)
    {
        var tileIndex = 0;
        board.GetComponent<BoardManager>().tileSet = new Dictionary<string, GameObject>();
        var tileComponents = GetComponent<TileComponents>();
        var playerPos = GeneratePlayerPosition(xSize, ySize);
        var voidPos = GenerateVoidPosition(xSize, ySize, playerPos);
        board.GetComponent<PlayerMoveController>().playerPos = playerPos;
        boardDimensions = new Vector2(xSize * xSpacing, ySize * ySpacing);
        Vector2 boardOffset = boardDimensions / 2;

        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                var newTile = Instantiate(tilePrefab, board.transform);
                newTile.GetComponent<TileInteractManager>().boardPosition = new Vector2(x, y);
                string index = x + "," + y;
                bool isOddRow = y % 2 == 1;
                float xOffset = isOddRow ? boardOffset.x - alternateOffset : boardOffset.x;
                newTile.transform.position = new Vector2(
                    x * xSpacing - xOffset,
                    y * ySpacing - boardOffset.y
                );
                if (playerPos.x == x && playerPos.y == y)
                {
                    newTile.GetComponent<TileEntityContainer>().AddEntity(startingPlayerEntity);
                }
                else if (voidPos.x == x && voidPos.y == y)
                {
                    newTile.GetComponent<TileEntityContainer>().AddEntity(voidFrontEntity);
                    voidDirector.GetComponent<VoidController>().RegisterVoidFrontTile(newTile, false);
                }
                else
                {
                    if (y == 0 || y == ySize - 1  || x == 0 || x == xSize - 1)
                    {
                        Debug.Log("Place Exit tile at (" + x + "," + y + ")");
                        var component = tileExitEntity;
                        newTile.GetComponent<TileEntityContainer>().AddEntity(component);
                    } else
                    {
                        tileIndex = (tileIndex + 1) % tileComponents.componentList.Count;
                        var component = tileComponents.componentList[tileIndex];
                        newTile.GetComponent<TileEntityContainer>().AddEntity(component);
                    }
                }
                board.GetComponent<BoardManager>().tileSet[index] = newTile;
            }
        }
        GameObject.Find("Main Camera").GetComponent<CameraControl>().CentreCameraOnTile(playerPos);
    }

    private Vector2 GeneratePlayerPosition(int xSize, int ySize)
    {
        Vector2 xBand = new Vector2(Mathf.FloorToInt(xSize * 0.25f), Mathf.FloorToInt(xSize * 0.75f));
        Vector2 yBand = new Vector2(Mathf.FloorToInt(ySize * 0.25f), Mathf.FloorToInt(ySize * 0.75f));
        return new Vector2(xBand.x, yBand.x);
    }

    private Vector2 GenerateVoidPosition(int xSize, int ySize, Vector2 playerPosition)
    {
        Vector2 xBand = new Vector2(Mathf.FloorToInt(xSize * 0.25f), Mathf.FloorToInt(xSize * 0.75f));
        Vector2 yBand = new Vector2(Mathf.FloorToInt(ySize * 0.25f), Mathf.FloorToInt(ySize * 0.75f));
        return new Vector2(xBand.y, yBand.y);
    }
}
