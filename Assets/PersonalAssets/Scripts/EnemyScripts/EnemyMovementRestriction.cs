using UnityEngine;
using System.Collections;

public class EnemyMovementRestriction : MonoBehaviour
{
    public EnemyMovementBasic movementScript;
    public string restrictionTag;
    public bool restrictMovementInsideTag;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (LookupTag(other.transform) == restrictionTag)
        {
            if (restrictMovementInsideTag)
            {
                movementScript.canMoveForward = true;
            } else
            {
                movementScript.canMoveForward = false;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (LookupTag(other.transform) == restrictionTag)
        {
            if (restrictMovementInsideTag)
            {
                movementScript.canMoveForward = true;
            }
            else
            {
                movementScript.canMoveForward = false;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (LookupTag(other.transform) == restrictionTag)
        {
            if (restrictMovementInsideTag)
            {
                movementScript.canMoveForward = false;
            }
            else
            {
                movementScript.canMoveForward = true;
            }
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
