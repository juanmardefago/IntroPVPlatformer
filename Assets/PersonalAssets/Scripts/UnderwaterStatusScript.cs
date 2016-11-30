using UnityEngine;
using System.Collections;

public class UnderwaterStatusScript : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spawn")
        {
            other.SendMessage("UnderwaterStatus", true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Spawn")
        {
            other.gameObject.SendMessage("UnderwaterStatus", false);
        }
    }
}
