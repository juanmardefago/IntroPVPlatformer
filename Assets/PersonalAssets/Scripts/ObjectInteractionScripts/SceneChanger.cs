using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : ObjectInteractionScript {

    public string sceneName;

    public override void DeInteract(PlayerInteraction pi, PlayerMovement pm)
    {
        // Nothing
    }

    public override void Interact(PlayerInteraction pi, PlayerMovement pm)
    {
        SceneManager.LoadScene(sceneName);
    }
}
