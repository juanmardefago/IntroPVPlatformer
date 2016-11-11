using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    // Movement-specific variables
    private float moveX;
    public float speed;
    private bool isRunning;
    [HideInInspector]
    public Rigidbody2D rBody;
    [HideInInspector]
    public Animator anim;
    private bool facingRight = true;
    private PlayerMovementState playerMovementState;
    private Collider2D playerColl;
    private float hitRecoveryTimer;
    public float hitRecoveryTime;


	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovementState = EmptyState.GetInstance();
        playerColl = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
        if (IsNotHit())
        {
            playerMovementState.KeyPressUpdate();
            playerMovementState.StateDependentUpdate(this);
        } else
        {
            CheckForRecoveryAndRecover();
            playerMovementState.KeepStateOnPushback(this);
        }
    }

    void FixedUpdate () {
    }

    // Cambia el PlayerMovementState, para que pueda tener un estado diferente dependiendo del collider que esta atravezando.
    public void SwapState(PlayerMovementState pms)
    {
        playerMovementState = pms;
    }

    // Checkea el input que cambia el movimiento horizontal y realiza la acción correspondiente cuando haga falta.
    // Además le da al animator los parametros que necesite para cambiar su estado.
    // Para ser usado por default en los States que lo quieran usar.
    public void CheckHorizontalMovement()
    {
        if (IsNotHit())
        {
            moveX = Input.GetAxis("Horizontal");
            if (moveX != 0)
            {
                rBody.velocity = new Vector2(moveX * speed, rBody.velocity.y);
                anim.SetBool("isWalking", true);
                anim.SetFloat("speed", Mathf.Abs(moveX));
            }
            else
            {
                anim.SetBool("isWalking", false);
                anim.SetFloat("speed", 0);
                rBody.velocity = new Vector2(0, rBody.velocity.y);
            }

            CorrectLocalScale(moveX);
        }
    }

    // Puede ser usado por los state para corregir el localScale si lo necesitan
    public void CorrectLocalScale(float axis)
    {
        if((axis > 0 && !facingRight) || (axis < 0 && facingRight))
        {
            FlipScale();
        }
    }

    private void FlipScale()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = scale.x * -1;
        transform.localScale = scale;
    }

    public void SetColliderStatus(bool status)
    {
        playerColl.enabled = status;
    }

    public void Pushback(Vector2 dir)
    {
        hitRecoveryTimer = hitRecoveryTime;
        playerMovementState.Pushback(this, dir);
    }

    public void DefaultPushback(Vector2 dir)
    {
        rBody.AddForce(dir * 10, ForceMode2D.Impulse);
    }

    private bool IsNotHit()
    {
        return hitRecoveryTimer == 0f;
    }

    private void CheckForRecoveryAndRecover()
    {
        // if IsHit.
        if (!IsNotHit() && hitRecoveryTimer >= Time.deltaTime)
        {
            hitRecoveryTimer -= Time.deltaTime;
        } else if (!IsNotHit() && hitRecoveryTimer < Time.deltaTime)
        {
            hitRecoveryTimer = 0f;
        }
    }
}
