using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileScript : MonoBehaviour
{

    private float timer = 0f;
    public float maxTime;
    public int damage;
    public Sprite explotionSprite;
    private bool shouldBurst = false;
    private float burstTimer = 0f;
    public float burstMaxTime;
    private List<UpgradeScript> upgrades;

    public void Awake()
    {
        upgrades = new List<UpgradeScript>();
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDisappearTime();
        CheckForBurstTime();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagged("Ground", other))
        {
            Burst();
        }
        else if (IsTagged("Enemy", other))
        {
            ApplyUpgrades(other);
            Burst();
        }
    }

    private bool IsTagged(string tag, Collider2D other)
    {
        bool res = false;
        if (other.transform.parent != null)
        {
            res = other.tag == tag || other.transform.parent.tag == tag;
        }
        else
        {
            res = other.tag == tag;
        }
        return res;
    }

    private void Burst()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = explotionSprite;
        shouldBurst = true;
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();
        rBody.velocity = new Vector2(0, 0);
    }

    private void CheckForDisappearTime()
    {
        if (timer < maxTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckForBurstTime()
    {
        if (shouldBurst && burstTimer < burstMaxTime)
        {
            burstTimer += Time.deltaTime;
        }
        else if (shouldBurst)
        {
            Destroy(gameObject);
        }
    }

    public void AddUpgrade(UpgradeScript upgrade)
    {
        upgrades.Add(upgrade);
    }

    private void ApplyUpgrades(Collider2D enemy)
    {
        foreach (UpgradeScript upgrade in upgrades)
        {
            upgrade.ApplyEffect(enemy, this);
        }
        enemy.gameObject.SendMessage("TakeDamage", damage);
    }
}
