using UnityEngine;
using System.Collections;
using System;

public class RopeInteraction : ObjectInteractionScript {

    public override void DeInteract(PlayerInteraction pi)
    {
        pi.SwapState(MidAirState.GetInstance());
        pi.anim.SetBool("isClimbing", false);
        pi.SetKinematic(false);
    }

    public override void Interact(PlayerInteraction pi)
    {
        pi.SwapState(ClimbingState.GetInstance());
        pi.ResetAnims();
        pi.anim.SetBool("isClimbing", true);
        pi.SetKinematic(true);
    }
}
