using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    public AudioClip[] musics;
    public PlayingMusic _playingMusic;

    private GameObject _player;
    private Battle _battlePlayer;

    public enum PlayingMusic
    {
        nothing = -1,
        village = 0,
        battle
    }

    void Start()
    {
        _backgroundMusic = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _battlePlayer = _player.GetComponent<Battle>();
    }

    private void Update()
    {
        if (_battlePlayer.IsBattle)
        {
            _playingMusic = PlayingMusic.battle;
        }
        else
        {
            _playingMusic = PlayingMusic.village;
        }
        
        try
        {
            if (_backgroundMusic.clip != musics[(int)_playingMusic] || !_backgroundMusic.isPlaying)
            {
                _backgroundMusic.clip = musics[(int)_playingMusic];
                _backgroundMusic.Play();
            }
        }
        catch { }
    }
}