using UnityEngine;
using System.Collections;

public class EnemyMovementRestriction : MonoBehaviour
{
    public EnemyMovementBasic movementScript;
    public string restrictionTag;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (LookupTag(other.transform) == restrictionTag)
        {
            movementScript.canMoveForward = true;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (LookupTag(other.transform) == restrictionTag)
        {
            movementScript.canMoveForward = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (LookupTag(other.transform) == restrictionTag)
        {
            movementScript.canMoveForward = false;
        }
    }

    private string LookupTag(Transform trans)
    {
        string res = "Untagged";
        if (trans.tag != "Untagged")
        {
            res = trans.tag;
        }
        else if (trans.parent != null)
        {
            res = LookupTag(trans.parent);
        }
        return res;
    }
}
