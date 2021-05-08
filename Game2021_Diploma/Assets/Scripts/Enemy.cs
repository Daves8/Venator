using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Animator _animator;

    public float _hp;
    private bool _agressive = false;

    private int _numberAttack;
    private bool _canHit = true;
    private bool _death = false;

    private GameObject _player;
    private NavMeshAgent _agent;

    private GameObject[] _allEnemy;

    private Battle _battlePlayer;
    private PlayerCharacteristics _playerCharacteristics;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _allEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        _player = GameObject.FindGameObjectWithTag("Player");
        _battlePlayer = _player.GetComponent<Battle>();
        _playerCharacteristics = _player.GetComponent<PlayerCharacteristics>();
        _agent = GetComponent<NavMeshAgent>();

        _hp = Random.Range(100, 200);
        //print("У " + gameObject.name + " хп: " + _hp);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_death) { return; }
        if (_hp <= 0f)
        {
            _death = true;
            _agent.enabled = false;
            _playerCharacteristics.allEnemies.Remove(gameObject);
            StopCoroutine("RotateToCamera");

            if (Random.Range(0, 2) == 0) // переделать, чтобы не триггер а int
            {
                _animator.SetTrigger("Death");
            }
            else
            {
                _animator.SetTrigger("Death2");
            }

            Invoke("Death", 1.0f);
            Invoke("Delete", 300.0f);
        }

        if(_playerCharacteristics.isBattle && Vector3.Distance(_player.transform.position, transform.position) <= 20f)
        {
            _agressive = true;
            _playerCharacteristics.allEnemies.Add(gameObject);
        }
        else
        {
            _agressive = false;
            _playerCharacteristics.allEnemies.Remove(gameObject);
        }

        if (_agressive) { Attack(); }
    }

    private void Attack()
    {
        // от это все
        if (Vector3.Distance(transform.position, _player.transform.position) <= 20f)
        {
            //_battlePlayer.IsBattle = true;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= 1.5f && _canHit)
        {
            StartCoroutine("RotateToCamera");
            _agent.isStopped = true;
            Invoke("ChangeMoveAgent", 0.65f); // через 0.5 сек _agent.isStopped = false;

            _canHit = false;
            float nextHit = Random.Range(8, 21) / 10.0f;
            Invoke("HitTrue", nextHit);
            _numberAttack = Random.Range(1, 4); // 1-3
            switch (_numberAttack)
            {
                case 1:
                    _animator.SetTrigger("Attack1");
                    break;
                case 2:
                    _animator.SetTrigger("Attack2");
                    break;
                case 3:
                    _animator.SetTrigger("Attack3");
                    break;
                default:
                    break;
            }
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) < 20f && Vector3.Distance(transform.position, _player.transform.position) > 1.5f)
        {
            _animator.SetBool("Aggressive", true);
            _agent.SetDestination(_player.transform.position + new Vector3(Random.Range(-10, 10) / 10.0f, 0f, Random.Range(-10, 10) / 10.0f));
            // ИДТИ К ИГРОКУ
        }
        // удалить
    }

    IEnumerator RotateToCamera()
    {
        while (true)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            _agressive = true;
            _playerCharacteristics.allEnemies.Add(gameObject);
            _hp -= Random.Range(30, 70);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Knife")
        {
            _agressive = true;
            _playerCharacteristics.allEnemies.Add(gameObject);
            _hp -= Random.Range(10, 30);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Arrow")
        {
            _playerCharacteristics.allEnemies.Add(gameObject);
            _agressive = true;
            _hp -= Random.Range(30, 350);
            print("Стрела попала. Осталось хп: " + _hp);
        }
    }
    private void Death()
    {
        _animator.enabled = false;
        // ragdoll
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
}