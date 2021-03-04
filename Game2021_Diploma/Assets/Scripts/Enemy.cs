using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float _hp;
    private bool _agressive = false;

    private int _numberAttack;

    private void Start()
    {
        _hp = Random.Range(100, 200);
        print("У " + gameObject.name + " хп: " + _hp);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_agressive)
        {
            Attack();
        }
        
        if (_agressive && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 20f)
        {
            _agressive = false;
        }

        if (_hp <= 0)
        {
            Invoke("Delete", 300.0f);

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
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2f)
        {
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