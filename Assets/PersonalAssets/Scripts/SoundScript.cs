using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour
{

    public AudioClip[] deathSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] attackSounds;
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDeathSound()
    {
        PlayClip(deathSounds[NextInt(deathSounds.Length - 1)]);
    }

    public void PlayHitSound()
    {
        PlayClip(hitSounds[NextInt(hitSounds.Length - 1)]);
    }

    public void PlayAttackSound()
    {
        PlayClip(attackSounds[NextInt(attackSounds.Length - 1)]);
    }

    private void PlayClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private int NextInt(int max)
    {
        return Mathf.RoundToInt(Random.value * max);
    }
}
