using UnityEngine;
using System.Collections;
using System;

public class PlayerMenus : MonoBehaviour {

    public GameObject mainPanel;
    private bool isMenuOpen = false;
    private GameObject player;

    // Rect transforms for content
    public RectTransform inventoryContent;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("PlayerMenu") && !isMenuOpen)
        {
            OpenPlayerMenu();
        } else if (Input.GetButtonDown("PlayerMenu") && isMenuOpen)
        {
            ClosePlayerMenu();
        }
	}

    public void ClosePlayerMenu()
    {
        isMenuOpen = false;
        mainPanel.SetActive(false);
    }

    public void OpenPlayerMenu()
    {
        mainPanel.SetActive(true);
        isMenuOpen = true;
    }

    public void OpenInventory()
    {

    }

    
}
