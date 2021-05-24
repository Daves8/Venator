using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    public AudioClip[] musics;
    public PlayingMusic _playingMusic;

    private GameObject _player;
    private PlayerCharacteristics _playerCharacteristics;

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
        _playerCharacteristics = _player.GetComponent<PlayerCharacteristics>();
    }

    private void Update()
    {
        if (_playerCharacteristics.isBattle )//|| _playerCharacteristics.isBattleAnimal)
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