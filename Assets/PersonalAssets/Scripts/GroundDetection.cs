using UnityEngine;
using System.Collections;

public class GroundDetection : MonoBehaviour {

    private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
        movementScript = GetComponentInParent<PlayerMovement>();
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.parent.tag == "Ground") {
            movementScript.IsGrounded(true);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.transform.parent.tag == "Ground") {
            movementScript.IsGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        movementScript.IsGrounded(false);
    }
}
