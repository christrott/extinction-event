using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Text titleText;
    public Text contentText;

    public void SetCardContent(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
    }
}
