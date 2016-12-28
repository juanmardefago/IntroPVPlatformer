using UnityEngine;
using System.Collections;
using System;

public class SwimmingState : PlayerMovementState {

    // Implementacion de Singleton para no tener mil states dando vueltas.
    private static SwimmingState instance;

    static public SwimmingState GetInstance()
    {
        if (instance == null)
        {
            instance = new SwimmingState();
        }
        return instance;
    }

    private float moveX;
    private float moveY;
    private float speed;

    public SwimmingState()
    {
        moveX = 0;
        //speed = 5f;
    }

    public void KeyPressUpdate()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
    }

    public void StateDependentUpdate(PlayerMovement pm)
    {
        speed = pm.speed * 0.5f;
        VerticalMovement(pm.rBody);
        HorizontalMovement(pm.rBody);
        pm.CorrectLocalScale(moveX);
    }

    private void HorizontalMovement(Rigidbody2D rBody)
    {
        if (moveX != 0)
        {
            rBody.velocity = new Vector2(moveX * speed, rBody.velocity.y);
        }
        else
        {
            rBody.velocity = new Vector2(0, rBody.velocity.y);
        }
    }

    private void VerticalMovement(Rigidbody2D rBody)
    {
        if (moveY != 0)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, moveY * speed);
        }
        else
        {
            rBody.velocity = new Vector2(rBody.velocity.x, 0);
        }
    }

    public void KeepStateOnPushback(PlayerMovement pm)
    {
        pm.rBody.velocity = new Vector2(pm.rBody.velocity.x, 0);
    }

    public void Pushback(PlayerMovement pm, Vector2 dir)
    {
        pm.DefaultPushback(dir);
    }
}
