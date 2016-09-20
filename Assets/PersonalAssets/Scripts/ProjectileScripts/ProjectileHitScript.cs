using UnityEngine;
using System.Collections;

public class ProjectileHitScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(IsTagged("Ground", other)) {
            Destroy(gameObject);
        }
        
    }

    private bool IsTagged(string tag, Collider2D other)
    {
        return other.tag == tag || other.transform.parent.tag == tag;
    }
}
