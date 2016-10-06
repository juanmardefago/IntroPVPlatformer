﻿using UnityEngine;
using System.Collections;

public class PlayerNPCInteraction : MonoBehaviour
{

    public GameObject objectToInteract;
    private PlayerMovement movementScript;
    public bool interacting;

    // Use this for initialization
    void Start()
    {
        interacting = false;
        movementScript = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si apreta el boton para usar y puede usar, pero además no esta interactuando
        if (Input.GetButtonDown("Use") && CanInteract())
        {
            // avisa que empezo a interactuar y llama al metodo de interaccion del script interactivo del objeto.
            interacting = true;
            objectToInteract.GetComponent<UsableObjectScript>().Interact(this);
        }
        else if (Input.GetButtonDown("Use") && interacting)
        {
            // Si estaba interactuando entonces avisa al script de desinteraccion del objeto.
            interacting = false;
            objectToInteract.GetComponent<UsableObjectScript>().DeInteract(this);
        }
    }

    // puede interactuar si esta sobre un objeto para interactuar y si no esta interactuando
    private bool CanInteract()
    {
        return objectToInteract != null && !interacting;
    }

    // estos mensajes estan para simplificar las cosas.
    public void SwapState(PlayerMovementState pms)
    {
        movementScript.SwapState(pms);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "NPC")
        {
            objectToInteract = other.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "NPC")
        {
            if (interacting)
            {
                objectToInteract.GetComponent<UsableObjectScript>().DeInteract(this);
                interacting = false;
            }
            objectToInteract = null;
        }
    }
}
