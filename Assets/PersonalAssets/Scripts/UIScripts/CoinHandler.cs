﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinHandler : MonoBehaviour {

    public GameObject player;
    private Inventory playerInventory;
    private Text textUI;

	// Use this for initialization
	void Start () {
        if(player == null) {
            player = GameObject.FindWithTag("Player");
        }
        playerInventory = player.GetComponent<Inventory>();
        textUI = gameObject.GetComponent<Text>();
        textUI.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
        HandleCoinText();
	}

    private void HandleCoinText()
    {
        int currentCoins = playerInventory.Coins;
        textUI.text = currentCoins.ToString();
    }
}
