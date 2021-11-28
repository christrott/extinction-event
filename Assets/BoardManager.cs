using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Dictionary<string, GameObject> tileSet;

    public GameObject GetTile(Vector2 position)
    {
        Debug.Log("GetTile(" + position.x + "," + position.y + ")");
        return tileSet[position.x + "," + position.y];
    }
}
