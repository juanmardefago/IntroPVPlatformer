using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponShopRowHandler : MonoBehaviour {

    public Image weaponImage;
    public Text weaponText;

    private GameObject weapon;

	public void RefreshWeapon(GameObject weapon)
    {
        weaponImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        weaponText.text = weapon.GetComponent<WeaponScript>().weaponName;
    }
}
