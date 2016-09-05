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
    [SerializeField]
    private PlayerMovementState playerMovementState;
    private Collider2D playerColl;


	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovementState = EmptyState.GetInstance();
        playerColl = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
        playerMovementState.KeyPressUpdate();
        playerMovementState.StateDependentUpdate(this);
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
        moveX = Input.GetAxis("Horizontal");
        if (moveX != 0) {
            rBody.velocity = new Vector2(moveX * speed, rBody.velocity.y);
            anim.SetBool("isWalking", true);
            anim.SetFloat("speed", Mathf.Abs(moveX));
        } else {
            anim.SetBool("isWalking", false);
            anim.SetFloat("speed", 0);
            rBody.velocity = new Vector2(0, rBody.velocity.y);
        }

        CorrectLocalScale(moveX);
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

    // DEPRECATED

    //hace el checkeo de Jump
    //private void CheckJump()
    //{
    //    // Si quiso saltar en un Update y esta en el piso salta, esto es para evitar que se hagan multiples saltos en el aire.
    //    if (wantsToJump && isGrounded){
    //        //rBody.AddRelativeForce(Vector2.up * jumpStrength, ForceMode2D.Force);
    //        rBody.velocity += Vector2.up * jumpStrength;
    //        jumpTimer += Time.deltaTime;
    //        wantsToJump = false;
    //        isCurrentlyJumping = true;
    //        anim.SetBool("isJumping", true);
    //    } // Si sigue queriendo saltar, pero además esta saltando, se checkea si no se le agoto el timer de salto y si no solto la tecla.
    //    else if (wantsToJump && isCurrentlyJumping && jumpTimer < maxTimeJumping && !letGoJump){
    //        jumpTimer += Time.deltaTime;
    //        rBody.velocity += Vector2.up * jumpStrength * 0.1f;
    //    } // Si solto la tecla no va a poder volver a continuar el salto.
    //    else if (letGoJump){
    //        jumpTimer = maxTimeJumping;
    //    }
    //}
}
