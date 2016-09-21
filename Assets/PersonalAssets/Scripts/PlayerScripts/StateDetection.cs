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
        // Si nos mantenemos en un collider y no se esta interactuando, que detecte el estado correspondiente
        // Esto lo hacemos para que si, por ejemplo, esta en el agua, y se pone a interactuar con la soga, y luego
        // deja de interactuar, pueda seguir nadando, dado que nunca salio del collider de agua, y no va a volver
        // a triggerear un OnTriggerEnter, pero si un OnTriggerStay.
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
        // Si el collider tiene tag Ground, reseteamos las animaciones y cambiamos el estado a Grounded.
        if (IsTagged("Ground", other))
        {
            ResetAnims();
            movementScript.SwapState(GroundedState.GetInstance());
        }
        // Si el collider tiene tag Rope (Soga), le avisamos al script de interaccion que tiene un objeto
        // para interactuar, y no hacemos nada más.
        else if (IsTagged("Rope", other))
        {
            interactionScript.objectToInteract = other.gameObject;
        }
        // Si el collider tiene tag Water, reseteamos las animaciones, cambiamos el estado a Swimming y
        // le avisamos al animatorController que se esta nadando.
        else if (IsTagged("Water", other))
        {
            ResetAnims();
            movementScript.SwapState(SwimmingState.GetInstance());
            movementScript.anim.SetBool("isSwimming", true);
        }
        else if (IsTagged("TeleportInteracter", other)) {
            interactionScript.objectToInteract = other.gameObject;
        }
    }

    void ExitNonInteracting(Collider2D other)
    {
        // Si sale de un collider de tag Water mientras no interactua cambiamos el estado a midAir para que 
        // pueda saltar y salir del agua.
        if (IsTagged("Water", other))
        {
            movementScript.SwapState(MidAirState.GetInstance());
        }
        // Si no estamos interactuando y salimos de un collider Rope (soga), seteamos en null el objeto
        // interactuable del interaction script, para que no pueda interactuar si no esta sobre la soga.
        else if (IsTagged("Rope", other) || IsTagged("TeleportInteracter", other))
        {
            interactionScript.objectToInteract = null;
        }
    }

    void ExitInteracting(Collider2D other)
    {
        // Si salimos de un collider rope mientras interactuaba, resetamos el rigidbody, interaction script, 
        // animaciones y colliders para que esten igual que en un estado previo a la interaccion
        // Luego seteamos el estado por un midAir para poder saltar una vez y tener algo de control en la caida.

        // Quizas para hacerlo más ordenado y objetoso tendríamos que hacer que el mismo objectToInteract limpie todo esto
        // Onda poniendo interactionScript.CleanInteraction y que eso llame al script de interaccion del objectToInteract
        // Pasando como parametro todo lo que tiene que limpiar y listo. Por ahora así alcanza, pero puede crecer bastante.
        if (IsTagged("Rope", other))
        {
            interactionScript.SetKinematic(false);
            ResetAnims();
            movementScript.SetColliderStatus(true);
            movementScript.SwapState(MidAirState.GetInstance());
            interactionScript.interacting = false;
            interactionScript.objectToInteract = null;
        }
        if(IsTagged("TeleportInteracter", other))
        {
            interactionScript.interacting = false;
            interactionScript.objectToInteract = null;
        }

    }

    void ResetAnims()
    {
        interactionScript.ResetAnims();
    }

    private bool IsTagged(string tag, Collider2D other)
    {
        bool res = false;
        if (other.transform.parent != null)
        {
            res = other.tag == tag || other.transform.parent.tag == tag;
        }
        else
        {
            res = other.tag == tag;
        }
        return res;
    }
}
