using UnityEngine;
using System.Collections;

public class EnemyMovementBasic : MonoBehaviour {

    private Rigidbody2D rBody;
    public float speed;
    private bool facingRight = false;
    public float recoveryTime;
    private float recoveryTimer;
    private Animator anim;
    public bool canMoveForward;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMoveForward = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (recoveryTimer > 0f)
        {
            recoveryTimer -= Time.deltaTime;
        } else if (recoveryTimer < 0f){
            recoveryTimer = 0f;
        }

        if(rBody.velocity.x == 0)
        {
            anim.SetBool("IsWalking", false);
        }
	
	}

    public void MoveTowardsPosition(Vector2 pos)
    {
        // Debug.Log("Moving to " + pos.ToString());

    }

    public void MoveWithDirection(Vector2 dir)
    {
        if (!IsRecovering())
        {
            DoMoveWithDirection(dir);
        }
    }

    private void DoMoveWithDirection(Vector2 dir)
    {
        CorrectLocalScale(dir.x);
        if (canMoveForward)
        {
            rBody.velocity = new Vector2(dir.x * speed, rBody.velocity.y);
            if (dir != Vector2.zero)
            {
                anim.SetBool("IsWalking", true);
            }
        } else
        {
            rBody.velocity = new Vector2(0, rBody.velocity.y);
        }
    }

    public void CorrectLocalScale(float axis)
    {
        if ((axis > 0 && !facingRight) || (axis < 0 && facingRight))
        {
            FlipScale();
        }
    }

    public void FlipScale()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = scale.x * -1;
        transform.localScale = scale;
    }

    public void PushbackTo(Vector2 dir)
    {
        rBody.AddRelativeForce(dir * 3, ForceMode2D.Impulse);
        anim.SetBool("IsWalking", false);
    }

    private bool IsRecovering()
    {
        return recoveryTimer != 0f;
    }

    public void SetRecovering()
    {
        recoveryTimer = recoveryTime;
    }

    public void MakeKinematic()
    {
        rBody.isKinematic = true;
    }
}
