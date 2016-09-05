using UnityEngine;
using System.Collections;

public class StateDetection : MonoBehaviour
{

    private PlayerMovement movementScript;
    private PlayerInteraction interactionScript;

    // Use this for initialization
    void Start()
    {
        movementScript = GetComponentInParent<PlayerMovement>();
        interactionScript = GetComponentInParent<PlayerInteraction>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ResetAnims();
        if (other.transform.parent.tag == "Ground") {
            ResetAnims();
            movementScript.SwapState(GroundedState.GetInstance());
        } else if(other.transform.parent.tag == "Rope") {
            interactionScript.objectToInteract = other.gameObject;
        } else if (other.transform.parent.tag == "Water") {
            ResetAnims();
            movementScript.SwapState(SwimmingState.GetInstance());
            movementScript.anim.SetBool("isSwimming", true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Nada por ahora.
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.parent.tag == "Water") {
            movementScript.SwapState(MidAirState.GetInstance());
        } else if (other.transform.parent.tag == "Rope")
        {
            //ResetAnims();
            //movementScript.SetColliderStatus(true);
            //movementScript.SwapState(EmptyState.GetInstance());
        }
    }

    void ResetAnims()
    {
        movementScript.anim.SetBool("isClimbing", false);
        movementScript.anim.SetBool("isJumping", false);
        movementScript.anim.SetBool("isSwimming", false);
    }
}
