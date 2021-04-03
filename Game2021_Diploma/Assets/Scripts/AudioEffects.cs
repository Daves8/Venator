using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public AudioSource step;
    public AudioSource hit;

    public AudioClip[] steps;
    public AudioClip[] hits;
    public AudioClip[] shoots;

    public void Step()
    {
        step.volume = 0.5f;
        step.pitch = Random.Range(0.9f, 1.1f);
        step.PlayOneShot(steps[Random.Range(0, steps.Length)]);
    }

    public void Hit()
    {
        hit.pitch = Random.Range(0.9f, 1.1f);
        hit.PlayOneShot(hits[Random.Range(0, hits.Length)]);
    }

    public void Shoot()
    {
        hit.pitch = Random.Range(0.9f, 1.1f);
        hit.PlayOneShot(shoots[Random.Range(0, shoots.Length)]);
    }
}