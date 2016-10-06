using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupText : MonoBehaviour {

    public Animator anim;

    private Text textToDisplay;

	// Use this for initialization
	void Start () {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        textToDisplay = anim.GetComponent<Text>();
	}

    public void SetText(string text)
    {
        textToDisplay.text = text;
    }
	
}
