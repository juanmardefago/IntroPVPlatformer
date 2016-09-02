using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    // Movement-specific variables
    private float moveX;
    public float speed;
    private Rigidbody2D rBody;
    private bool wantsToJump;
    public float jumpStrength;
    [HideInInspector]
    public bool isGrounded;
    private float jumpTimer;
    public float maxTimeJumping;
    private bool letGoJump;
    private bool isCurrentlyJumping;


	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        jumpTimer = 0f;
        letGoJump = false;
	}
	
	// Hago el check de si un player quiere saltar en update porque en general se va a llamar más veces por segundo que el fixedUpdate.
    // Lo mismo para el check de si un player solto el boton de saltar, este nos sirve para no volver a dejarlo intentar "planear" en el aire, si ya lo solto una vez.
	void Update () {
        wantsToJump = Input.GetButton("Jump");
        if (!letGoJump)
        {
            letGoJump = Input.GetButtonUp("Jump");
        }
	}

    // En el fixed update hago los cambios basicos de fisica, si se esta moviendo horizontalmente y el checkeo de jump.
    void FixedUpdate () {
        moveX = Input.GetAxis("Horizontal");
        rBody.velocity = new Vector2(moveX * speed, rBody.velocity.y);
        CheckJump();
    }

    // Esta funcion la llama el groundCheck, si toca piso manda true, si deja de tocar piso manda false.
    public void IsGrounded(bool groundedStatus) {
        this.isGrounded = groundedStatus;
        if (groundedStatus)
        {
            // solo si esta tocando piso se reinician estos valores, porque sino cuando deja de tocar piso tambien se reiniciarian y el user podría "planear".
            this.jumpTimer = 0f;
            letGoJump = false;
            isCurrentlyJumping = false;
        }
    }

    //hace el checkeo de Jump
    private void CheckJump()
    {
        // Si quiso saltar en un Update y esta en el piso salta, esto es para evitar que se hagan multiples saltos en el aire.
        if (wantsToJump && isGrounded){
            //rBody.AddRelativeForce(Vector2.up * jumpStrength, ForceMode2D.Force);
            rBody.velocity += Vector2.up * jumpStrength;
            jumpTimer += Time.deltaTime;
            wantsToJump = false;
            isCurrentlyJumping = true;
        } // Si sigue queriendo saltar, pero además esta saltando, se checkea si no se le agoto el timer de salto y si no solto la tecla.
        else if (wantsToJump && isCurrentlyJumping && jumpTimer < maxTimeJumping && !letGoJump){
            jumpTimer += Time.deltaTime;
            rBody.velocity += Vector2.up * jumpStrength * 0.1f;
        } // Si solto la tecla no va a poder volver a continuar el salto.
        else if (letGoJump){
            jumpTimer = maxTimeJumping;
        }
    }
}
