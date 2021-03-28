using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    public AudioClip[] musics;

    private bool _isBattle;
    private PlayingMusic _playingMusic;

    enum PlayingMusic
    {
        nothing = -1,
        village = 0,
        battle
    }

    void Start()
    {
        _backgroundMusic = GetComponent<AudioSource>();
        _isBattle = GameObject.FindGameObjectWithTag("Player").GetComponent<Battle>().IsBattle;
    }

    private void Update()
    {
        if (_isBattle)
        {
            _playingMusic = PlayingMusic.battle;
        }
        else
        {
            _playingMusic = PlayingMusic.nothing;
        }

        //if (!_backgroundMusic.isPlaying)
        {
            try { _backgroundMusic.PlayOneShot(musics[(int)_playingMusic]); } catch { }
        }
    }
}