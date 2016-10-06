using UnityEngine;
using System.Collections;

public class ScaleFixForCanvas : MonoBehaviour {

	void Update () {
        transform.localScale = transform.parent.transform.localScale;
	}
}
