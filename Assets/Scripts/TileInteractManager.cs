using UnityEngine;

public class TileInteractManager : MonoBehaviour
{
    public GameObject glowObject;
    public Vector2 boardPosition;
    public CardDirector cardDirector;
    public EntityCostDirector entityCostManager;
    public EntityEnergyDirector entityGainManager;

    private PlayerMoveAdvisor moveAdvisor;
    private PlayerMoveController moveController;
    private Color32 highlightNoAction = new Color32(192, 2, 156, 128); // #C0029C
    private Color32 canMoveColor = new Color32(156, 192, 2, 255); // #9CC002
    private TileEntityContainer entityContainer;

    public void Start()
    {
        entityContainer = GetComponent<TileEntityContainer>();
        cardDirector = GameObject.Find("CardDirector").GetComponent<CardDirector>();
        var board = GameObject.Find("Board");
        entityCostManager = board.GetComponent<EntityCostDirector>();
        entityGainManager = board.GetComponent<EntityEnergyDirector>();
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
        var entity = entityContainer.GetBaseTileEntity();
        string description = "Type: " + entity.type + "\nCost: " + entityCostManager.GetCostForTile(entity.type) + "\nGain: " + entityGainManager.GetEnergyGainForTile(entity.type);
        cardDirector.ShowCardAtWorldPosition(transform.position, entity.name, description);
    }

    public void OnMouseExit()
    {
        glowObject.SetActive(false);
        glowObject.GetComponent<SpriteRenderer>().color = highlightNoAction;
        cardDirector.HideCard();
    }

    public void OnMouseDown()
    {
        Debug.Log("Tile->OnMouseDown " + boardPosition);
        moveController.MovePlayer(boardPosition);
        glowObject.GetComponent<SpriteRenderer>().color = highlightNoAction;
    }
}
