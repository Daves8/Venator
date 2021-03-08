using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float _hp;
    private bool _agressive = false;

    private int _numberAttack;
    private bool _canHit = true;
    private bool _dead = false;

    private GameObject _player;
    private NavMeshAgent _agent;

    private GameObject[] _allEnemy;

    private bool _hit = false;

    private void Start()
    {
        _allEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();

        _hp = Random.Range(100, 200);
        print("У " + gameObject.name + " хп: " + _hp);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_dead) { return; }
        if (_agressive)
        {
            Attack();
        }

        if (_agressive && Vector3.Distance(transform.position, _player.transform.position) >= 20f)
        {
            _agressive = false;
            _animator.SetBool("Aggressive", false);
            _agent.isStopped = true; // остановился
            Invoke("ChangeMoveAgent", 1.0f);
            _hit = false;
            _agent.SetDestination(new Vector3(926f, 3.69f, 978.2f));
            StopCoroutine("RotateToCamera"); // ЗАНИМАТЬСЯ ПАТРУЛИРОВАНИЕМ
        }

        if (_agent.velocity == Vector3.zero)
        {
            _animator.SetBool("IdleToWalk", false);
        }
        else
        {
            _animator.SetBool("IdleToWalk", true);
        }

        foreach (GameObject enemy in _allEnemy)
        {
            if (Vector3.Distance(enemy.transform.position, _player.transform.position) < 20f)
            {
                Enemy en = enemy.GetComponent<Enemy>();
                if (en._agressive)
                {
                    _agressive = true;
                    break;
                }
            }
        }

        if (_hp <= 0.0f)
        {
            Invoke("Delete", 300.0f);
            _dead = true;
            StopCoroutine("RotateToCamera");

            if (Random.Range(0, 2) == 0)
            {
                _animator.SetTrigger("Death");

            }
            else
            {
                _animator.SetTrigger("Death2");
            }
        }
    }

    private void Attack()
    {
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
    }

    private void ChangeMoveAgent()
    {
        _agent.isStopped = !_agent.isStopped;
    }
    private void HitTrue()
    {
        _canHit = true;
    }
    private void FirstAggressive()
    {
        //foreach (GameObject enemy in _allEnemy)
        //{
        //    if(Vector3.Distance(enemy.transform.position, _player.transform.position) < 20f)
        //    {
        //        Enemy en = enemy.GetComponent<Enemy>();
        //        en._agent.isStopped = true;
        //        en._animator.SetTrigger("SwordOn");
        //        en.Invoke("ChngAgr", 1.0f);
        //        en.Invoke("ChangeMoveAgent", 1.0f);
        //    }
        //}

        _agent.isStopped = true;
        _animator.SetTrigger("SwordOn");
        Invoke("ChngAgr", 1.0f);
        Invoke("ChangeMoveAgent", 1.0f);
    }
    private void ChngAgr()
    {
        _agressive = !_agressive;
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print(collision.gameObject.name);
    //    if (collision.gameObject.tag == "Sword")
    //    {
    //        _hp -= Random.Range(30, 70);
    //        print(collision.gameObject.name + " попал! Осталось хп: " + _hp);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            if (!_agressive && !_hit)
            {
                FirstAggressive();
            }
            _hit = true;

            _hp -= Random.Range(30, 70);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Knife")
        {
            if (!_agressive && !_hit)
            {
                FirstAggressive();
            }
            _hit = true;

            _hp -= Random.Range(10, 30);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Arrow")
        {
            if (!_agressive && !_hit)
            {
                FirstAggressive();
            }
            _hit = true;

            _hp -= Random.Range(80, 120);
            print("Стрела попала. Осталось хп: " + _hp);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}