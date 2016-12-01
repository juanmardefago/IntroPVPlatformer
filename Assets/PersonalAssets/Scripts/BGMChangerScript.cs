using UnityEngine;
using System.Collections;

public class BGMChangerScript : MonoBehaviour
{
    public AudioClip areaMusic;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (audioSource.clip != areaMusic)
        {
            audioSource.clip = areaMusic;
            audioSource.Play();
        }
    }
}
