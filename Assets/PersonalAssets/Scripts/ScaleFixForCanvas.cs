using UnityEngine;
using System.Collections;

public class ScaleFixForCanvas : MonoBehaviour {



	void Update () {
        if((transform.localScale.x > 0 && transform.parent.transform.localScale.x < 0) || (transform.localScale.x < 0 && transform.parent.transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        } 
	}
}
