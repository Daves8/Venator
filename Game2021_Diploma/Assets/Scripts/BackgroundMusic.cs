using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    public AudioClip forestMusics;
    public AudioClip villageMusics;
    public AudioClip battleMusics;
    private AudioClip _music;
    public PlayingMusic _playingMusic;

    private GameObject _player;
    private PlayerCharacteristics _playerCharacteristics;

    public enum PlayingMusic
    {
        nothing = -1,
        village = 0,
        forest,
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
        if (_playerCharacteristics.isBattle || _playerCharacteristics.isBattleAnimal)
        {
            _playingMusic = PlayingMusic.battle;
            _music = battleMusics;
        }
        else if (_playerCharacteristics.place == PlayerCharacteristics.Place.village)
        {
            _playingMusic = PlayingMusic.village;
            _music = villageMusics;
        }
        else if (_playerCharacteristics.place == PlayerCharacteristics.Place.forest)
        {
            _playingMusic = PlayingMusic.forest;
            _music = forestMusics;
        }


        if (_backgroundMusic.clip != _music || !_backgroundMusic.isPlaying)
        {
            _backgroundMusic.clip = _music;
            _backgroundMusic.Play();
        }

    }
}