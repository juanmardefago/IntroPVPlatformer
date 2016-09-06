using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    public GameObject objectToInteract;
    private PlayerMovement movementScript;
    public bool interacting;
    [HideInInspector]
    public Animator anim;

	// Use this for initialization
	void Start () {
        interacting = false;
        movementScript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact") && CanInteract() && !interacting)
        {
            interacting = true;
            objectToInteract.GetComponent<ObjectInteractionScript>().Interact(this);
        } else if(Input.GetButtonDown("Interact") && interacting)
        {
            objectToInteract.GetComponent<ObjectInteractionScript>().DeInteract(this);
        }
	}

    private bool CanInteract()
    {
        return objectToInteract != null && !interacting;
    }

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
}
