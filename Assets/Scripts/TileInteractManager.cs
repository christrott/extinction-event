using UnityEngine;

public class TileInteractManager : MonoBehaviour
{
    public GameObject glowObject;
    public Vector2 boardPosition;

    private PlayerMoveAdvisor moveAdvisor;
    private PlayerMoveController moveController;
    private Color32 highlightNoAction = new Color32(192, 2, 156, 128); // #C0029C
    private Color32 canMoveColor = new Color32(156, 192, 2, 255); // #9CC002

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
            glowObject.GetComponent<SpriteRenderer>().color = canMoveColor;
        }
    }

    public void OnMouseExit()
    {
        glowObject.SetActive(false);
        glowObject.GetComponent<SpriteRenderer>().color = highlightNoAction;
    }

    public void OnMouseDown()
    {
        Debug.Log("Tile->OnMouseDown " + boardPosition);
        moveController.MovePlayer(boardPosition);
        glowObject.GetComponent<SpriteRenderer>().color = highlightNoAction;
    }
}
