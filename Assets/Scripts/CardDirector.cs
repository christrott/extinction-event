using UnityEngine;

public class CardDirector : MonoBehaviour
{
    public GameObject cardPrefab;

    private GameObject cardInstance;

    // Show card at world position
    public void ShowCardAtWorldPosition(Vector3 worldPosition, string title, string content)
    {
        if (cardInstance == null)
        {
            cardInstance = Instantiate(cardPrefab, gameObject.transform);
        } else
        {
            cardInstance.SetActive(true);
        }
        var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        cardInstance.transform.position = screenPosition;
        cardInstance.GetComponent<Card>().SetCardContent(title, content);
        cardInstance.GetComponent<RectTransform>().pivot = getCardOffset(screenPosition, Camera.main);
    }

    // Show card at screen position
    public void ShowCardAtScreenPosition(Vector2 screenPosition, string title, string content)
    {
        if (cardInstance == null)
        {
            cardInstance = Instantiate(cardPrefab, gameObject.transform);
        }
        else
        {
            cardInstance.SetActive(true);
        }
        cardInstance.transform.position = screenPosition;
        cardInstance.GetComponent<Card>().SetCardContent(title, content);
        cardInstance.GetComponent<RectTransform>().pivot = getCardOffset(screenPosition, Camera.main);
    }

    public void HideCard()
    {
        cardInstance.GetComponent<Card>().SetCardContent("", "");
        cardInstance.SetActive(false);
    }

    private Vector2 getCardOffset(Vector2 screenPoint, Camera camera)
    {
        Vector2 screenDimensions = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        return new Vector2(Mathf.Round(screenPoint.x / screenDimensions.x), Mathf.Round(screenPoint.y / screenDimensions.y));
    }
}
