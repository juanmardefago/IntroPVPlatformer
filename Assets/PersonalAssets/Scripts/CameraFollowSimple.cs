using UnityEngine;
using System.Collections;

public class CameraFollowSimple : MonoBehaviour {

    public GameObject target;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0, 0, -10);
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = target.transform.position + offset;
	}
}
