using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAdvisor : MonoBehaviour
{
    public GameObject GetTileAtPlayerPosition()
    {
        Vector2 playerPos = GetComponent<PlayerMoveController>().playerPos;
        BoardManager boardManager = GetComponent<BoardManager>();
        return boardManager.GetTile(playerPos);
    }

    public bool IsValidMovePosition(Vector2 movePos)
    {
        Vector2 playerPos = GetComponent<PlayerMoveController>().playerPos;

        if (playerPos == movePos)
        {
            return false;
        }

        bool isSameRow = playerPos.y == movePos.y;
        if (isSameRow)
        {
            if (movePos.x == playerPos.x + 1 || movePos.x == playerPos.x - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool isOddRow = playerPos.y % 2 == 1;
        var adjustLeftX = isOddRow ? 1 : 1;
        var adjustRightX = isOddRow ? 1 : 0;

        // Check x
        if (movePos.x > playerPos.x + adjustRightX || movePos.x < playerPos.x - adjustLeftX)
        {
            return false;
        }
        // Check y
        if (movePos.y > playerPos.y + 1 || movePos.y < playerPos.y - 1)
        {
            return false;
        }

        return true;
    }
}
