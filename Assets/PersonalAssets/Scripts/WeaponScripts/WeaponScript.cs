using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour {

    public GameObject laserShot;
    public GameObject laserChargedShot;

    public string weaponName;
    public int weaponPrice;

    [SerializeField]
    private int damage;
    private int deviation;
    public int baseDamage;
    public int damageIncreasePerLevel;
    public int weaponLevel;
    public int weaponPriceToLevelUp;

    private int bullets;
    public int maxBullets;
    public int Bullets { get { return bullets; } }

    public float chargedShotCD = 1.5f;
    private float chargedShotTimer = 0f;

    public float normalShotCD = 0.2f;
    private float normalShotTimer = 0f;

    public float reloadCD = 0.5f;
    private float reloadTimer = 0f;

    public List<UpgradeScript> gems;
    public int gemSlots;

    public float hitRate;
    public float critRate;

    // Use this for initialization
    void Start () {
        RecalculateDamage();
        bullets = maxBullets;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateNormalShotTimer();
        UpdateChargedShotTimer();
        UpdateReloadTimer();
    }

    public void Fire(int localScaleFlipFactor) {
        if (!NormalShotOnCD() && gameObject.activeSelf && bullets > 0 && !Reloading())
        {
            normalShotTimer = normalShotCD;
            GameObject shot = Instantiate(laserShot);
            ProjectileScript pScript = shot.GetComponent<ProjectileScript>();
            pScript.hitRate = hitRate;
            if (Random.value <= critRate)
            {
                pScript.damage = NormalDistribution.CalculateNormalDistRandom(damage * 2, deviation * 2);
                pScript.isCrit = true;
            }
            else
            {
                pScript.damage = NormalDistribution.CalculateNormalDistRandom(damage, deviation);
            }
            ApplyUpgrades(pScript);
            shot.transform.position = transform.position + new Vector3(0.0001f * localScaleFlipFactor - localScaleFlipFactor * 0.3f, 0, 0);
            pScript.SetFlipFactor(localScaleFlipFactor);
            bullets--;
            if(bullets == 0)
            {
                Reload();
            }
        }
    }

    public void ChargedFire(int localScaleFlipFactor)
    {
        if (!ChargedShotOnCD() && gameObject.activeSelf && !Reloading())
        {
            chargedShotTimer = chargedShotCD;
            GameObject shot = Instantiate(laserChargedShot);
            ProjectileScript pScript = shot.GetComponent<ProjectileScript>();
            // Aca se podría hacer que en vez de que el chargedShot sea siempre x4, tenga una variable que se puede tocar desde el inspector de unity.
            pScript.hitRate = hitRate;
            if (Random.value <= critRate)
            {
                pScript.damage = NormalDistribution.CalculateNormalDistRandom(damage * 4, deviation * 4);
                pScript.isCrit = true;
            } else
            {
                pScript.damage = NormalDistribution.CalculateNormalDistRandom(damage * 4, deviation * 4);
            }
            ApplyUpgrades(pScript);
            shot.transform.position = transform.position + new Vector3(0.0001f * localScaleFlipFactor - localScaleFlipFactor * 0.01f, 0, 0);
            pScript.SetFlipFactor(localScaleFlipFactor);
        }
    }

    private void ApplyUpgrades(ProjectileScript pScript)
    {
        foreach (UpgradeScript upgrade in gems)
        {
            upgrade.UpgradeProjectile(pScript);
        }
    }

    private void RecalculateDamage()
    {
        damage = baseDamage + ((weaponLevel - 1) * damageIncreasePerLevel);
        deviation = Mathf.Max((damage / 100) * 5, 1);
    }

    private void RecalculatePriceToLevelUp()
    {
        weaponPriceToLevelUp += weaponPriceToLevelUp;
    }

    public void LevelUp()
    {
        weaponLevel++;
        RecalculateDamage();
        RecalculatePriceToLevelUp();
    }

    // Cooldown Related

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

    // Boolean checks

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

    public void ReloadIfNeeded()
    {
        if(bullets < maxBullets && !Reloading())
        {
            Reload();
        }
    }

    public void AddGem(GameObject gem)
    {
        UpgradeScript gemScript = gem.GetComponent<UpgradeScript>();
        gems.Add(gemScript);
        gemScript.ApplyEffectToWeapon(this);
    }

    public void RemoveGem(GameObject gem)
    {
        UpgradeScript gemScript = gem.GetComponent<UpgradeScript>();
        gems.Remove(gemScript);
        gemScript.RemoveEffectFromWeapon(this);
    }

    public bool CanEquipGem()
    {
        return gems.Count < gemSlots;
    }
}
