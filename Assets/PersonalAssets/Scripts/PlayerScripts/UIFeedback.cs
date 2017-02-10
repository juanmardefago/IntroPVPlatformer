using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFeedback : MonoBehaviour {

    public Image feedbackImage;
    public Sprite ropeFeedback;
    public Sprite doorFeedback;
    public Sprite npcFeedback;
    public Sprite treasureFeedback;
    private Color transparent;
    private Color basic;

	// Use this for initialization
	void Start () {
        transparent = new Color(255, 255, 255, 0);
        basic = new Color(255, 255, 255, 255);
        feedbackImage.color = transparent;
	}
    
    public void ResetImage()
    {
        feedbackImage.color = transparent;
    }

    public void ShowRopeFeedback()
    {
        feedbackImage.sprite = ropeFeedback;
        feedbackImage.color = basic;
    }

    public void ShowDoorFeedback()
    {
        feedbackImage.sprite = doorFeedback;
        feedbackImage.color = basic;
    }

    public void ShowNPCFeedback()
    {
        feedbackImage.sprite = npcFeedback;
        feedbackImage.color = basic;
    }

    public void ShowTreasureFeedback()
    {
        feedbackImage.sprite = treasureFeedback;
        feedbackImage.color = basic;
    }
}
