using UnityEngine;
using System.Collections;

public interface Damagable  {
    void TakeDamage(int damage, Color? color = null);
}
