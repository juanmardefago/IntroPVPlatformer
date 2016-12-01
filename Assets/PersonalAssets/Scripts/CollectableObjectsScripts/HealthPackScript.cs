using UnityEngine;
using System.Collections;

public class HealthPackScript : MonoBehaviour {

    public int healthToRestore;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("ReceiveHeal", healthToRestore);
            Destroy(gameObject);
        }
        else if (other.tag == "Ground")
        {
            Rigidbody2D rBody = GetComponent<Rigidbody2D>();
            rBody.isKinematic = true;
            rBody.velocity = Vector2.zero;
        }
    }
}
