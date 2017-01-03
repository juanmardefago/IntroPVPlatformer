using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class BuffScript : MonoBehaviour {

    public Image buffImage;
    public string buffName;
    public string buffDescription;
    [SerializeField]
    protected List<BuffScript> buffList;

    public float buffDuration;
    [HideInInspector]
    public float buffTimer;

    protected void Awake()
    {
        buffTimer = buffDuration;
    }

    public void UpdateBuff(GameObject unit)
    {
        if (buffTimer < buffDuration - Time.deltaTime)
        {
            buffTimer += Time.deltaTime;
        } else
        {
            buffTimer = buffDuration;
            RemoveBuff(unit);
        }
        UpdateEffect(unit);
    }

    // Con este metodo se agrega la posibilidad de modificar el efecto del buff con el paso del tiempo
    // Por ejemplo, podria hacer que el veneno de un debuff de poison se vuelva mas fuerte o debil con el tiempo.
    protected virtual void UpdateEffect(GameObject unit)
    {
        // El comportamiento basico es no hacer nada.
    }

    public void AddBuffTo(List<BuffScript> buffs)
    {
        buffs.Add(this);
        buffList = buffs;
        buffTimer = 0f;
    }

    public void RemoveBuff(GameObject unit)
    {
        RemoveBaseEffect(unit);
        buffList.Remove(this);
        Destroy(gameObject);
    }

    // Aca se le agrega el comportamiento base del efecto, ya sea slowear, golpear cada X segundos (InvokeRepeat, y CancelInvoke.), etc.
    public abstract void ApplyBaseEffect(GameObject unit);

    // En este se remueve el base effect si se agrego uno.
    public abstract void RemoveBaseEffect(GameObject unit);
}
