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


    public void TakeDamage(int damage)
    {
        GameObject popup = Instantiate(popupPrefab);
        popup.GetComponentInChildren<Text>().text = damage.ToString();
        popup.transform.SetParent(canvas.transform, false);
        Destroy(popup, clipLength);
    }
}
