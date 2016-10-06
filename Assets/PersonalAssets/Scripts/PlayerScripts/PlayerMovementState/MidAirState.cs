using UnityEngine;
using System.Collections;
using System;

public class MidAirState : PlayerMovementState {
    // Implementacion de Singleton para no tener mil states dando vueltas.
    private static MidAirState instance;

    static public MidAirState GetInstance()
    {
        if (instance == null)
        {
            instance = new MidAirState();
        }
        return instance;
    }

    private bool wantsToJump;
    private float jumpStrength;

    public MidAirState()
    {
        wantsToJump = false;
        jumpStrength = 21f;
    }

    public void KeyPressUpdate()
    {
        wantsToJump = Input.GetButtonDown("Jump");
    }

    public void StateDependentUpdate(PlayerMovement pm)
    {
        if (wantsToJump)
        {
            //rBody.AddRelativeForce(Vector2.up * jumpStrength, ForceMode2D.Force);
            pm.rBody.velocity = Vector2.up * jumpStrength;
            wantsToJump = false;
            pm.anim.SetBool("isJumping", true);
            pm.SwapState(EmptyState.GetInstance());
        }
        pm.CheckHorizontalMovement();
    }
}


