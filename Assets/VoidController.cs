using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    private List<GameObject> voidTiles;

    public void RegisterVoidTile(GameObject voidTile)
    {
        if (voidTiles == null)
        {
            voidTiles = new List<GameObject>();
        }
        voidTiles.Add(voidTile);
    }

    public void SpreadVoid()
    {
        // TODO Find a tile next to existing void tiles
    }
}
