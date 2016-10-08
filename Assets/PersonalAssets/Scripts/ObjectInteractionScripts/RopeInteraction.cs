using UnityEngine;
using System.Collections;
using System;

public class RopeInteraction : ObjectInteractionScript {

    // Al dejar de interactuar se cambia el estado a MidAir (para que pueda saltar una vez y no caer sin control)
    // se desactiva la animacion de Climbing y se vuelve a poner el rigidbody como antes.
    public override void DeInteract(PlayerInteraction pi, PlayerMovement pm)
    {
        pm.rBody.isKinematic = false;
        pi.anim.SetBool("isClimbing", false);
        pm.SetColliderStatus(true);
        pm.SwapState(MidAirState.GetInstance());
        pi.objectToInteract = null;
        pi.SetWeaponActive(true);

        pi.interactingScript = null;
    }

    // Al interactuar con la Rope (Soga), se cambia el estado a Climbing, se resetean las animaciones 
    // poniendo los principales bool del animatorController en false, se pone el animatorController
    // para que muestre la animacion de Climbing, y se cambiar el rigidBody a Kinematic
    // para que deje de ser afectado por la gravedad
    public override void Interact(PlayerInteraction pi, PlayerMovement pm)
    {
        pm.SwapState(ClimbingState.GetInstance());
        pi.ResetAnims();
        pi.anim.SetBool("isClimbing", true);
        pm.rBody.isKinematic = true;
        pi.SetWeaponActive(false);
    }
}
