using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float _hp;
    private bool _agressive = false;

    private int _numberAttack;
    private bool _canHit = true;
    private bool _dead = false;

    private void Start()
    {
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

        if (_agressive && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 20f)
        {
            _agressive = false;
            StopCoroutine("RotateToCamera"); // ЗАНИМАТЬСЯ ПАТРУЛИРОВАНИЕМ
        }

        if (_hp <= 0)
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
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 1.5f && _canHit)
        {
            StartCoroutine("RotateToCamera");

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
        else if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 20f)
        {
            // ИДТИ К ИГРОКУ
        }
    }

    private void HitTrue()
    {
        _canHit = true;
    }

    IEnumerator RotateToCamera()
    {
        while (true)
        {
            Vector3 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
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
            _agressive = true;

            _hp -= Random.Range(30, 70);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Knife")
        {
            _agressive = true;

            _hp -= Random.Range(10, 30);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Arrow")
        {
            _agressive = true;

            _hp -= Random.Range(80, 120);
            print("Стрела попала. Осталось хп: " + _hp);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}