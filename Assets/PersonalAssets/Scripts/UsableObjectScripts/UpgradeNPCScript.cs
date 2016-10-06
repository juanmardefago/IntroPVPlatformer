using UnityEngine;
using System.Collections;
using System;

public class UpgradeNPCScript : UsableObjectScript
{
    public override void DeInteract(PlayerNPCInteraction pi)
    {
        Debug.Log("Deje de interactuar con el NPC");
    }

    public override void Interact(PlayerNPCInteraction pi)
    {
        Debug.Log("Estoy interactuando con el NPC");
    }
}
