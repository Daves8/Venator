using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Animator _animator;

    public float _hp;

    private bool _agressive = false;
    private bool _attack = false;
    private bool _death = false;

    private bool _coroutStart;

    private NavMeshAgent _agent;
    private GameObject _player;
    private PlayerCharacteristics _playerCharacteristics;

    private float _timePastHit;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCharacteristics = _player.GetComponent<PlayerCharacteristics>();
        _agent = GetComponent<NavMeshAgent>();

        _hp = Random.Range(100, 200);

        _timePastHit = 0f;
        //print("У " + gameObject.name + " хп: " + _hp);
    }

    // Update is called once per frame
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
            _playerCharacteristics.allEnemies.Remove(gameObject);
        }

        if (_agent.velocity.magnitude > 0f)
        {
            // анимация бега
        }
        else
        {
            // анимация idle
        }

        if (_agressive) { Attack(); }
        else
        {
            // патрулировать
        }
    }

    private void Attack()
    {
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

    IEnumerator Battle() // вся проблема в корутине, она вызывается каждый кадр и выполняет анимацию удара постоянно каждый кадр
    {
        _agent.isStopped = true;
        _coroutStart = true;
        StartCoroutine(RotateToPlayer());
        while (_attack)
        {
            // вызов корутины цикл битвы
            // пока не произойдет условие, удар не начинать
            _animator.SetTrigger("Attack" + Random.Range(0, 4));
            yield return new WaitForSeconds(Random.Range(0.9f, 2.0f));
        }
        _agent.isStopped = false;
        _coroutStart = false;
    }
    IEnumerator CycleBattle()
    {
        // посмотрть видос про корутины от емералд павдер, там показано как приостановить корутину до выполнения условия

        // анимация доставания меча
        yield return new WaitForSeconds(1f);
        // переменную в тру, значит можно биться
        // битва окончена, меч в ножны
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
        // ragdoll
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
}