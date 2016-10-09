using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponImageHandler : MonoBehaviour {

    public GameObject player;
    private Image img;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        img = gameObject.GetComponent<Image>();
        img.color = new Color32(255,255,255,255);
        SpriteRenderer[] comps = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer comp in comps)
        {
            if (comp.gameObject.GetInstanceID() != player.GetInstanceID())
            {
                img.sprite = comp.sprite;
            }
        }
    }

}
