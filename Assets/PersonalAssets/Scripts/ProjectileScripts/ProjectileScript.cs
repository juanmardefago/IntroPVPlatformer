using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    private float timer;
    public float maxTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    if(timer < maxTime)
        {
            timer += Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
	}
}
