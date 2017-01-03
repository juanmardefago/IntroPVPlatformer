using UnityEngine;
using System.Collections;

public class BuffTileHandler : MonoBehaviour {

    public GameObject buff;
    public bool shouldReapplyOnStay;
    public float reapplyCD;
    private float reapplyTimer;

    private void Start()
    {
        reapplyTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("AddBuff", buff.GetComponent<BuffScript>());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (shouldReapplyOnStay && reapplyTimer == 0f)
        {
            other.SendMessage("AddBuff", buff.GetComponent<BuffScript>());
            reapplyTimer = reapplyCD;
        } else if(shouldReapplyOnStay && reapplyTimer > Time.deltaTime)
        {
            reapplyTimer -= Time.deltaTime;
        } else if(shouldReapplyOnStay && reapplyTimer <= Time.deltaTime)
        {
            reapplyTimer = 0f;
        }
    }


}
