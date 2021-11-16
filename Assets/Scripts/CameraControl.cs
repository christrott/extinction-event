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

        // TODO Add scroll zoom
        /*if (Input.mouseScrollDelta.y != 0.0f)
        {
            Debug.Log(Input.mouseScrollDelta.y);
            Camera.main.orthographicSize *= scrollWheelSpeed * Input.mouseScrollDelta.y;
            targetZoom -= Input.mouseScrollDelta.y * scrollWheelSpeed;
            targetZoom = Mathf.Clamp(targetZoom, MAX_ZOOM, MIN_ZOOM);
            float newSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetZoom, scrollWheelSpeed * Time.deltaTime);
            Camera.main.orthographicSize = newSize;
        }*/

        // TODO Add panning with arrow keys

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
}
