using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    public AudioClip[] musics;

    private bool _isBattle;
    private PlayingMusic _playingMusic;

    private GameObject _player;

    bool first = false;
    bool second = false;

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

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        _isBattle = _player.GetComponent<Battle>().IsBattle;

        if (_isBattle)
        {
            _playingMusic = PlayingMusic.battle;
            Debug.Log("БИТВЫА");
        }
        else
        {
            _playingMusic = PlayingMusic.village;
        }


        if (first != _isBattle)
        {
            second = true;
        }
        else
        {
            second = false;
        }
        


        if (second)
        {
            try { _backgroundMusic.clip = musics[(int)_playingMusic]; _backgroundMusic.Play(); } catch { }
        }

        first = _isBattle;
    }
}