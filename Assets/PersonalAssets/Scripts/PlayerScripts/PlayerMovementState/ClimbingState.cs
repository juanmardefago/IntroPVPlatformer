﻿using UnityEngine;
using System.Collections;
using System;

public class ClimbingState : PlayerMovementState
{
    // Implementacion de Singleton para no tener mil states dando vueltas.
    private static ClimbingState instance;

    static public ClimbingState GetInstance()
    {
        if (instance == null)
        {
            instance = new ClimbingState();
        }
        return instance;
    }

    private float moveY;
    private float speed;

    public ClimbingState()
    {
        moveY = 0f;
        //speed = 5f;
    }

    public void KeyPressUpdate()
    {
        moveY = Input.GetAxis("Vertical");
    }

    public void StateDependentUpdate(PlayerMovement pm)
    {
        speed = pm.speed * 0.5f;
        pm.SetColliderStatus(false);
        VerticalMovement(pm.rBody);
        pm.anim.SetFloat("speed", moveY);
    }

    private void VerticalMovement(Rigidbody2D rBody)
    {
        if (moveY != 0)
        {
            rBody.velocity = new Vector2(0, moveY * speed);
        }
        else
        {
            rBody.velocity = new Vector2(0, 0);
        }
    }

    public void KeepStateOnPushback(PlayerMovement pm)
    {

    }

    public void Pushback(PlayerMovement pm, Vector2 dir)
    {
        pm.DefaultPushback(dir);
    }
}
