using UnityEngine;
using System.Collections;
using System;

public class EmptyState : PlayerMovementState {

    // Implementacion de Singleton para no tener mil states dando vueltas.
    private static EmptyState instance;

    static public EmptyState GetInstance()
    {
        if (instance == null)
        {
            instance = new EmptyState();
        }
        return instance;
    }

    public void KeyPressUpdate()
    {
        // Do nothing, this is an empty state.
    }

    public void StateDependentUpdate(PlayerMovement pm)
    {
        pm.CheckHorizontalMovement();
    }
}
