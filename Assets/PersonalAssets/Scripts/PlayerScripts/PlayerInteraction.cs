using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour {

    // Este es el script de interacción del player con cosas como sogas y teleporters, el de NPCs se maneja por separado

    public GameObject objectToInteract;
    private PlayerMovement movementScript;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Stack<Vector3> goBackPositionStack;
    public Inventory inventory;
    public ObjectInteractionScript interactingScript;
    private UIFeedback feedbackScript;

	// Use this for initialization
	void Start () {
        inventory = GetComponent<Inventory>();
        movementScript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        goBackPositionStack = new Stack<Vector3>();
        feedbackScript = GetComponent<UIFeedback>();
	}
	
	// Update is called once per frame
	void Update () {
        // Si apreta el boton para interactuar y puede interactuar, pero además no esta interactuando
        if (Input.GetButtonDown("Interact") && CanInteract())
        {
            // avisa que empezo a interactuar y llama al metodo de interaccion del script interactivo del objeto.
            interactingScript = objectToInteract.GetComponent<ObjectInteractionScript>();
            interactingScript.Interact(this, movementScript);
            feedbackScript.ResetImage();
        } else if(Input.GetButtonDown("Jump") && Interacting())
        {
            // Si estaba interactuando entonces avisa al script de desinteraccion del objeto.
            interactingScript.DeInteract(this, movementScript);
            interactingScript = null;
        }
	}

    // puede interactuar si esta sobre un objeto para interactuar y si no esta interactuando
    private bool CanInteract()
    {
        return objectToInteract != null && !Interacting();
    }

    // estos mensajes estan para simplificar las cosas.
    public void ResetAnims()
    {
        movementScript.anim.SetBool("isClimbing", false);
        movementScript.anim.SetBool("isJumping", false);
        movementScript.anim.SetBool("isSwimming", false);
    }

    public void SetWeaponActive(bool cond)
    {
        WeaponScript weapon = inventory.currentWeapon;
        if (weapon != null)
        {
            weapon.gameObject.SetActive(cond);
        }
    }

    public bool Interacting()
    {
        return interactingScript != null;
    }
}
