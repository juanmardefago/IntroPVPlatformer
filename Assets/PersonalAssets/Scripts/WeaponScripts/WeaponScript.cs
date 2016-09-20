using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public Rigidbody2D laserShot;
    public Rigidbody2D laserChargedShot;

    private float chargedShotCD = 1.5f;
    private float chargedShotTimer = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (chargedShotTimer >= Time.deltaTime) {
            chargedShotTimer -= Time.deltaTime;
        } else if (ChargedShotOnCD()) {
            chargedShotTimer = 0f;
        }
	}

    public void Fire(int localScaleFlipFactor) {
        Rigidbody2D shot = (Rigidbody2D) Instantiate(laserShot);
        shot.transform.position = transform.position + new Vector3(0.001f * localScaleFlipFactor, 0, 0);
        shot.velocity = new Vector2(15 * localScaleFlipFactor, 0);
    }

    public void ChargedFire(int localScaleFlipFactor)
    {
        if (!ChargedShotOnCD())
        {
            Rigidbody2D shot = (Rigidbody2D)Instantiate(laserChargedShot);
            shot.transform.position = transform.position + new Vector3(0.001f * localScaleFlipFactor, 0, 0);
            shot.velocity = new Vector2(15 * localScaleFlipFactor, 0);
            chargedShotTimer = chargedShotCD;
        }
    }

    private bool ChargedShotOnCD()
    {
        return chargedShotTimer != 0f;
    }
}
