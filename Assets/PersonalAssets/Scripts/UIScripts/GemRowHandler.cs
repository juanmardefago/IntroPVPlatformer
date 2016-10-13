using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GemRowHandler : MonoBehaviour {

    public Image gemImage;
    public Text gemText;
    public Button gemInfoButton;
    public Button gemEquipButton;

    public void RefreshGem(GameObject gem)
    {
        gemText.text = gem.GetComponent<UpgradeScript>().upgradeName;
        gemImage.sprite = gem.GetComponent<UpgradeScript>().upgradeSprite;
    }
}
