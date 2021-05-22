using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snake : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private ImportantBuildings _importBuild;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
