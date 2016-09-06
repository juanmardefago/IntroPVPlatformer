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
        // Cuando entro al trigger solo detecto cambios de estado al entrar a un trigger si no esta interactuando con algo en particular
        // Por ejemplo, si esta colgado de la soga, no quiero que se cambie el estado hasta que no termine de interactuar con la soga
        if (!interactionScript.interacting)
        {
            DetectState(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!interactionScript.interacting)
        {
            DetectState(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Cuando se sale de un trigger deberia cambiar el estado dependiendo de si esta interactuando o no
        // Si no esta interactuando, los cambios de estado que me interezan son los de exit que no tengan
        // interaccion, como por ejemplo, nadar, o saltar
        if (!interactionScript.interacting)
        {
            ExitNonInteracting(other);
        }
        // en cambio, si esta interactuando, quiero que se fije si salio de la soga por ejemplo.
        else
        {
            ExitInteracting(other);
        }
    }

    void DetectState(Collider2D other)
    {
        if (other.transform.parent.tag == "Ground")
        {
            ResetAnims();
            movementScript.SwapState(GroundedState.GetInstance());
        }
        else if (other.transform.parent.tag == "Rope")
        {
            interactionScript.objectToInteract = other.gameObject;
        }
        else if (other.transform.parent.tag == "Water")
        {
            ResetAnims();
            movementScript.SwapState(SwimmingState.GetInstance());
            movementScript.anim.SetBool("isSwimming", true);
        }
    }

    void ExitNonInteracting(Collider2D other)
    {
        if (other.transform.parent.tag == "Water")
        {
            movementScript.SwapState(MidAirState.GetInstance());
        } else if (other.transform.parent.tag == "Rope")
        {
            interactionScript.objectToInteract = null;
        }
    }

    void ExitInteracting(Collider2D other)
    {
        if (other.transform.parent.tag == "Rope")
        {
            interactionScript.SetKinematic(false);
            interactionScript.interacting = false;
            interactionScript.objectToInteract = null;
            ResetAnims();
            movementScript.SetColliderStatus(true);
            movementScript.SwapState(MidAirState.GetInstance());
        }
    }

    void ResetAnims()
    {
        interactionScript.ResetAnims();
    }
}
