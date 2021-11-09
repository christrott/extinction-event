using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public GameObject player;

    public static bool INVERT_SCROLL = true;
    private static float MAX_ZOOM = 38.0f;

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
        if (newPosition.y >= MAX_ZOOM)
        {
            newPosition.y = MAX_ZOOM;
        }
        newPosition.x = Mathf.Clamp(newPosition.x, -mapDimensions.x, mapDimensions.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -mapDimensions.y, mapDimensions.y);

        target.position = newPosition;
    }

    private void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // TODO Add scroll zoom
        /*if (Input.mouseScrollDelta.y != 0.0f)
        {
            Debug.Log(Input.mouseScrollDelta.y);
            currentPos.z += Input.mouseScrollDelta.y * scrollWheelSpeed * Time.deltaTime;
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
}
