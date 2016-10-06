using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public GameObject laserShot;
    public GameObject laserChargedShot;

    private int damage;
    public int baseDamage;
    public int damageIncreasePerLevel;
    public int weaponLevel;

    private int bullets;
    public int maxBullets;
    public int Bullets { get { return bullets; } }

    public float chargedShotCD = 1.5f;
    private float chargedShotTimer = 0f;

    public float normalShotCD = 0.2f;
    private float normalShotTimer = 0f;

    public float reloadCD = 0.5f;
    private float reloadTimer = 0f;

    // Use this for initialization
    void Start () {
        damage = baseDamage + (weaponLevel * damageIncreasePerLevel);
        bullets = maxBullets;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateNormalShotTimer();
        UpdateChargedShotTimer();
        UpdateReloadTimer();
    }

    public void Fire(int localScaleFlipFactor) {
        if (!NormalShotOnCD() && gameObject.activeSelf && bullets > 0)
        {
            GameObject shot = Instantiate(laserShot);
            shot.GetComponent<ProjectileScript>().damage = damage;
            shot.transform.position = transform.position + new Vector3(0.001f * localScaleFlipFactor, 0, 0);
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(15 * localScaleFlipFactor, 0);
            normalShotTimer = normalShotCD;
            bullets--;
            if(bullets == 0)
            {
                Reload();
            }
        }
    }

    public void ChargedFire(int localScaleFlipFactor)
    {
        if (!ChargedShotOnCD() && gameObject.activeSelf)
        {
            GameObject shot = Instantiate(laserChargedShot);
            shot.GetComponent<ProjectileScript>().damage = damage;
            shot.transform.position = transform.position + new Vector3(0.001f * localScaleFlipFactor, 0, 0);
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(15 * localScaleFlipFactor, 0);
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

    private void UpdateReloadTimer()
    {
        if (reloadTimer >= Time.deltaTime)
        {
            reloadTimer -= Time.deltaTime;
        }
        else if (Reloading())
        {
            reloadTimer = 0f;
            bullets = maxBullets;
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

    public bool Reloading()
    {
        return reloadTimer != 0f;
    }

    private void Reload()
    {
        reloadTimer = reloadCD;
    }
}
