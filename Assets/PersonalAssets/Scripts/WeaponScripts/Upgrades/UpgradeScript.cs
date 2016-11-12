using UnityEngine;
using System.Collections;

public abstract class UpgradeScript : MonoBehaviour {

    public Sprite upgradeSprite;
    public string upgradeName;
    public string description;
    public int upgradePrice;

    // Una upgrade puede mejorar el proyectil a mano, por ejemplo, un upgrade de doble daño
    // directamente le cambia el valor de Damage al projectil y no necesita aplicar efecto despues
    // O puede ser como el vampiric que le dice al proyectil que lo tenga en cuenta para cuando
    // golpee al enemigo, y cuando lo haga, lo llame para aplicar el efecto

    // En este metodo se define que se va a hacer cuando se le pasa el script al proyectil, se podría cambiar algunos valores
    // como el daño, o se lo podria agregar a una lista que va es llamada al momento de impactar con ApplyEffect.
    public abstract void UpgradeProjectile(ProjectileScript projectile);

    // Es llamado al momento del impacto contra un enemy si en el UpgradeProjectile le pediste que te agregue a la lista.
    public abstract void ApplyEffect(Collider2D enemy, ProjectileScript projectile);

    // Es llamado al equipar la gema en un arma, se puede modificar valores del arma, o no hacer nada si solo se quiere modificar el proyectil.
    public abstract void ApplyEffectToWeapon(WeaponScript weapon);

    // Es llamado al remover la gema de un arma, tiene que deshacer todos los efectos que se pusieron en ApplyEffectToWeapon.
    public abstract void RemoveEffectFromWeapon(WeaponScript weapon);
}
