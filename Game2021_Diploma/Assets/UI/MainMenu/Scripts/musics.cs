using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musics : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    public AudioClip[] music;

    // Start is called before the first frame update
    void Start()
    {
        _backgroundMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_backgroundMusic.isPlaying)
        {
            _backgroundMusic.clip = music[Random.Range(0, music.Length)];
            _backgroundMusic.Play();
        }
    }
}
