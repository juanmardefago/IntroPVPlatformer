using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.transform.position = new Vector3(-6f, -1.35f, 0f);
            other.SendMessage("Die");
        }
        if (other.tag == "Enemy")
        {
            other.SendMessage("Die");
        }
    }

}
