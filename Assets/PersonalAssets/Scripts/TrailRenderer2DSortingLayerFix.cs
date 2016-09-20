using UnityEngine;
using System.Collections;

public class TrailRenderer2DSortingLayerFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.sortingLayerName = "Projectiles";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
