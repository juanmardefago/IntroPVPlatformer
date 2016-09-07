using UnityEngine;
using System.Collections;
using System;

public class TeleportInteraction : ObjectInteractionScript {

    public Transform destination;
    public Vector3 destinationPosition;
    public bool needToCreateReturnPoint;
    public bool needToUseReturnPoint;

    public override void DeInteract(PlayerInteraction pi)
    {
        // No se puede desinteractuar por ahora, ya que teleporta de una
        // Si preguntase si queres teleportar, quizas tendría que hacer algo, o no
        // porque capaz lo haría el boton de "No".
    }

    public override void Interact(PlayerInteraction pi)
    {
        if (needToUseReturnPoint)
        {
            destinationPosition = pi.goBackPositionStack.Pop();
        } else
        {
            destinationPosition = destination.position;
        }

        if (needToCreateReturnPoint)
        {
            pi.goBackPositionStack.Push(pi.transform.position);
        }

        pi.transform.position = destinationPosition;
    }

}
