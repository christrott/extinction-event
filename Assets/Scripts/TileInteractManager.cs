using UnityEngine;

public class TileInteractManager : MonoBehaviour
{
    public GameObject glowObject;
    public Vector2 boardPosition;

    private PlayerMoveAdvisor moveAdvisor;
    private PlayerMoveController moveController;

    public void Start()
    {
        moveAdvisor = gameObject.transform.parent.GetComponent<PlayerMoveAdvisor>();
        moveController = gameObject.transform.parent.GetComponent<PlayerMoveController>();
    }

    public void OnMouseOver()
    {
        glowObject.SetActive(true);
        if (moveAdvisor.IsValidMovePosition(boardPosition))
        {
            glowObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void OnMouseExit()
    {
        glowObject.SetActive(false);
        glowObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnMouseDown()
    {
        Debug.Log("Tile->OnMouseDown " + boardPosition);
        moveController.MovePlayer(boardPosition);
        glowObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
