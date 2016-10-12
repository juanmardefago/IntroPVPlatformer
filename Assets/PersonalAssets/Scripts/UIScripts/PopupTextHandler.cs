using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupTextHandler : MonoBehaviour
{

    public GameObject popupPrefab;
    public Animator prefabAnim;
    public GameObject canvas;
    private float clipLength;

    public void Start()
    {
        clipLength = 1.02f;
    }

    public void Show(string text)
    {
        GameObject popup = Instantiate(popupPrefab);
        popup.GetComponentInChildren<Text>().text = text;
        popup.transform.SetParent(canvas.transform, false);
        Destroy(popup, clipLength);
    }

    public void Show(string text, Color color)
    {
        GameObject popup = Instantiate(popupPrefab);
        Text popupText = popup.GetComponentInChildren<Text>();
        popupText.color = color;
        popupText.text = text;
        popup.transform.SetParent(canvas.transform, false);
        Destroy(popup, clipLength);
    }
}
