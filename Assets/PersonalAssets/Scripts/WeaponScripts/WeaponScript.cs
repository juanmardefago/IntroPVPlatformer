using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public Rigidbody2D laserShot;
    public Rigidbody2D laserChargedShot;


    public float chargedShotCD = 1.5f;
    private float chargedShotTimer = 0f;

    public float normalShotCD = 0.2f;
    private float normalShotTimer = 0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateNormalShotTimer();
        UpdateChargedShotTimer();
    }

    public void Fire(int localScaleFlipFactor) {
        if (!NormalShotOnCD() && gameObject.activeSelf)
        {
            Rigidbody2D shot = (Rigidbody2D)Instantiate(laserShot);
            shot.transform.position = transform.position + new Vector3(0.001f * localScaleFlipFactor, 0, 0);
            shot.velocity = new Vector2(15 * localScaleFlipFactor, 0);
            normalShotTimer = normalShotCD;
        }
    }

    public void ChargedFire(int localScaleFlipFactor)
    {
        if (!ChargedShotOnCD() && gameObject.activeSelf)
        {
            Rigidbody2D shot = (Rigidbody2D)Instantiate(laserChargedShot);
            shot.transform.position = transform.position + new Vector3(0.001f * localScaleFlipFactor, 0, 0);
            shot.velocity = new Vector2(15 * localScaleFlipFactor, 0);
            chargedShotTimer = chargedShotCD;
        }
    }

    private void UpdateChargedShotTimer()
    {
        if (chargedShotTimer >= Time.deltaTime)
        {
            chargedShotTimer -= Time.deltaTime;
        }
        else if (ChargedShotOnCD())
        {
            chargedShotTimer = 0f;
        }
    }

    private void UpdateNormalShotTimer()
    {
        if (normalShotTimer >= Time.deltaTime)
        {
            normalShotTimer -= Time.deltaTime;
        }
        else if (NormalShotOnCD())
        {
            normalShotTimer = 0f;
        }
    }

    private bool ChargedShotOnCD()
    {
        return chargedShotTimer != 0f;
    }

    private bool NormalShotOnCD()
    {
        return normalShotTimer != 0f;
    }
}
