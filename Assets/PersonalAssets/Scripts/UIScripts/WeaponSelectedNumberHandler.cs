using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponSelectedNumberHandler : MonoBehaviour {

    public Inventory playerInventory;
    private Text[] numbers;
    public Color colorSelected;
    public Color colorNotSelected;

	// Use this for initialization
	void Start () {
	    if(playerInventory == null)
        {
            playerInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }
        numbers = GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleNumber(0);
        HandleNumber(1);
	}

    private void HandleNumber(int number)
    {
        if(playerInventory.GetCurrentWeaponSlot() == number)
        {
            numbers[number].color = colorSelected;
        } else
        {
            numbers[number].color = colorNotSelected;
        }
    }
}
