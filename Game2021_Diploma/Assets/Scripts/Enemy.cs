using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Animator _animator;

    public float _hp;
    public GameObject swordOn;
    public GameObject swordOff;

    private bool _agressive = false;
    private bool _agrPast = false;
    private bool _attack = false;
    private bool _death = false;

    private bool _coroutStart;

    private NavMeshAgent _agent;
    private GameObject _player;
    private PlayerCharacteristics _playerCharacteristics;
    private GameObject[] _buildsForPatrol;
    private ImportantBuildings _importantBuildings;
    private BuildEn _nextBuild;

    private bool _canAttack = false;

    private List<Rigidbody> ragdolls;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCharacteristics = _player.GetComponent<PlayerCharacteristics>();
        _agent = GetComponent<NavMeshAgent>();

        _hp = Random.Range(100, 200);

        _importantBuildings = GameObject.FindGameObjectWithTag("BuildingsImportant").GetComponent<ImportantBuildings>();
        _buildsForPatrol = new GameObject[] { _importantBuildings.EntranceToTavern, _importantBuildings.Garden, _importantBuildings.RightGate, _importantBuildings.RightUpGate, _importantBuildings.LeftUpGate };
        _nextBuild = (BuildEn)Random.Range(0, _buildsForPatrol.Length);

        ragdolls.AddRange(GetComponentsInChildren<Rigidbody>());
        foreach (Rigidbody rigidbody in ragdolls)
        {
            rigidbody.isKinematic = true;
        }
    }

    private void Update()
    {
        if (_death) { return; }
        if (_hp <= 0f)
        {
            _death = true;
            _attack = false;
            _agent.enabled = false;
            _animator.SetInteger("Death", Random.Range(0, 2));
            Invoke("Death", 2.0f);
            Invoke("Delete", 300.0f);
            return;
        }

        if (_playerCharacteristics.isBattle && Vector3.Distance(_player.transform.position, transform.position) <= 20f)
        {
            _agressive = true;
            Add(gameObject);
        }
        else if (Vector3.Distance(_player.transform.position, transform.position) > 20f)
        {
            _agressive = false;
            _canAttack = false;
            _playerCharacteristics.allEnemies.Remove(gameObject);
            if (_agrPast != _agressive)
            {
                StartCoroutine(CycleAfterBattle());
            }
        }

        if (_agent.velocity.magnitude > 0f)
        {
            if (_agressive)
            {
                _agent.speed = Random.Range(3.8f, 4.2f);
                _animator.SetBool("IdleToWalk", false);
                _animator.SetBool("IdleToRun", true);
            }
            else
            {
                _agent.speed = Random.Range(1.4f, 1.6f);
                _animator.SetBool("IdleToRun", false);
                _animator.SetBool("IdleToWalk", true);
            }
        }
        else
        {
            _animator.SetBool("IdleToRun", false);
            _animator.SetBool("IdleToWalk", false);
        }

        if (_agressive) { Attack(); }
        else
        {
            if (Vector3.Distance(_buildsForPatrol[(int)_nextBuild].transform.position, transform.position) < 5f)
            {
                _nextBuild = (BuildEn)Random.Range(0, _buildsForPatrol.Length);
            }
            else
            {
                _agent.SetDestination(_buildsForPatrol[(int)_nextBuild].transform.position);
            }
        }
        _agrPast = _agressive;
    }

    private void Attack()
    {
        if (_agrPast != _agressive)
        {
            StartCoroutine(CycleBattle());
            return;
        }

        if (!_canAttack) { return; }
        if (Vector3.Distance(_player.transform.position, transform.position) >= 1.5f)
        {
            _attack = false;
            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            _attack = true;
            if (!_coroutStart)
            {
                StartCoroutine(Battle());
            }
        }
    }

    IEnumerator Battle()
    {
        _agent.isStopped = true;
        _coroutStart = true;
        StartCoroutine(RotateToPlayer());
        while (_attack)
        {
            _animator.SetTrigger("Attack" + Random.Range(0, 4));
            yield return new WaitForSeconds(Random.Range(0.9f, 1.6f));
        }
        _agent.isStopped = false;
        _coroutStart = false;
    }
    IEnumerator CycleBattle()
    {
        _animator.SetTrigger("SwordOn");
        _agent.isStopped = true;
        yield return new WaitForSeconds(1.0f);
        _canAttack = true;
        _agent.isStopped = false;
    }
    IEnumerator CycleAfterBattle()
    {
        _animator.SetTrigger("SwordOff");
        _agent.isStopped = true;
        yield return new WaitForSeconds(1.0f);
        _agent.isStopped = false;
    }
    IEnumerator RotateToPlayer()
    {
        while (_attack)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_death)
        {
            if (other.gameObject.tag == "Sword")
            {
                _agressive = true;
                Add(gameObject);
                _hp -= Random.Range(30, 70);
                //print(other.gameObject.name + " попал! Осталось хп: " + _hp);
            }
            else if (other.gameObject.tag == "Knife")
            {
                _agressive = true;
                Add(gameObject);
                _hp -= Random.Range(10, 30);
                //print(other.gameObject.name + " попал! Осталось хп: " + _hp);
            }
            else if (other.gameObject.tag == "Arrow")
            {
                Add(gameObject);
                _agressive = true;
                _hp -= Random.Range(30, 100);
                //print("Стрела попала. Осталось хп: " + _hp);
            }
        }
    }
    private void Add(GameObject enemy)
    {
        if (!_playerCharacteristics.allEnemies.Contains(enemy))
        {
            _playerCharacteristics.allEnemies.Add(enemy);
        }
    }
    private void Death()
    {
        _animator.enabled = false;
        _playerCharacteristics.allEnemies.Remove(gameObject);

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach (Rigidbody rigidbody in ragdolls)
        {
            rigidbody.isKinematic = false;
        }
    }
    private void Delete()
    {
        Destroy(gameObject);
    }

    private void SwordOn()
    {
        swordOff.SetActive(false);
        swordOn.SetActive(true);
    }
    private void SwordOff()
    {
        swordOn.SetActive(false);
        swordOff.SetActive(true);
    }
    // звук шагов и ударов


    public enum BuildEn
    {
        EntranceToTavern,
        Garden,
        RightGate,
        RightUpGate,
        LeftUpGate
    }
}