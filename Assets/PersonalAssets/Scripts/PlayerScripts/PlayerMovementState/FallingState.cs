using UnityEngine;
using System.Collections;
using System;

public class FallingState : PlayerMovementState
{

    // Implementacion de Singleton para no tener mil states dando vueltas.
    private static FallingState instance;

    static public FallingState GetInstance()
    {
        if (instance == null)
        {
            instance = new FallingState();
        }
        return instance;
    }

    public void KeyPressUpdate()
    {
        // Do nothing
    }

    public void StateDependentUpdate(PlayerMovement pm)
    {
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
