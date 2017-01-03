using UnityEngine;
using System.Collections;

public class BuffTileHandler : MonoBehaviour {

    public GameObject buff;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("AddBuff", buff.GetComponent<BuffScript>());
    }

}
