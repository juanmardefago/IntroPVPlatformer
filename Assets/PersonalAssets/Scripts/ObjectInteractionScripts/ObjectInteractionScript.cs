using UnityEngine;
using System.Collections;

public abstract class ObjectInteractionScript : MonoBehaviour {

    public abstract void DeInteract(PlayerInteraction pi, PlayerMovement pm);
    public abstract void Interact(PlayerInteraction pi, PlayerMovement pm);
}
