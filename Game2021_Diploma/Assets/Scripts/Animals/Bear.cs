using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    public float hp;

    private Animator _bearAnim;
    private NavMeshAgent _bearAgent;

    private GameObject _player;
    private GameObject[] _hunters;

    private bool _die = false;
    private bool _agressive = false;
    private bool _startCoroutine = false;

    void Start()
    {
        _bearAnim = GetComponent<Animator>();
        _bearAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hunters = GameObject.FindGameObjectsWithTag("Hunter");

        hp = 500;
        StartCoroutine(Healing());
    }

    void Update()
    {
        if (_die) { return; }



        if (_agressive) { Attack(); }
        else { Walking(); }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void Attack()
    {

    }

    private void Walking()
    {

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
}