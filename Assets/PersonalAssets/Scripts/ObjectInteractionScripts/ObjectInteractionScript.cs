using UnityEngine;
using System.Collections;

public abstract class ObjectInteractionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void DeInteract(PlayerInteraction pi);
    public abstract void Interact(PlayerInteraction pi);
}
