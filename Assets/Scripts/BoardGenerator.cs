using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2 boardDimensions;

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
        boardDimensions = new Vector2(xSize * xSpacing, ySize * ySpacing);
        Vector2 boardOffset = boardDimensions / 2;

        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                var newTile = Instantiate(tilePrefab, board.transform);
                bool isOddRow = y % 2 == 1;
                float xOffset = isOddRow ? boardOffset.x - alternateOffset : boardOffset.x;
                newTile.transform.position = new Vector2(
                    x * xSpacing - xOffset,
                    y * ySpacing - boardOffset.y
                );
            }
        }
    }

}
