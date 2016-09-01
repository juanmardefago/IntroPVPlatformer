using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    // Movement-specific variables
    private float moveX;
    public float speed;
    private Rigidbody2D rBody;
    private bool m_Jump;
    private bool holdingJump;
    public float jumpStrength;
    [HideInInspector]
    public bool isGrounded;
    private float jumpTimer;
    public float maxTimeJumping;
    private bool letGoJump;


	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        jumpTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        m_Jump = Input.GetButton("Jump");
        letGoJump = Input.GetButtonUp("Jump");
	}

    void FixedUpdate () {
        moveX = Input.GetAxis("Horizontal");
        rBody.velocity = new Vector2(moveX * speed, rBody.velocity.y);

        if (m_Jump && isGrounded) {
            //rBody.AddRelativeForce(Vector2.up * jumpStrength, ForceMode2D.Force);
            rBody.velocity += Vector2.up * jumpStrength;
            jumpTimer += Time.deltaTime;
            m_Jump = false;
        } else if (m_Jump && jumpTimer < maxTimeJumping && !letGoJump) {
            jumpTimer += Time.deltaTime;
            rBody.velocity += Vector2.up * jumpStrength * 0.1f;
            letGoJump = false;
        } 
    }

    public void IsGrounded(bool isGrounded) {
        this.isGrounded = isGrounded;
        this.jumpTimer = 0f;
    }
}
