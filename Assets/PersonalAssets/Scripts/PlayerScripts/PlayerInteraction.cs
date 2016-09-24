using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour {

    public GameObject objectToInteract;
    private PlayerMovement movementScript;
    public bool interacting;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Stack<Vector3> goBackPositionStack;
    public GameObject weapon;
    private StateDetection stateDetect;

	// Use this for initialization
	void Start () {
        interacting = false;
        movementScript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        goBackPositionStack = new Stack<Vector3>();
        stateDetect = GetComponentInChildren<StateDetection>();
	}
	
	// Update is called once per frame
	void Update () {
        // Si apreta el boton para interactuar y puede interactuar, pero además no esta interactuando
        if (Input.GetButtonDown("Interact") && CanInteract())
        {
            // avisa que empezo a interactuar y llama al metodo de interaccion del script interactivo del objeto.
            interacting = true;
            objectToInteract.GetComponent<ObjectInteractionScript>().Interact(this);
        } else if(Input.GetButtonDown("Interact") && interacting)
        {
            // Si estaba interactuando entonces avisa al script de desinteraccion del objeto.
            objectToInteract.GetComponent<ObjectInteractionScript>().DeInteract(this);
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

    public void SetKinematic(bool status)
    {
        movementScript.rBody.isKinematic = status;
    }

    public void ResetAnims()
    {
        movementScript.anim.SetBool("isClimbing", false);
        movementScript.anim.SetBool("isJumping", false);
        movementScript.anim.SetBool("isSwimming", false);
    }

    public void SetWeaponActive(bool cond)
    {
        if (weapon != null)
        {
            weapon.SetActive(cond);
        }
    }

    public void OnDeInteract()
    {
        stateDetect.OnDeInteract();
    }
}
