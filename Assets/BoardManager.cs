using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject GetTile(Vector2 position)
    {
        return GameObject.Find(position.x + "," + position.y);
    }
}
