using UnityEngine;
using System.Collections;

public abstract class UsableObjectScript : MonoBehaviour {

    public abstract void DeInteract(PlayerNPCInteraction pi);
    public abstract void Interact(PlayerNPCInteraction pi);
}
