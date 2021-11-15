using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
    public GameObject loadingProgressObject;

    public void SetProgress(float progress)
    {
        var loadingBarRect = transform.GetComponent<RectTransform>();
        var progressRect = loadingProgressObject.GetComponent<RectTransform>();
        progressRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, loadingBarRect.rect.width * progress);
    }
}
