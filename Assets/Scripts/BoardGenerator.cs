using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2 boardDimensions;
    public TileEntity startingPlayerEntity;

    private const float xSpacing = 1.0f;
    private const float ySpacing = 0.8f;
    private const float alternateOffset = xSpacing / 2.0f;
    private GameObject board;

    // Start is called before the first frame update
    void Awake()
    {
        board = GameObject.Find("Board");
        GenerateBoard(16, 16);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateBoard(int xSize, int ySize)
    {
        board.GetComponent<BoardManager>().tileSet = new Dictionary<string, GameObject>();
        var tileComponents = GetComponent<TileComponents>();
        var playerPos = GeneratePlayerPosition(xSize, ySize);
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
                } else
                {
                    var component = tileComponents.componentList[0];
                    newTile.GetComponent<TileEntityContainer>().AddEntity(component);
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
        return new Vector2(xBand.x, yBand.y);
    }
}
