using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffSystem : MonoBehaviour {

    private GameObject unit;
    [SerializeField]
    private List<BuffScript> buffs;

    // Use this for initialization
    void Start()
    {
        unit = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBuffs();
    }

    private void UpdateBuffs()
    {
        foreach(BuffScript buff in buffs.ToArray())
        {
            buff.UpdateBuff(unit);
        }
    }

    public void AddBuff(BuffScript prefabBuff)
    {
        BuffScript buff = GetBuffWithName(prefabBuff.buffName);
        if(buff == null)
        {
            buff = Instantiate(prefabBuff);
            buff.gameObject.transform.SetParent(unit.transform, false);
            buff.AddBuffTo(buffs);
            buff.ApplyBaseEffect(unit);
        } else
        {
            buff.buffTimer = 0f;
        }
    }

    public void ClearBuffs()
    {
        foreach (BuffScript buff in buffs.ToArray())
        {
            buff.RemoveBuff(unit);
            Destroy(buff);
        }
    }

    public BuffScript GetBuffWithName(string name)
    {
        BuffScript res = null;
        foreach(BuffScript buff in buffs)
        {
            if(buff.buffName == name)
            {
                res = buff;
            }
        }
        return res;
    }

}
