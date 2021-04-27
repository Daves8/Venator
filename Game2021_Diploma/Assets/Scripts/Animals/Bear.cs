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

    private bool _startCoroutine = false;

    // Start is called before the first frame update
    void Start()
    {
        //_bearAnim = GetComponent<Animator>();
        //_bearAgent = GetComponent<NavMeshAgent>();
        //_player = GameObject.FindGameObjectWithTag("Player");
        //_hunters = GameObject.FindGameObjectsWithTag("Hunter");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_startCoroutine)
        {
            _startCoroutine = true;
            // start coroutine()
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
