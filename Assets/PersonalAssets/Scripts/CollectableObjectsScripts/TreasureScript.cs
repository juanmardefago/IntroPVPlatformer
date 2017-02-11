using UnityEngine;
using System.Collections;

public class TreasureScript : UsableObjectScript {

    private LootDropScript lootDrop;
    private BoxCollider2D coll;

    private void Start()
    {
        lootDrop = GetComponent<LootDropScript>();
        coll = GetComponent<BoxCollider2D>();
    }

    public override void DeInteract(PlayerNPCInteraction pi)
    {
        pi.interacting = false;
        pi.objectToInteract = null;
        coll.enabled = false;
    }

    public override void Interact(PlayerNPCInteraction pi)
    {
        lootDrop.DropLoot();
        DeInteract(pi);
    }

}
