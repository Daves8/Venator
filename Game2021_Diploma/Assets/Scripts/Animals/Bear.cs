﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    public float hp;

    private Animator _bearAnim;
    private NavMeshAgent _bearAgent;
    private PlayerCharacteristics _playerCharact;

    private GameObject _player;
    private GameObject[] _hunters;

    private bool _die = false;
    public bool _agressive = false;
    private bool _startCoroutine = false;
    private bool _startCoroutineW = false;
    private bool _startCoroutineE = false;
    private bool _walkCorout;
    private bool _attack;

    private float _speedWalk = 1.5f;
    private float _speedRun = 4.0f;

    private GameObject[] _places;

    void Start()
    {
        _bearAnim = GetComponent<Animator>();
        _bearAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hunters = GameObject.FindGameObjectsWithTag("Hunter");
        _playerCharact = _player.GetComponent<PlayerCharacteristics>();

        _places = GameObject.FindGameObjectsWithTag("PlacesForBear");
        _places[_places.Length - 1] = GameObject.FindGameObjectWithTag("Den");

        hp = 750;
        StartCoroutine(Healing());
    }

    void Update()
    {
        if (_die) { return; }
        if (hp <= 0)
        {
            _die = true;
            _agressive = false;
            _playerCharact.isBattleAnimal = false;
            _bearAgent.enabled = false;
            _bearAnim.SetTrigger("Die");
            Invoke("Delete", 300.0f);
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < SafetyDistance() || NearHunters())
        {
            _agressive = true;
            _bearAgent.speed = _speedRun;
        }
        else
        {
            _agressive = false;
            _bearAgent.speed = _speedWalk;
        }

        if (_agressive) { _bearAgent.speed = _speedRun; _playerCharact.isBattleAnimal = true; }
        else { _bearAgent.speed = _speedWalk; _playerCharact.isBattleAnimal = false; }

        if (_bearAgent.velocity.magnitude > 0f)
        {
            if (_agressive)
            {
                _bearAnim.SetBool("Walk", false);
                _bearAnim.SetBool("Eat", false);
                _bearAnim.SetBool("Run", true);
            }
            else
            {
                _bearAnim.SetBool("Run", false);
                _bearAnim.SetBool("Eat", false);
                _bearAnim.SetBool("Walk", true);
            }
        }
        else
        {
            _bearAnim.SetBool("Walk", false);
            _bearAnim.SetBool("Run", false);
            _bearAnim.SetBool("Eat", false);
            if (!_agressive && !_startCoroutineE && Random.Range(0, 10) == 0)
            {
                StartCoroutine(Eat());
            }
        }

        if (_agressive && hp <= 250 && Vector3.Distance(transform.position, _places[_places.Length - 1].transform.position) > 7.5f) { RunAway(); }
        else if (_agressive) { Attack(); }
        else { Walking(); }
    }
    private bool NearHunters()
    {
        float distance = Random.Range(9f, 11f);
        for (int i = 0; i < _hunters.Length; i++)
        {
            if (Vector3.Distance(transform.position, _hunters[i].transform.position) < distance)
            {
                return true;
            }
        }
        return false;
    }
    private float SafetyDistance()
    {
        float distance = Random.Range(9f, 11f);
        if (_playerCharact.crouch)
        {
            return distance;
        }
        return distance * 2f;
    }
    private void Attack()
    {
        _walkCorout = false;
        Transform enHit = EnemyForHit().transform;
        if (Vector3.Distance(transform.position, enHit.position) > 1.5f)
        {
            _attack = false;
            _bearAgent.SetDestination(enHit.position);
        }
        else
        {
            _attack = true;
            if (!_startCoroutine)
            {
                StartCoroutine(Hit());
            }
        }
    }
    private Transform EnemyForHit()
    {
        Transform enfh = _player.transform;
        for (int i = 0; i < _hunters.Length; i++)
        {
            if (Vector3.Distance(transform.position, enfh.position) > Vector3.Distance(transform.position, _hunters[i].transform.position))
            {
                enfh = _hunters[i].transform;
            }
        }
        return enfh;
    }
    IEnumerator Hit()
    {
        _bearAgent.isStopped = true;
        _startCoroutine = true;
        while (_attack)
        {
            _bearAnim.SetTrigger("Attack");
            yield return new WaitForSeconds(Random.Range(2.0f, 2.7f));
        }
        _bearAgent.isStopped = false;
        _startCoroutine = false;
    }

    private void Walking()
    {
        _walkCorout = true;
        if (!_startCoroutineW)
        {
            StartCoroutine(Walk());
        }
    }
    IEnumerator Walk()
    {
        _startCoroutineW = true;
        while (_walkCorout)
        {
            _bearAgent.SetDestination(_places[Random.Range(0, _places.Length)].transform.position);
            yield return new WaitForSeconds(Random.Range(5f, 180f));
        }
        _startCoroutineW = false;
    }
    IEnumerator Eat()
    {
        _startCoroutineE = true;
        _bearAnim.SetBool("Eat", true);
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        _bearAnim.SetBool("Eat", false);
        _startCoroutineE = false;
    }

    private void RunAway()
    {
        _attack = false;
        _bearAgent.SetDestination(_places[_places.Length - 1].transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_die)
        {
            if (collision.gameObject.tag == "Arrow")
            {
                Agressive();
                hp -= Random.Range(30, 100);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_die)
        {
            // охотник
            if (other.gameObject.tag == "SwordEn")
            {
                Agressive();
                hp -= Random.Range(30, 70);
            }
            else if (other.gameObject.tag == "KnifeEn")
            {
                Agressive();
                hp -= Random.Range(10, 30);
            }

            // игрок
            if (other.gameObject.tag == "Sword")
            {
                Agressive();
                hp -= Random.Range(30, 70); // 100-150 меч 2-го уровня
            }
            else if (other.gameObject.tag == "Knife")
            {
                Agressive();
                hp -= Random.Range(10, 30);
            }
        }
    }
    private void Agressive()
    {
        if (NearHunters())
        {
            _agressive = true;
        }
        else
        {
            RunAway();
        }
    }

    IEnumerator Healing()
    {
        while (!_die)
        {
            if (hp < 500)
            {
                hp += 20;
            }
            yield return new WaitForSeconds(10f);
        }
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
}