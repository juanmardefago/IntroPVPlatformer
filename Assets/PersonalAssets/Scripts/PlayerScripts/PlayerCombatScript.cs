using UnityEngine;
using System.Collections;

public class PlayerCombatScript : MonoBehaviour {

    private WeaponScript weapon;
    public int health;
    private Animator anim;

	// Use this for initialization
	void Start () {
        weapon = GetComponentInChildren<WeaponScript>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckForShotInput();
	}

    private void CheckForShotInput()
    {
        if (Input.GetButtonDown("Fire1")) {
            weapon.Fire(OffsetForLocalScale());
        } else if (Input.GetButtonDown("Fire2")) {
            weapon.ChargedFire(OffsetForLocalScale());
        }
    }

    private int OffsetForLocalScale()
    {
        int res = 0;
        if (transform.localScale.x > 0) {
            res = 1;
        } else {
            res = -1;
        }
        return res;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("hit");
    }

}
