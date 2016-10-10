using UnityEngine;
using System.Collections;

public abstract class UpgradeScript : MonoBehaviour {

    public string upgradeName;
    public string description;

    // Una upgrade puede mejorar el proyectil a mano, por ejemplo, un upgrade de doble daño
    // directamente le cambia el valor de Damage al projectil y no necesita aplicar efecto despues
    // O puede ser como el vampiric que le dice al proyectil que lo tenga en cuenta para cuando
    // golpee al enemigo, y cuando lo haga, lo llame para aplicar el efecto
    public abstract void UpgradeProjectile(ProjectileScript projectile);
    public abstract void ApplyEffect(Collider2D enemy, ProjectileScript projectile);
}
