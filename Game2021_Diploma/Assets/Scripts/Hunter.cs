using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hunter : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;

    private SpawnEnemyes _enemies;
    private GameObject arrow;

    private float _speedRun;
    private float _speedWalk;
    private bool _shot;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();

        _speedRun = Random.Range(3.0f, 4.0f);
        _speedWalk = Random.Range(1.5f, 2.5f);
        _shot = false;

        _enemies = GameObject.FindGameObjectWithTag("Enemies").GetComponent<SpawnEnemyes>();
        ++_enemies.allEnemies["Partisans"];
        arrow = _enemies.arrow;
    }

    void Update()
    {

        if (_agent.velocity.magnitude > 0f)
        {
            if (_agent.speed == _speedWalk)
            {
                _animator.SetBool("Walk", true);
                _animator.SetBool("Run", false);
            }
            else if (_agent.speed == _speedRun)
            {
                _animator.SetBool("Run", true);
                _animator.SetBool("Walk", false);
            }
        }
        else
        {
            _animator.SetBool("Run", false);
            _animator.SetBool("Walk", false);
        }
    }

    public void Attack(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) > Random.Range(8.0f, 15.0f))
        {
            RunTo(target);
        }
        else
        {
            _shot = true;
            StartCoroutine(RotateToTarget(target));
            _animator.SetTrigger("Shot");
        }
    }

    private IEnumerator RotateToTarget(Transform target)
    {
        while (_shot)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            yield return null;
        }
    }

    public void WalkTo(Transform target)
    {
        _agent.speed = _speedWalk;
        _agent.SetDestination(target.position);
    }

    public void RunTo(Transform target)
    {
        _agent.speed = _speedRun;
        _agent.SetDestination(target.position);
    }

    private void ShotArrow()
    {

    }
}