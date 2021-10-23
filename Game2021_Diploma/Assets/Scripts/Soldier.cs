using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public float hp;
    private Animator _animator;
    private NavMeshAgent _agent;
    private SpawnEnemyes _enemies;

    void Start()
    {
        //_animator = GetComponent<Animator>();
        //_agent = GetComponent<NavMeshAgent>();
        _enemies = GameObject.FindGameObjectWithTag("Enemies").GetComponent<SpawnEnemyes>();
        ++_enemies.allEnemies["EnemySoldier"];
    }

    void Update()
    {
        //if (_agent.velocity.normalized.magnitude >= 0.1f)
        //{
        //    //StopCoroutine("AnimIdle");
        //    _animator.SetBool("Walk", true);
        //}
        //else
        //{
        //    _animator.SetBool("Walk", false);
        //}
    }
}