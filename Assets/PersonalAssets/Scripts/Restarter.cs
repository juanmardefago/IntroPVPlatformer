using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if(IsTagged("Player", other))
        {
            other.transform.position = new Vector3(-6f, -1.35f, 0f);
            other.SendMessage("Die");
        }
        if (IsTagged("Enemy", other))
        {
            other.SendMessage("Die");
        }
    }

    private bool IsTagged(string tag, Collider2D other)
    {
        bool res = false;
        if (other.transform.parent != null)
        {
            res = other.tag == tag || other.transform.parent.tag == tag;
        }
        else
        {
            res = other.tag == tag;
        }
        return res;
    }
}
