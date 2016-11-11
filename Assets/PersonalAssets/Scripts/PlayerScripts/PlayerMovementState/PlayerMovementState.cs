using UnityEngine;
using System.Collections;

public interface PlayerMovementState {

    // Si se presiona un boton importante para el estado se checkea y actualiza acá
    // Esto debería tener la captura y guardado del Input en una variable
    // Se llama una vez por Update en el PlayerMovement.cs
    void KeyPressUpdate();

    // Se realiza la acción dependiendo del estado
    // Se llama una vez por Update en el PlayerMovement.cs
    void StateDependentUpdate(PlayerMovement pm);

    void KeepStateOnPushback(PlayerMovement pm);

    void Pushback(PlayerMovement pm, Vector2 dir); 
}
