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
        WeaponScript wScript = weapon.GetComponent<WeaponScript>();
        weaponText.text = wScript.weaponName + " Lv." + wScript.weaponLevel;
    }
}
