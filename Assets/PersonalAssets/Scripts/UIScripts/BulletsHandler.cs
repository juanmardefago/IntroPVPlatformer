using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletsHandler : MonoBehaviour {

    public GameObject player;
    private WeaponScript playerWeapon;
    private Text textUI;

    // Use this for initialization
    void Start()
    {
        playerWeapon = player.GetComponentInChildren<WeaponScript>();
        textUI = gameObject.GetComponent<Text>();
        textUI.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        HandleBulletAmount();
    }

    private void HandleBulletAmount()
    {
        int currentBullets = playerWeapon.Bullets;
        int maxBullets = playerWeapon.maxBullets;
        textUI.text = currentBullets + " / " + maxBullets;
    }
}
