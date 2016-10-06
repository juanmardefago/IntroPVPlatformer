using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupTextHandler : MonoBehaviour
{

    public GameObject popupPrefab;
    public Animator prefabAnim;
    private GameObject canvas;
    private float clipLength;

    public void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void TakeDamage(int damage)
    {
        GameObject popup = Instantiate(popupPrefab);
        if (clipLength == 0f)
        {
            AnimatorClipInfo[] clipInfo = popup.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0);
            clipLength = clipInfo[0].clip.length;
        }
        popup.GetComponentInChildren<Text>().text = damage.ToString();
        popup.transform.SetParent(canvas.transform, false);
        Vector2 screenLocation = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f)));
        popup.transform.position = screenLocation;
        Destroy(popup, clipLength);
    }
}
