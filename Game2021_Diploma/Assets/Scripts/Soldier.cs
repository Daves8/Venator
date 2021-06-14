using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public float hp;
    private Animator _animator;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;
    private SpawnEnemyes _enemies;

    void Start()
    {
        _enemies = GameObject.FindGameObjectWithTag("Enemies").GetComponent<SpawnEnemyes>();
        ++_enemies.allEnemies["EnemySoldier"];
    }

    void Update()
    {

    }
}