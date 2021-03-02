using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float _hp;

    private void Start()
    {
        _hp = Random.Range(100, 200);
        print("У " + gameObject.name + " хп: " + _hp);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_hp <= 0)
        {

            if (Random.Range(0, 2) == 0)
            {
                _animator.SetTrigger("Death");
            }
            else
            {
                _animator.SetTrigger("Death2");
            }
            Invoke("Delete", 300.0f);
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
            _hp -= Random.Range(30, 70);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
        else if (other.gameObject.tag == "Knife")
        {
            _hp -= Random.Range(10, 30);
            print(other.gameObject.name + " попал! Осталось хп: " + _hp);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
