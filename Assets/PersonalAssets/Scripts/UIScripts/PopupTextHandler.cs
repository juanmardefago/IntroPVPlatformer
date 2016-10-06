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
        //Vector2 screenLocation = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f)));
        //popup.transform.position = screenLocation;
        Destroy(popup, clipLength);
    }
}
