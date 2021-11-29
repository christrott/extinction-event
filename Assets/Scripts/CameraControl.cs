using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public GameObject player;

    /*public static bool INVERT_SCROLL = true;
    private static float MIN_ZOOM = 20.0f;
    private static float MAX_ZOOM = 5.0f;
    private float targetZoom = 5.0f;*/

    public float arrowMoveSpeed;
    public float scrollWheelSpeed;
    public float panSpeed;

    private Vector3 lastPos = Vector3.zero;
    private Vector2 mapDimensions;
    private Vector2 panningMove;

    private void Start()
    {
        lastPos = target.position;
        BoardGenerator boardGenerator = GameObject.Find("GenerationDirector").GetComponent<BoardGenerator>();
        mapDimensions = boardGenerator.boardDimensions;
    }

    private void UpdateTransform(Vector3 deltaPosition)
    {
        Vector3 newPosition = target.position + deltaPosition;
        if (mapDimensions.x > 0 && mapDimensions.y > 0)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, -mapDimensions.x, mapDimensions.x);
            newPosition.y = Mathf.Clamp(newPosition.y, -mapDimensions.y, mapDimensions.y);
        }
        target.position = newPosition;
    }

    private void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        updateArrowPanning();

        if (Input.GetMouseButton(2))
        {
            Vector3 currentMousePos = mouseWorldPosition;
            Vector3 mouseDelta = currentMousePos - lastPos;
            mouseDelta = new Vector3(mouseDelta.x, mouseDelta.y, mouseDelta.z);
            UpdateTransform(mouseDelta * panSpeed * Time.deltaTime);
            lastPos = currentMousePos;
        }
        else
        {
            lastPos = mouseWorldPosition;
        }
    }

    public void CentreCameraOnTile(Vector2 tilePosition)
    {
        BoardManager boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        var worldPosition = boardManager.GetTile(tilePosition).transform.position;
        UpdateTransform(-worldPosition);
    }

    private void updateArrowPanning()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            panningMove.x = 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            panningMove.x = -1;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            panningMove.y = -1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            panningMove.y = 1;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            panningMove.x = 0;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            panningMove.x = 0;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            panningMove.y = 0;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            panningMove.y = 0;
        }

        UpdateTransform(panningMove * arrowMoveSpeed * Time.deltaTime);
    }
}
