using UnityEngine;
using System.Collections;
using System;

public class GroundedState : PlayerMovementState {

    // Implementacion de Singleton para no tener mil states dando vueltas.
    private static GroundedState instance;

    static public GroundedState GetInstance()
    {
        if (instance == null)
        {
            instance = new GroundedState();
        }
        return instance;
    }
    
    // Variables
    private bool wantsToJump;
    private bool letGoJump;
    private float jumpStrength;

    // Constructor
    public GroundedState()
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
            pm.SwapState(MidAirState.GetInstance());
        }
        pm.CheckHorizontalMovement();
    }

    public void KeepStateOnPushback(PlayerMovement pm)
    {

    }

    public void Pushback(PlayerMovement pm, Vector2 dir)
    {
        pm.DefaultPushback(dir);
    }
}
