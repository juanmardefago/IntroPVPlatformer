using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    public GameObject objectToInteract;
    private PlayerMovement movementScript;
    private bool interacting;
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
        if (Input.GetButtonDown("Interact") && CanInteract())
        {
            interacting = true;
            objectToInteract.GetComponent<ObjectInteractionScript>().Interact(this);
        }
	}

    private bool CanInteract()
    {
        return objectToInteract != null && !interacting;
    }

    public void SwapState(PlayerMovementState pms)
    {
        movementScript.SwapState(pms);
        interacting = false;
    }
}
