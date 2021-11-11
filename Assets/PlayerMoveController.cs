using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public Vector2 playerPos;

    private BoardManager boardManager;

    // Start is called before the first frame update
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
        playerPos = new Vector2(0,0);
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
            playerPos = newPosition;
        }
    }
}
