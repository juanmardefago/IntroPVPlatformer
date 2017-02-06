using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChanceDebuffApplier : MonoBehaviour {

    public List<GameObject> debuffs;
    public List<float> chance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<GameObject> GetRolledBuffs()
    {
        List<GameObject> res = new List<GameObject>();
        for(int i = 0; i < debuffs.Count; i++)
        {
            if(Random.value <= chance[i])
            {
                res.Add(debuffs[i]);
            }
        }
        return res;
    }

    public void AddBuffWithChance(GameObject debuff, float rate)
    {
        debuffs.Add(debuff);
        chance.Add(rate);
    }
}
