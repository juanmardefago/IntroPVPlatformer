using UnityEngine;
using System.Collections;

public class SpikeDamageScript : MonoBehaviour
{
    public int damage;
    public float hitCD;
    private bool hitting;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (!hitting) StartCoroutine(DamagePlayer(collision.transform.gameObject));
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (!hitting) StartCoroutine(DamagePlayer(collision.transform.gameObject));
        }
    }

    private IEnumerator DamagePlayer(GameObject player)
    {
        hitting = true;
        player.SendMessage("TakeSpikeDamage", damage);
        yield return new WaitForSeconds(hitCD);
        hitting = false;
    }

}
