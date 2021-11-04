using UnityEngine;

public class TileInteractManager : MonoBehaviour
{
    public void OnMouseOver()
    {
        Debug.Log("Tile->OnMouseOver");
    }

    public void OnMouseExit()
    {
        Debug.Log("Tile->OnMouseExit");
    }

    public void OnMouseDown()
    {
        Debug.Log("Tile->OnMouseDown");
    }
}
